using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WebApi.Domain;

namespace WebApiRepository
{
    public class Context : IdentityDbContext<WebApi.Domain.User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            # region estabelecendo relacoes de user com roles ( N usuarios com N permissoes )
            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(x => new { x.UserId, x.RoleId });

                //estabelecendo relacoes n para n ( N usuarios com N permissoes )
                userRole.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(key => key.RoleId).IsRequired();
                userRole.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId).IsRequired();
               
            });
            #endregion

            builder.Entity<Organizacao>(org =>
            {
                org.ToTable("Organizacao");
                org.HasKey(x => x.Id);

                org.HasMany<User>().WithOne().HasForeignKey(x => x.OrgId).IsRequired(false);
            });
        }
    }
}
