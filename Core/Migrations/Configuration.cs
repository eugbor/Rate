using Core.Entities;
using Core.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    /// <summary>
    /// Configuration
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<Core.Context>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
            //AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// Generates context
        /// </summary>
        /// <param name="context">Context</param>
        protected override void Seed(Core.Context context)
        {
            if (context.Roles.FirstOrDefault(role => role.Name == Core.Roles.Moderator) == null)
                context.Roles.Add(new IdentityRole(Core.Roles.Moderator));
            if (context.Roles.FirstOrDefault(role => role.Name == Core.Roles.TeamMember) == null)
                context.Roles.Add(new IdentityRole(Core.Roles.TeamMember));

            var userManager = new UserManager(context);
            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //roleManager.Create(new IdentityRole { Name = "Admin"});
            //roleManager.Create(new IdentityRole { Name = "Employee" });
            //roleManager.Create(new IdentityRole { Name = "User" });

            // создаем пользователей
            if (userManager.FindByEmail("somemail@mail.ru") == null)
            {
                var admin = new User { Email = "somemail@mail.ru", UserName = "somemail@mail.ru" };
                string password = "123456";
                var result = userManager.Create(admin, password);

                // если создание пользователя прошло успешно
                if (result.Succeeded)
                {
                    // добавляем для пользователя роль
                    userManager.AddToRole(admin.Id, Core.Roles.Moderator);
                }
            }

            base.Seed(context);

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
