using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDBContext : IdentityDbContext
    {
        public AuthDBContext(DbContextOptions<AuthDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //creating Writer and Reader roles
            var readerRoleId = "5b4e5cc7-6aa3-46ef-8592-8f09dfe0e5fc";
            var writerRoleID = "d09439a1-d6e6-458f-ad27-93278b2aa779";
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id=readerRoleId,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper(),
                    ConcurrencyStamp=readerRoleId
                },
                new IdentityRole()
                {
                    Id=writerRoleID,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper(),
                    ConcurrencyStamp=writerRoleID
                }
            };
            //seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            //Create Admin users
            var adminUserId = "09c6a656-23c3-4413-ab33-0e93ef21f031";
            var admin = new IdentityUser()
            {
                Id=adminUserId,
                UserName="admin@email.com",
                Email="admin@email.com",
                NormalizedEmail = "admin@email.com".ToUpper(),
                NormalizedUserName= "admin@email.com".ToUpper()
            };
            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");
            builder.Entity<IdentityUser>().HasData(admin);

            //Provide the roles to Admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                  UserId=adminUserId,
                  RoleId=readerRoleId
                },
                new()
                {
                  UserId=adminUserId,
                  RoleId=writerRoleID
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
