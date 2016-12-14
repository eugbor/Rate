using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Core.Managers
{
    /// <summary>
    /// Comment manager.
    /// </summary>
    public class CommentManager : BaseManager<Comment>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="context">Context.</param>
        internal CommentManager(Context context)
            : base(context)
        {
        }

        /// <summary>
		/// Creates instance of comment manager.
		/// </summary>
		/// <param name="options">Options.</param>
		/// <param name="owinContext">Context.</param>
		/// <returns>Comment manager.</returns>
        public static CommentManager Create(IdentityFactoryOptions<CommentManager> options, IOwinContext owinContext)
        {
            var context = owinContext.Get<Context>();

            return new CommentManager(context);
        }

        /// <summary>
        /// Saves comment.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="story">Task.</param>
        /// <param name="text">Comment.</param>
        public void Save(User user, Story story, string text)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (story == null)
                throw new ArgumentNullException(nameof(story));

            if (!story.IsActive)
                return;

            Comment comment = new Comment()
            {
                Text = text,
                CreatedDate = DateTime.Now,
                Story = story,
                User = user
            };

            Set.Add(comment);

            Context.SaveChanges();
        }

        /// <summary>
        /// Gets list of comments.
        /// </summary>
        /// <param name="story">Task.</param>
        /// <returns>List of comments.</returns>
        public List<Comment> GetList(Story story)
        {
            return Set.Where(el => el.Story.Id == story.Id).ToList();
        }
    }
}
