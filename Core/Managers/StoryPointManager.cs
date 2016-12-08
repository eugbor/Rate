using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Core.Managers
{
    public class StoryPointManager : BaseManager<StoryPoint>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Context</param>
        internal StoryPointManager(Context context)
            : base(context)
        {
        }

        public static StoryPointManager Create(IdentityFactoryOptions<StoryPointManager> options,
            IOwinContext owinContext)
        {
            var context = owinContext.Get<Context>();

            return new StoryPointManager(context);
        }

        public void Save(User user, Story story, string value)
        {
            if (story.IsActive == false){}

            StoryPoint storyPoint = new StoryPoint();

            if (storyPoint.Value.Contains(value))
                storyPoint.Value = value;

            if (storyPoint.Id <= 0)
            {
                storyPoint.Value = value;
                Set.Add(storyPoint);
            }
                

            Context.SaveChanges();
        }

        public List<StoryPoint> GetList(Story story)
        {
            var list = new List<StoryPoint>();
            var storyPoints = new StoryPoint();

            if (story != null)
            {
                storyPoints.Story = story;
                list.Add(storyPoints);
            }

            return list;
        }

    }
}
