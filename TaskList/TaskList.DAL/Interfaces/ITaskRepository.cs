using TaskList.DAL.Models;

namespace TaskList.DAL.Interfaces
{
    /// <summary>
    /// Interface that describes task repository
    /// </summary>
    /// <seealso cref="TaskList.DAL.Interfaces.IRepository{TaskList.DAL.Models.TaskModel}" />
    public interface ITaskRepository : IRepository<TaskModel>
    {
    }
}
