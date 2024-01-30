using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserAPI.Abstracts;
using UserAPI.Data;
using UserAPI.Data.Dto;
using UserAPI.Data.Entity;
using UserAPI.Data.Model;

namespace UserAPI.Infrastructure.Services
{ 
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly DbContextOptions<AppDbContext> _options;
        public UserRepository(IMapper mapper, DbContextOptions<AppDbContext> options)
        {
            _mapper = mapper;
            _options = options;
        }

        public void UserAdd(string name, string password, RoleId roleId)
        {
            using (var context = new AppDbContext(_options))
            {
                if(roleId == RoleId.Administrator)
                {
                    if(AdminExist())
                    {
                        throw new Exception("Администратор может быть только один");
                    }
                }
                User user = new User();
                user.Name = name;
                user.RoleId = roleId;

                user.Salt = new byte[16];
                new Random().NextBytes(user.Salt);

                var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();

                SHA512 shaM = new SHA512Managed();

                user.Password = shaM.ComputeHash(data);
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public UserDto UserCheck(string name, string password)
        {
            using (var context = new AppDbContext(_options))
            {
                var user = context.Users.FirstOrDefault(u => u.Name == name);

                if(user == null)
                {
                    throw new Exception("User not found");
                }

                var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();
                SHA512 shaM = new SHA512Managed();

                var bpassword = shaM.ComputeHash(data);

                if(user.Password.SequenceEqual(bpassword))
                {
                    return _mapper.Map<UserDto>(user);
                }
                else
                {
                    throw new Exception("Wrong password");
                }
            }
        }
        public bool UserExists(string name)
        {
            using(var context = new AppDbContext(_options))
            {
                var user = context.Users.FirstOrDefault(u => u.Name == name);
                if (user == null)
                    return false;
                return true;
            }
        }
        public bool IsAdmin(string name)
        {
            using (var context = new AppDbContext(_options))
            {
                var user = context.Users.FirstOrDefault(u => u.Name == name);
                if (user == null)
                    return false;
                if (user.RoleId == RoleId.Administrator)
                    return true;
                return false;
            }
        }
        public bool AdminExist()
        {
            using (var context = new AppDbContext(_options))
            {
                int count = context.Users.Count(u => u.RoleId == RoleId.Administrator);
                if (count > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public List<UserDto> GetUsers()
        {
            using(var context = new AppDbContext(_options))
            {
                return context.Users.Select(u => _mapper.Map<UserDto>(u)).ToList();
            }
        }
        public bool DeleteUserByEmail(string email)
        {
            using (var context = new AppDbContext(_options))
            {
                User user = context.Users.FirstOrDefault(u => u.Name == email);
                if (user == null)
                    return false;
                context.Users.Remove(user);
                if(context.SaveChanges() == 0)
                    return false;
                return true;
            }
        }
        public Guid GetUserId(string email)
        {
            using (var context = new AppDbContext(_options))
            {
                User user = context.Users.FirstOrDefault(u => u.Name == email);
                return user.Guid;
            }
        }
    }
}
