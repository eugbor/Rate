using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Core.Managers
{
    /// <summary>
    /// Story point manager.
    /// </summary>
    public class StoryPointManager : BaseManager<StoryPoint>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="context">Context.</param>
        internal StoryPointManager(Context context)
            : base(context)
        {
        }

        /// <summary>
		/// Creates instance of story point manager.
		/// </summary>
		/// <param name="options">Options.</param>
		/// <param name="owinContext">Context.</param>
		/// <returns>Story point manager.</returns>
        public static StoryPointManager Create(IdentityFactoryOptions<StoryPointManager> options,
            IOwinContext owinContext)
        {
            var context = owinContext.Get<Context>();

            return new StoryPointManager(context);
        }

        /// <summary>
        /// Saves estimate.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="story">Task.</param>
        /// <param name="value">Estimate.</param>
        public void Save(User user, Story story, string value)
        {
            if (!story.IsActive)
                return;

            StoryPoint storyPoint = Set.FirstOrDefault(el => el.User.Id == user.Id && el.Story.Id == story.Id);

            if (storyPoint != null)
            {
                storyPoint.Value = value;
                Context.SaveChanges();
                return;
            }

            storyPoint = new StoryPoint
            {
                CreatedDate = DateTime.Now,
                Story = story,
                User = user
            };

            Set.Add(storyPoint);

            Context.SaveChanges();
        }

        /// <summary>
        /// Gets list of story point.
        /// </summary>
        /// <param name="story">Task.</param>
        /// <returns>List of story point.</returns>
        public List<StoryPoint> GetList(Story story)
        {
            if (story == null)
                throw new ArgumentNullException(nameof(story));
           
            if (!story.IsActive)
                return new List<StoryPoint>();

            var countActiveUser = Context.Users.Count(el => el.IsActive);

            List<StoryPoint> listStoryPoints = Set.Where(el => el.Story.Id == story.Id).ToList();

            if(countActiveUser != listStoryPoints.Count)
                return new List<StoryPoint>();
            
            Dictionary<int, int> points = new Dictionary<int, int>(); 

            foreach (var point in listStoryPoints)
            {
                int number;

                if (!int.TryParse(point.Value, out number))
                    return listStoryPoints;

                if (points.ContainsKey(number))
                {
                    points[number]++;
                }
                else
                {
                    points.Add(number, 1);
                }
            }
            
            if (points.Count > 2)
                return listStoryPoints;

            int maxPoint = points.Keys.Max();

            if (points[maxPoint] == 1)
                return listStoryPoints;

            return new List<StoryPoint>
            {
                new StoryPoint
                {
                    Value = maxPoint.ToString()
                }
            };
        }
    }
}
