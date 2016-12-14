using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Core.Entities
{
    /// <summary>
    /// User.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Ctor.
        /// </summary>
        public User()
        {
            IsActive = true;
        }

        /// <summary>
        /// Date of creation.
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}