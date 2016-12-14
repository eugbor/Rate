using Core.Entities;
using Core.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    /// <summary>
    /// Configuration.
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<Core.Context>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
        }

        /// <summary>
        /// Generates context.
        /// </summary>
        /// <param name="context">Context.</param>
        protected override void Seed(Core.Context context)
        {
            if (context.Roles.FirstOrDefault(role => role.Name == Core.Roles.Moderator.ToString()) == null)
                context.Roles.Add(new IdentityRole(Core.Roles.Moderator.ToString()));
            if (context.Roles.FirstOrDefault(role => role.Name == Core.Roles.TeamMember.ToString()) == null)
                context.Roles.Add(new IdentityRole(Core.Roles.TeamMember.ToString()));

            var userManager = new UserManager(context);

            // Create users.
            if (userManager.FindByEmail("somemail@mail.ru") == null)
            {
                var admin = new User { Email = "somemail@mail.ru", UserName = "somemail@mail.ru" };
                string password = "123456";
                var result = userManager.Create(admin, password);
               
                if (result.Succeeded)
                {
                    userManager.AddToRole(admin.Id, Core.Roles.Moderator.ToString());
                }
            }

            base.Seed(context);
        }
    }
}
