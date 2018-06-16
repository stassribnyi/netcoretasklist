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
    public class TaskService_SetPriority
    {
        private readonly ITaskService _taskService;
        private readonly Mock<ITaskRepository> _taskRepositoryMock;

        public TaskService_SetPriority()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);
        }

        [Test]
        public void ShouldIncrementPriorityForEqualTask()
        {
            // Arange
            var firstTask = new TaskModel { Id = 1, Priority = 1, Name = "First task" };
            var secondTask = new TaskModel { Id = 2, Priority = 4, Name = "Second task" };
            var thirdTask = new TaskModel { Id = 3, Priority = 3, Name = "Third task" };
            _taskRepositoryMock.ResetCalls();
            _taskRepositoryMock
                .Setup(x => x.Get(It.IsAny<Func<TaskModel, bool>>()))
                .Returns(
                    (Func<TaskModel, bool> p) =>
                        new List<TaskModel> { firstTask, secondTask, thirdTask }.Where(p));
            _taskRepositoryMock
                .Setup(x => x.BulkUpdate(It.IsAny<IEnumerable<TaskModel>>()))
                .Returns((IEnumerable<TaskModel> p) => p);

            // Act
            _taskService.SetPriority(thirdTask.Id, secondTask.Priority);

            // Assert
            _taskRepositoryMock
                .Verify(c => c.Get(
                    It.Is<Func<TaskModel, bool>>(predicate => predicate != null)),
                    Times.Exactly(2));

            _taskRepositoryMock
                .Verify(c => c.BulkUpdate(
                    It.Is<IEnumerable<TaskModel>>(array => array.Count() == 2
                            && array.All(a => a == thirdTask || a == secondTask))),
                    Times.Once);

            Assert.AreEqual(secondTask.Priority, 5);
            Assert.AreEqual(thirdTask.Priority, 4);
        }

        [Test]
        public void ShouldIncrementPriorityForEqualTasks()
        {
            // Arange
            var firstTask = new TaskModel { Id = 1, Priority = 1, Name = "First task" };
            var secondTask = new TaskModel { Id = 2, Priority = 2, Name = "Second task" };
            var thirdTask = new TaskModel { Id = 3, Priority = 3, Name = "Third task" };
            var tasks = new List<TaskModel> { firstTask, secondTask, thirdTask };
            _taskRepositoryMock.ResetCalls();
            _taskRepositoryMock
                .Setup(x => x.Get(It.IsAny<Func<TaskModel, bool>>()))
                .Returns(
                    (Func<TaskModel, bool> p) =>
                        tasks.Where(p));
            _taskRepositoryMock
                .Setup(x => x.BulkUpdate(It.IsAny<IEnumerable<TaskModel>>()))
                .Returns((IEnumerable<TaskModel> p) => p);

            // Act
            _taskService.SetPriority(firstTask.Id, secondTask.Priority);

            // Assert
            _taskRepositoryMock
                .Verify(c => c.Get(
                    It.Is<Func<TaskModel, bool>>(predicate => predicate != null)),
                    Times.Exactly(2));

            _taskRepositoryMock
                .Verify(c => c.BulkUpdate(
                    It.Is<IEnumerable<TaskModel>>(array => array.Count() == tasks.Count
                            && array.All(tasks.Contains))),
                    Times.Once);

            Assert.AreEqual(firstTask.Priority, 2);
            Assert.AreEqual(secondTask.Priority, 3);
            Assert.AreEqual(thirdTask.Priority, 4);
        }

        [Test]
        public void ShouldNotSetPriopity()
        {
            // Arange
            var firstTask = new TaskModel { Id = 1, Priority = 1, Name = "First task" };
            var secondTask = new TaskModel { Id = 2, Priority = 4, Name = "Second task" };
            var thirdTask = new TaskModel { Id = 3, Priority = 3, Name = "Third task" };
            _taskRepositoryMock.ResetCalls();
            _taskRepositoryMock
                .Setup(x => x.Get(It.IsAny<Func<TaskModel, bool>>()))
                .Returns(
                    (Func<TaskModel, bool> p) =>
                        new List<TaskModel> { firstTask, secondTask, thirdTask }.Where(p));
            _taskRepositoryMock
                .Setup(x => x.BulkUpdate(It.IsAny<IEnumerable<TaskModel>>()))
                .Returns((IEnumerable<TaskModel> p) => p);

            // Act
            _taskService.SetPriority(secondTask.Id, -10);

            // Assert
            _taskRepositoryMock
                .Verify(c => c.Get(
                    It.Is<Func<TaskModel, bool>>(predicate => predicate != null)),
                    Times.Never);

            _taskRepositoryMock
                .Verify(c => c.BulkUpdate(
                    It.IsAny<IEnumerable<TaskModel>>()),
                    Times.Never);

            Assert.AreEqual(secondTask.Priority, 4);
        }
    }
}
