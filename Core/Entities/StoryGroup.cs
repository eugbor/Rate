using System.Collections.Generic;

namespace Core.Entities
{
    /// <summary>
    /// Task group
    /// </summary>
    public class StoryGroup : Entity
    {
        /// <summary>
        /// List of task group
        /// </summary>
        public virtual List<Story> Stories { get; set; }

        public bool IsActive { get; set; }
        
        /// <summary>
        /// Ctor
        /// </summary>
        public StoryGroup()
        {
            Stories = new List<Story>();
            IsActive = true;
        }
    }
}