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
            if (story == null)
                throw new ArgumentNullException(nameof(story));

            // активна ли задача? или (задачи в активном наборе, т.е. если все задачи активны, то и набор задач активен)
            if (!(story.IsActive))
                return new List<StoryPoint>();

            // активен ли участник?
            if (!(story.User.IsActive))
                return new List<StoryPoint>();

            //Если оценки всех участников совпадают или отличаются на одну ступень (для чисел),
            //задача показывает итоговую оценку как максимальную из двух. Это правило не работает,
            //если только один участник указал оценку выше, чем остальные.
            
            List<StoryPoint> listSP = new List<StoryPoint>();
            
            IEnumerable<StoryPoint> list = listSP.Where(el => el.Story.Id == story.Id);

            List<StoryPoint> listStoryPoints = list as List<StoryPoint> ?? list.ToList();

            int number; // число, для проверки (является ли строка числом)

            if (listStoryPoints.Any(storyPoints => !(int.TryParse(storyPoints.Value, out number))))
                return listStoryPoints;

            List<string> estimate = StoryPoint.Estimates; // список значений оценок

            List<int> listInt = new List<int>(); // список оценок в виде чисел

            StoryPoint maxStoryPoint = new StoryPoint();
            
            for (int i = 0; i < listStoryPoints.Count; i++)
            {
                if (int.Parse(listStoryPoints[i].Value) == int.Parse(listStoryPoints[i + 1].Value))
                    listInt.Add(int.Parse(listStoryPoints[i].Value));


                int index = estimate.IndexOf(listStoryPoints[i].Value); // определяет индекс оценки в списке значений оценок

                if ((index - 1) >= 0 || (index + 1) <= (estimate.Count - 5))
                {
                    if (int.Parse(estimate[index + 1]) == int.Parse(listStoryPoints[i + 1].Value) ||
                        int.Parse(estimate[index - 1]) == int.Parse(listStoryPoints[i + 1].Value))
                    {
                        listInt.Add(int.Parse(listStoryPoints[i].Value));
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

            if (indexEstimate < 0 || indexEstimate > 3)
                return listStoryPoints;

            listStoryPoints.RemoveAll(el => el.Story.Id == story.Id);
            listInt.Sort();
            maxStoryPoint.Value = listInt[listInt.Count - 1].ToString();
            listStoryPoints.Add(maxStoryPoint);
            
            
            return listStoryPoints;
        }
    }
}
