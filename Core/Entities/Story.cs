using System;

namespace Core.Entities
{
    /// <summary>
    /// Model of task.
    /// </summary>
    public class Story : Entity
    {
        /// <summary>
        /// Gets or sets a value indicating whether the task is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Ctor.
        /// </summary>
        public Story()
        {
            IsActive = true;
        }

        /// <summary>
        /// User who creates a set of tasks.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Link to the bug tracker.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Date of task creations.
        /// </summary>
        public DateTime Date { get; set; }
    }
}