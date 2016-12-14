using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Core.Managers
{
    /// <summary>
	/// User manager.
	/// </summary>
    public class UserManager : UserManager<User, String>
    {
        /// <summary>
        /// User context.
        /// </summary>
        private Context Context { get; set; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="context">User context.</param>
        internal UserManager(Context context)
            : this(new UserStore<User>(context))
        {
            Context = context;
        }

        /// <summary>
		/// Ctor.
		/// </summary>
		/// <param name="store">User storage.</param>
        internal UserManager(IUserStore<User> store)
            : base(store)
        {
        }

        /// <summary>
        /// Creates manager instance. 
        /// </summary>
        /// <param name="options">Options.</param>
        /// <param name="owinContext">Context.</param>
        /// <returns>User manager.</returns>
        public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext owinContext)
        {
            var manager = new UserManager(owinContext.Get<Context>());

            // Configure validation logic for the usernames.
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for the passwords.
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireDigit = true
            };

            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var dataProtectionProvider = options.DataProtectionProvider;

            if (dataProtectionProvider != null)
                manager.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("Identity"));

            return manager;
        }

        /// <summary>
        /// Creates user.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns>Saved user.</returns>
        public override Task<IdentityResult> CreateAsync(User user)
        {
            user.CreatedDate = DateTime.Now;
            return base.CreateAsync(user);
        }

        /// <summary>
        /// Deletes user.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns>Removed user.</returns>
        public override Task<IdentityResult> DeleteAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return base.DeleteAsync(user);
        }

        /// <summary>
        /// Generates list of users on role.
        /// </summary>
        /// <param name="roleName">Role.</param>
        /// <returns>List of users on role.</returns>
        public List<User> GetListByRole(string roleName)
        {
            var role = Context.Roles.FirstOrDefault(el => el.Name == roleName);
            if (role == null)
                return new List<User>();

            return Users.Where(el => el.Roles.Any(r => r.RoleId == role.Id)).ToList();
        }

        /// <summary>
        /// Changes password.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>Identity result.</returns>
        public async Task<IdentityResult> ChangePasswordAsync(string id, string newPassword)
        {
            User user = await FindByIdAsync(id);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var store = this.Store as IUserPasswordStore<User, String>;
            if (store == null)
            {
                var errors = new string[] { "Current UserStore doesn't implement IUserPasswordStore" };
                return IdentityResult.Failed(errors);
            }

            var newPasswordHash = this.PasswordHasher.HashPassword(newPassword);
            await store.SetPasswordHashAsync(user, newPasswordHash);
            await store.UpdateAsync(user);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Creats teammember.
        /// </summary>
        /// <param name="current">Current user.</param>
        /// <param name="user">New user.</param>
        /// <param name="password">Password.</param>
        /// <returns>Return a value indicating whether the user is creates.</returns>
        public bool Create(User current, User user, string password)
        {
            if (!GetRolesAsync(current.Id).Result.Contains(Roles.Moderator.ToString()))
                return false;
            
            if (user != null)
            {
                var teamMember = new User { Email = user.Email, UserName = user.Name };
                
                var result = CreateAsync(teamMember, password).Result;
                
                if (result.Succeeded)
                    result = AddToRoleAsync(teamMember.Id, Core.Roles.TeamMember.ToString()).Result;

                if (!result.Succeeded)
                    return false;
            }

            return true;
        }
    }
}