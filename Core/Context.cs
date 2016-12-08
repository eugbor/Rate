using System.Data.Entity;
using Core.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Core
{
    /// <summary>
    /// Data context for the database
    /// </summary>
    public class Context : IdentityDbContext<User>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public Context()
            : base("DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        static Context()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Migrations.Configuration>());
        }

        /// <summary>
        /// Creates a context
        /// </summary>
        /// <returns>Context</returns>
        public static Context Create()
        {
            return new Context();
        }

        /// <summary>
        /// Interface access to the collection of comments
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// Interface access to the collection tasks
        /// </summary>
        public DbSet<Story> Stors { get; set; }

        /// <summary>
        /// Interface access to the collection set of tasks
        /// </summary>
        public DbSet<StoryGroup> StoryGroups { get; set; }
    }
}