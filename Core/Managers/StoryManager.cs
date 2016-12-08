using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Core.Managers
{
    /// <summary>
    /// Менеджер для работы с задачами
    /// </summary>
    public class StoryManager : BaseManager<Story>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Context</param>
        internal StoryManager(Context context)
            : base(context)
        {
        }

        public static StoryManager Create(IdentityFactoryOptions<StoryManager> options, IOwinContext owinContext)
        {
            var context = owinContext.Get<Context>();

            return new StoryManager(context);
        }

        public void CreateGroup(List<Story> stories)
        {
            StoryGroup storyGroup = new StoryGroup();
            storyGroup.Stories = stories;

            storyGroup.IsActive = true;

            Context.StoryGroups.Add(storyGroup);

            Context.SaveChanges();
        }

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

        public StoryGroup GetGroup(int id)
        {
            var group = Get(id);
            if (group == null)
                return new StoryGroup();

            var storyGroup = new StoryGroup();

            if(storyGroup.Stories.Contains(group))
                return storyGroup;

            storyGroup.Stories.Add(group);
            
            return storyGroup;
        }

        public Story GetNext(StoryGroup storyGroup, int storyId)
        {
            var story = Get(storyId);
            if (story == null)
                return new Story();

            if (storyGroup.Stories.IndexOf(story) == storyGroup.Stories.Count - 1)
                return Get(storyGroup.Stories.Count - storyGroup.Stories.IndexOf(story));

           return Get(storyGroup.Stories.IndexOf(story) + 1);
        }

        public Story GetLast(StoryGroup storyGroup, int storyId)
        {
            var story = Get(storyId);
            if (story == null)
                return new Story();

            if (storyGroup.Stories.IndexOf(story) == (storyGroup.Stories.Count - storyGroup.Stories.IndexOf(story)))
                return Get(storyGroup.Stories.Count - 1);

            return Get(storyGroup.Stories.IndexOf(story) - 1);
        }
    }
}
