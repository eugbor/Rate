using System;

namespace Core.Entities
{
    /// <summary>
    /// Task comment.
    /// </summary>
    public class Comment: Entity
    {
        /// <summary>
        /// User.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Task.
        /// </summary>
        public virtual Story Story { get; set; }

        /// <summary>
        /// Comment.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Date of creation.
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}