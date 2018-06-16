using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using TaskList.BLL.Dtos;
using TaskList.BLL.Interfaces;

using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.BLL.Services
{
    /// <summary>
    /// Class implementing task service functionality
    /// </summary>
    /// <seealso cref="TaskList.BLL.Interfaces.ITaskService" />
    public class TaskService : ITaskService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private ITaskRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Creates the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>
        /// The created task.
        /// </returns>
        public TaskDto Create(TaskDto dto)
        {
            var taskToCreate = Mapper.Map<TaskModel>(dto);
            var lowerPriority = _repository
                .Get()
                .OrderBy(x => x.Priority)
                .Select(x => x.Priority)
                .LastOrDefault();

            taskToCreate.Priority = lowerPriority + 1;
            var createdTask = _repository.Create(taskToCreate);

            return Mapper.Map<TaskDto>(createdTask);
        }

        /// <summary>
        /// Deletes the specified task identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        public void Delete(int taskId)
        {
            _repository.DeleteById(taskId);
        }

        /// <summary>
        /// Downs the priority.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        public void DownPriority(int taskId)
        {
            SwapWithNearestItem(taskId, (task, priority) => task.Priority > priority);
        }

        /// <summary>
        /// Ups the priority.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        public void UpPriority(int taskId)
        {
            SwapWithNearestItem(taskId, (task, priority) => task.Priority < priority);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>
        /// The list of tasks.
        /// </returns>
        public IEnumerable<TaskDto> GetAll()
        {
            return _repository
                .Get()
                .OrderBy(x => x.Priority)
                .Select(x => Mapper.Map<TaskDto>(x))
                .ToList();
        }

        /// <summary>
        /// Sets the priority.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="priority">The priority.</param>
        public void SetPriority(int taskId, int priority)
        {
            if (priority <= 0)
            {
                return;
            }

            var taskToPrioritize = GetTaskById(taskId);
            var tasksToUpdatePriority = _repository
                .Get(x => priority <= x.Priority && x.Id != taskToPrioritize.Id)
                .OrderBy(x => x.Priority)
                .ToList();

            taskToPrioritize.Priority = priority;

            tasksToUpdatePriority = ShiftTasksDueToPriority(priority, tasksToUpdatePriority).ToList();

            tasksToUpdatePriority.Add(taskToPrioritize);

            _repository.BulkUpdate(tasksToUpdatePriority);
        }

        /// <summary>
        /// Shifts the tasks due to priority.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="tasks">The tasks.</param>
        private IEnumerable<TaskModel> ShiftTasksDueToPriority(int priority, IEnumerable<TaskModel> tasks)
        {
            var similar = tasks.FirstOrDefault(t => t.Priority == priority);
            if (similar == null)
            {
                yield break;
            }

            similar.Priority++;
            yield return similar;

            foreach (var task in ShiftTasksDueToPriority(
                similar.Priority,
                tasks.Where(t => t.Id != similar.Id).ToList()))
            {
                yield return task;
            };
        }

        /// <summary>
        /// Gets the task by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The found task.</returns>
        private TaskModel GetTaskById(int id)
        {
            return _repository.Get(x => x.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Swaps the with nearest item.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="nearestItemByPriorityPredicate">The nearest item by priority predicate.</param>
        private void SwapWithNearestItem(int taskId, Func<TaskModel, int, bool> nearestItemByPriorityPredicate)
        {
            var taskToChangePriority = GetTaskById(taskId);
            var taskToSwapWith = _repository.Get(x => nearestItemByPriorityPredicate(x, taskToChangePriority.Priority)).FirstOrDefault();

            if (taskToSwapWith == null)
            {
                return;
            }

            SwapPriority(taskToSwapWith, taskToChangePriority);

            _repository.BulkUpdate(new List<TaskModel> { taskToSwapWith, taskToChangePriority });
        }

        /// <summary>
        /// Swaps the priotity.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        private void SwapPriority(TaskModel first, TaskModel second)
        {
            var store = first.Priority;
            first.Priority = second.Priority;
            second.Priority = store;
        }
    }
}
