using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Core.Managers
{
    public class StoryPointsManager : BaseManager<Story>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Context</param>
        internal StoryPointsManager(Context context)
            : base(context)
        {
        }

        public static StoryPointsManager Create(IdentityFactoryOptions<StoryPointsManager> options,
            IOwinContext owinContext)
        {
            var context = owinContext.Get<Context>();

            return new StoryPointsManager(context);
        }

        public void Save(User user, Story story, string value)
        {
            
        }

        public List<StoryPoints> GetList(Story story)
        {
            return new List<StoryPoints>();
        }

    }
}
