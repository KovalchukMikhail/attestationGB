using UserAPI.Data.Entity;

namespace UserAPI.Data.Model
{
    public class User
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = null!;
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public RoleId RoleId {  get; set; }
        public virtual Role Role { get; set; }
    }
}
