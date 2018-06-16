using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.DAL.Repositories
{
    /// <summary>
    /// Class implementing task repository functionality
    /// </summary>
    /// <seealso cref="TaskList.DAL.Interfaces.ITaskRepository" />
    public class TaskRepositoryMock : ITaskRepository
    {
        /// <summary>
        /// The static task source
        /// </summary>
        private static ICollection<TaskModel> _taskSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRepositoryMock"/> class.
        /// </summary>
        public TaskRepositoryMock()
        {
            _taskSource = new List<TaskModel>
            {
                new TaskModel
                {
                    Id = 1,
                    Priority = 1,
                    Name = "Write hight-quality code"
                },
                new TaskModel
                {
                    Id = 2,
                    Priority = 2,
                    Name = "Read Angular For professionals book"
                },
                new TaskModel
                {
                    Id = 3,
                    Priority = 3,
                    Name = "Relax on the Maldives"
                }
            };
        }

        /// <summary>
        /// Gets the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The list of tasks.</returns>
        public IEnumerable<TaskModel> Get(Func<TaskModel, bool> predicate = null)
        {
            if(predicate == null)
            {
                return _taskSource.ToList();
            }

            return _taskSource.Where(predicate).ToList();
        }

        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The created model.</returns>
        public TaskModel Create(TaskModel model)
        {
            model.Id = _taskSource
                .OrderBy(x => x.Id)
                .Select(x => x.Id)
                .LastOrDefault() + 1;

            _taskSource.Add(model);

            return Get(x => x.Id == model.Id).FirstOrDefault();
        }

        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The updated model.</returns>
        public TaskModel Update(TaskModel model)
        {
            var itemToUpdate = Get(x => x.Id == model.Id).FirstOrDefault();

            if (itemToUpdate == null)
            {
                return Create(model);
            }

            itemToUpdate = Mapper.Map<TaskModel>(model);

            return itemToUpdate;
        }

        /// <summary>
        /// Deletes by the identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void DeleteById(int id)
        {
            var itemToDelete = Get(x => x.Id == id).FirstOrDefault();

            if (itemToDelete == null)
            {
                return;
            }

            _taskSource.Remove(itemToDelete);
        }

        /// <summary>
        /// Bulks the update.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <returns>The update list.</returns>
        public IEnumerable<TaskModel> BulkUpdate(IEnumerable<TaskModel> models)
        {
            // there should be some king of execute in context to optimize queries to database

            foreach (var model in models)
            {
                Update(model);
            }

            return Get(x => models.Any(m => x.Id == m.Id));
        }
    }
}
