using System.Collections.Generic;

namespace Core.Entities
{
    /// <summary>
    /// Group of task.
    /// </summary>
    public class StoryGroup : Entity
    {
        /// <summary>
        /// List of task group.
        /// </summary>
        public virtual List<Story> Stories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the group is active.
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Ctor.
        /// </summary>
        public StoryGroup()
        {
            Stories = new List<Story>();
            IsActive = true;
        }
    }
}