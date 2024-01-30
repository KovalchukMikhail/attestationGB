using UserAPI.Data.Dto;
using UserAPI.Data.Entity;

namespace UserAPI.Abstracts
{
    public interface IUserRepository
    {
        public void UserAdd(string name, string password, RoleId roleId);
        public UserDto UserCheck(string name, string password);
        bool UserExists(string name);
        bool AdminExist();
        List<UserDto> GetUsers();
        bool DeleteUserByEmail(string email);
        bool IsAdmin(string name);
        Guid GetUserId(string email);
    }
}
