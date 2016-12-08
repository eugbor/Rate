using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    /// <summary>
    /// Interface base entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntity<T>
    {
        T Id { get; set; }
    }

    /// <summary>
    /// Basic entity
    /// </summary>
    public abstract class Entity : IEntity<int>
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}