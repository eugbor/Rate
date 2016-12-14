using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Core.Managers
{
    /// <summary>
    /// Story manager.
    /// </summary>
    public class StoryManager : BaseManager<Story>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="context">Story context.</param>
        internal StoryManager(Context context)
            : base(context)
        {
        }

        /// <summary>
		/// Creates instance of story manager.
		/// </summary>
		/// <param name="options">Options.</param>
		/// <param name="owinContext">Context.</param>
		/// <returns>Story manager.</returns>
        public static StoryManager Create(IdentityFactoryOptions<StoryManager> options, IOwinContext owinContext)
        {
            var context = owinContext.Get<Context>();

            return new StoryManager(context);
        }

        /// <summary>
        /// Generates a set of tasks.
        /// </summary>
        /// <param name="stories">List of tasks.</param>
        public void CreateGroup(List<Story> stories)
        {
            StoryGroup storyGroup = new StoryGroup();
            storyGroup.Stories = stories;

            storyGroup.IsActive = true;

            Context.StoryGroups.Add(storyGroup);

            Context.SaveChanges();
        }

        /// <summary>
        /// Changes activity.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="isActive">Activity flag.</param>
        /// <returns>Modified activity flag.</returns>
        public bool ChangeActiveSet(int id, bool isActive)
        {
            if (isActive && Context.StoryGroups.Count(el => el.IsActive) != 0)
                return false;

            var storyGroup = Context.StoryGroups.FirstOrDefault(el => el.Id == id);
            if (storyGroup == null)
                return false;
            
            storyGroup.IsActive = isActive;

            Context.SaveChanges();

            return true;
        }

        /// <summary>
        /// Gets a set of tasks.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Set of tasks.</returns>
        public StoryGroup GetGroup(int id)
        {
            return Context.StoryGroups.FirstOrDefault(el => el.Id == id);
        }

        /// <summary>
        /// Gets next task.
        /// </summary>
        /// <param name="storyGroup">Set of tasks.</param>
        /// <param name="storyId">Identifier.</param>
        /// <returns>Next task.</returns>
        public Story GetNext(StoryGroup storyGroup, int storyId)
        {
            if (storyGroup == null)
                throw new ArgumentNullException(nameof(storyGroup));

            int index = storyGroup.Stories.FindIndex(el => el.Id == storyId);
            if (index + 1 >= storyGroup.Stories.Count)
                return null;

            return storyGroup.Stories[index + 1];
        }

        /// <summary>
        /// Gets previous task.
        /// </summary>
        /// <param name="storyGroup">Set of tasks.</param>
        /// <param name="storyId">Identifier.</param>
        /// <returns>Previous task.</returns>
        public Story GetPrevious(StoryGroup storyGroup, int storyId)
        {
            if (storyGroup == null)
                throw new ArgumentNullException(nameof(storyGroup));

            int index = storyGroup.Stories.FindIndex(el => el.Id == storyId);
            if (index - 1 < 0)
                return null;

            return storyGroup.Stories[index - 1];
        }
    }
}
