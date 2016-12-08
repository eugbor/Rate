using System;

namespace Core.Entities
{
    /// <summary>
    /// Task
    /// </summary>
    public class Story : Entity
    {
        /// <summary>
        /// Status
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public Story()
        {
            IsActive = true;
        }

        /// <summary>
        /// User who creates a set of tasks
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Link to the bug tracker
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Date of creation task
        /// </summary>
        public DateTime Date { get; set; }
    }
}