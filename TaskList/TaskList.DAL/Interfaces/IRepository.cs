using System;
using System.Collections.Generic;

using TaskList.DAL.Models;

namespace TaskList.DAL.Interfaces
{
    /// <summary>
    /// Interface decribing repository functionality
    /// </summary>
    /// <typeparam name="T">The specific model to be used.</typeparam>
    public interface IRepository<T>
        where T : BaseModel
    {
        /// <summary>
        /// Gets by the predicate.
        /// </summary>
        /// <returns>The array of models.</returns>
        IEnumerable<T> Get(Func<T, bool> predicate = null);

        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The created model.</returns>
        T Create(T model);

        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The updated model.</returns>
        T Update(T model);

        /// <summary>
        /// Bulks the update.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <returns>The updated array.</returns>
        IEnumerable<T> BulkUpdate(IEnumerable<T> models);

        /// <summary>
        /// Deletes by the identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void DeleteById(int id);
    }
}
