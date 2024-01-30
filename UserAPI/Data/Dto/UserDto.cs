using UserAPI.Data.Entity;
using UserAPI.Data.Model;

namespace UserAPI.Data.Dto
{
    public class UserDto
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = null!;
        public RoleId RoleId { get; set; }
    }
}
