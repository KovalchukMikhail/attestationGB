using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAPI.Abstracts;
using UserAPI.Data.Dto;
using UserAPI.Data.Entity;
using UserAPI.Data.Model;
using UserAPI.Infrastructure;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public LoginController(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] LoginModel userLogin)
        {
            userLogin.Name = userLogin.Name.ToLower();
            try
            {
                UserDto userDto = _userRepository.UserCheck(userLogin.Name, userLogin.Password);
                var token = GenerateToken(userDto);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [AllowAnonymous]
        [HttpPost]
        [Route("AddAdmin")]
        public ActionResult AddAdmin([FromBody] LoginModel userLogin)
        {
            try
            {
                if (_userRepository.AdminExist())
                    return BadRequest("Пользователь с ролью \"Администратор\" уже существует");

                if (!DataChecker.IsEmail(userLogin.Name))
                    return BadRequest("Логин должен представлять электронную почту");

                if (!DataChecker.CheckPassword(userLogin.Password))
                    return BadRequest("Пароль должен содержать от 8 до 20 символов включая, заглавные, строчные латинские буквы и цифры");

                return AddUser(userLogin, RoleId.Administrator);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [AllowAnonymous]
        [HttpPost]
        [Route("AddUser")]
        public ActionResult AddUser([FromBody] LoginModel userLogin)
        {
            try
            {
                userLogin.Name = userLogin.Name.ToLower();
                if (!DataChecker.IsEmail(userLogin.Name))
                    return BadRequest("Логин должен представлять электронную почту");

                if (!DataChecker.CheckPassword(userLogin.Password))
                    return BadRequest("Пароль должен содержать от 8 до 20 символов включая, заглавные, строчные латинские буквы и цифры");

                return AddUser(userLogin, RoleId.User);

            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        private ActionResult AddUser(LoginModel userLogin, RoleId role)
        {
            try
            {
                if (_userRepository.UserExists(userLogin.Name))
                {
                    return BadRequest("Пользователь с таким электронным адресом уже существует");
                }
                _userRepository.UserAdd(userLogin.Name, userLogin.Password, role);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }
        private string GenerateToken(UserDto user)
        {
            var securityKey = new RsaSecurityKey(RsaCreator.GetPrivetKey());
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256Signature);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Guid.ToString()),
                new Claim(ClaimTypes.Role, user.RoleId.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
