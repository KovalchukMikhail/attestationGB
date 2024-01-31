using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserAPI.Abstracts;
using UserAPI.Data.Dto;
using UserAPI.Data.Entity;
using UserAPI.Data.Model;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestrictedController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public RestrictedController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("GetCurrentUserId")]
        [Authorize(Roles = "Administrator, User")]
        public IActionResult GetCurrentUserId()
        {
            try
            {
                var currentUser = GetCurrentUser();
                Guid userId = currentUser.Guid;
                return Ok(userId.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        [Route("GetUsers")]
        [Authorize(Roles = "Administrator, User")]
        public ActionResult<List<UserDto>> GetUsers()
        {
            try
            {
                return _userRepository.GetUsers();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeletUser")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteUserByEmail(string email)
        {
            try
            {
                if (!_userRepository.UserExists(email))
                    return BadRequest("Пользователя с таким электронным адресом не существует");
                if (_userRepository.IsAdmin(email))
                    return BadRequest("Предотвращена попытка удаления пользователя с правами \"Администратор\"");

                if (_userRepository.DeleteUserByEmail(email))
                    return Ok($"Успешно удален пользователь {email}");
                else
                    return StatusCode(500, "Не удалось удалить пользователя");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        private UserDto GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UserDto
                {
                    Guid = new Guid(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value),
                    RoleId = (RoleId)Enum.Parse(typeof(RoleId), userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value)
                };
            }
            return null;
        }
    }
}
