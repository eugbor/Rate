using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Core.Entities;

namespace Core.Managers
{
    /// <summary>
	/// Base manager
	/// </summary>
	/// <typeparam name="T">Entity type</typeparam>
    public abstract class BaseManager<T> : IDisposable
        where T : Entity
    {
        private DbSet<T> _set;

        /// <summary>
        /// Data context for the database
        /// </summary>
        protected Context Context { get; private set; }

        /// <summary>
        /// Check zero
        /// </summary>
        protected DbSet<T> Set => _set ?? (_set = Context.Set<T>());

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Context database</param>
        protected BaseManager(Context context)
        {
            Context = context;
        }

        /// <summary>
        /// Get entity of database
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual T Get(int id)
        {
            return Set.FirstOrDefault(el => el.Id == id);
        }

        /// <summary>
        /// Generates list entities
        /// </summary>
        /// <returns>List entities</returns>
        public virtual List<T> GetList()
        {
            return Set.ToList();
        }

        /// <summary>
        /// Adds object to database
        /// </summary>
        /// <param name="obj">Object</param>
        public virtual void Save(T obj)
        {
            if (obj.Id <= 0)
                Set.Add(obj);

            Context.SaveChanges();
        }

        /// <summary>
        /// Removes entity from the database by id
        /// </summary>
        /// <param name="id">Identifier</param>
        public virtual void Remove(int id)
        {
            var obj = Get(id);
            if (obj != null)
                Set.Remove(obj);

            Context.SaveChanges();
        }

        /// <summary>
        /// Releases resources
        /// </summary>
        public virtual void Dispose()
        {
            Context = null;
        }
    }
}