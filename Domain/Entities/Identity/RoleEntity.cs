using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class RoleEntity: IdentityRole<long>
    {
        public virtual ICollection<UserRoleEntity>? UserRoles { get; set; }
        public RoleEntity(){}
        public RoleEntity(string roleName) : base(roleName){}
    }
}
