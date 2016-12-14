using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    /// <summary>
    /// Interface of base entity.
    /// </summary>
    /// <typeparam name="T">Type of entity.</typeparam>
    public interface IEntity<T>
    {
        T Id { get; set; }
    }

    /// <summary>
    /// Base entity.
    /// </summary>
    public abstract class Entity : IEntity<int>
    {
        /// <summary>
        /// Entity identifier.
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}