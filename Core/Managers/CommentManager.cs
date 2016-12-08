using System.Collections.Generic;
using Core.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Core.Managers
{

    public class CommentManager : BaseManager<Comment>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Context</param>
        internal CommentManager(Context context)
            : base(context)
        {
        }

        public static CommentManager Create(IdentityFactoryOptions<CommentManager> options, IOwinContext owinContext)
        {
            var context = owinContext.Get<Context>();

            return new CommentManager(context);
        }

        public void Save(User user, Story story, string text)
        {
            if (story.IsActive == false) { }

            Comment comment = new Comment();

            comment.Text = text;
            Set.Add(comment);

            Context.SaveChanges();

        }

        public List<Comment> GetList(Story story)
        {
            var list = new List<Comment>();
            var comment = new Comment();

            if (story != null)
            {
                comment.Story = story;
                list.Add(comment);
            }

            return list;
        }
    }
}
