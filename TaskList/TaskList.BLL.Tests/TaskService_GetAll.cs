using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using TaskList.BLL.Interfaces;
using TaskList.BLL.Services;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.BLL.Tests
{
    [TestFixture]
    public class TaskService_GetAll
    {
        private readonly ITaskService _taskService;
        private readonly Mock<ITaskRepository> _taskRepositoryMock;

        public TaskService_GetAll()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);
        }

        [Test]
        public void ShouldBeThreeTasks()
        {
            // Arange
            var firstTask = new TaskModel { Id = 1, Priority = 1, Name = "First task" };
            var secondTask = new TaskModel { Id = 2, Priority = 4, Name = "Second task" };
            var thirdTask = new TaskModel { Id = 3, Priority = 3, Name = "Third task" };
            _taskRepositoryMock.ResetCalls();
            _taskRepositoryMock
                .Setup(x => x.Get(It.Is<Func<TaskModel, bool>>(p => p == null)))
                .Returns(
                    (Func<TaskModel, bool> p) =>
                        new List<TaskModel> { firstTask, secondTask, thirdTask });

            // Act
            var result = _taskService.GetAll();

            // Assert
            _taskRepositoryMock
                .Verify(c => c.Get(
                    It.Is<Func<TaskModel, bool>>(predicate => predicate == null)),
                    Times.Once());

            Assert.AreEqual(result.Count(), 3);

            var last = result.Last();
            Assert.AreEqual(last.Id, secondTask.Id);
            Assert.AreEqual(last.Name, secondTask.Name);
            Assert.AreEqual(last.Priority, secondTask.Priority);
        }

        [Test]
        public void ShouldBeZeroTasks()
        {
            // Arange
            _taskRepositoryMock.ResetCalls();
            _taskRepositoryMock
                .Setup(x => x.Get(It.Is<Func<TaskModel, bool>>(p => p == null)))
                .Returns(
                    (Func<TaskModel, bool> p) =>
                        new List<TaskModel> { });

            // Act
            var result = _taskService.GetAll();

            // Assert
            _taskRepositoryMock
                .Verify(c => c.Get(
                    It.Is<Func<TaskModel, bool>>(predicate => predicate == null)),
                    Times.Once());

            Assert.AreEqual(result.Count(), 0);
        }
    }
}
