using Moq;
using NUnit.Framework;

using TaskList.BLL.Interfaces;
using TaskList.BLL.Services;
using TaskList.DAL.Interfaces;

namespace TaskList.BLL.Tests
{
    [TestFixture]
    public class TaskService_Delete
    {
        private readonly ITaskService _taskService;
        private readonly Mock<ITaskRepository> _taskRepositoryMock;

        public TaskService_Delete()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);
        }

        [Test]
        public void DeleteByIdShouldBeCalledWithSpecifiedId()
        {
            // Arange
            var taskId = 10;

            // Act
            _taskService.Delete(taskId);

            // Assert
            _taskRepositoryMock.Verify(c => c.DeleteById(It.Is<int>(id => id == taskId)), Times.Once());
        }
    }
}
