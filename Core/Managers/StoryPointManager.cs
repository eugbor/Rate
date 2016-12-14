using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            if (!(story.User.IsActive))
                return new List<StoryPoint>();

            StoryPoint storyPoint = new StoryPoint();
            if (storyPoint.Value == null)
                throw new ArgumentNullException(nameof(storyPoint.Value));

            //Если оценки всех участников совпадают или отличаются на одну ступень (для чисел),
            //задача показывает итоговую оценку как максимальную из двух. Это правило не работает,
            //если только один участник указал оценку выше, чем остальные.

            storyPoint = Set.FirstOrDefault(el => el.Story.Id == story.Id); // оценки всех участников

            List<StoryPoint> listSP = new List<StoryPoint>();

            listSP.Add(storyPoint); // список оценок всех участников

            var estimate = StoryPoint.Estimates;
            int number;
            List<int> listInt = new List<int>();
            StoryPoint maxStoryPoint = new StoryPoint();
           
            for (int i = 0; i < listSP.Count; i++)
            {
                for (int j = i + 1; j < listSP.Count; j++)
                {
                    int index = estimate.IndexOf(listSP[i].Value);

                    if (int.TryParse(listSP[i].Value, out number) && int.TryParse(listSP[j].Value, out number))
                    {
                        if ((int.Parse(listSP[i].Value) == int.Parse(listSP[j].Value)) ||
                            (int.Parse(estimate[index + 1]) == int.Parse(listSP[j].Value)) ||
                            (int.Parse(estimate[index - 1]) == int.Parse(listSP[j].Value)))
                        {
                            listInt.Add(int.Parse(listSP[j].Value));
                        }
                    }

                }
            }

            int indexEstimate = 0;
            for (int i = 0; i < listInt.Count; i++)
            {
                if (listInt[i] != listInt[i+1])
                {
                    indexEstimate++;
                }
            }

            if (indexEstimate > 1)
            {
                listInt.Sort();
                maxStoryPoint.Value = listInt[listInt.Count - 1].ToString();
                listSP.Add(maxStoryPoint);
            }
            
            
            return listSP;
        }
    }
}
