using System.Collections.Generic;

using TaskList.BLL.Dtos;

namespace TaskList.BLL.Interfaces
{
    public interface ITaskService
    {
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>The list of tasks.</returns>
        IEnumerable<TaskDto> GetAll();

        /// <summary>
        /// Creates the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>The created task.</returns>
        TaskDto Create(TaskDto dto);

        /// <summary>
        /// Ups the priority.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        void UpPriority(int taskId);

        /// <summary>
        /// Downs the priority.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        void DownPriority(int taskId);

        /// <summary>
        /// Sets the priority.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="priority">The priority.</param>
        void SetPriority(int taskId, int priority);

        /// <summary>
        /// Deletes the specified task identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        void Delete(int taskId);
    }
}
