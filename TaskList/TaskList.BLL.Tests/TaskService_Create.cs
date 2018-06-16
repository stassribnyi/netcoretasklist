using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TaskList.BLL.Dtos;
using TaskList.BLL.Interfaces;
using TaskList.BLL.Services;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.BLL.Tests
{
    [TestFixture]
    public class TaskService_Create
    {
        private readonly ITaskService _taskService;
        private readonly Mock<ITaskRepository> _taskRepositoryMock;

        public TaskService_Create()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TaskDto, TaskModel>();
            });
        }

        [Test]
        public void CreateShouldMakeAppropriativeTask()
        {
            // Arange
            var firstTask = new TaskModel { Id = 1, Priority = 1, Name = "First task" };
            var secondTask = new TaskModel { Id = 2, Priority = 4, Name = "Second task" };
            var thirdTask = new TaskModel { Id = 3, Priority = 3, Name = "Third task" };
            _taskRepositoryMock
                .Setup(x => x.Get(It.Is<Func<TaskModel, bool>>(p => p == null)))
                .Returns(
                    (Func<TaskModel, bool> p) =>
                        new List<TaskModel> { firstTask, secondTask, thirdTask });

            var descriptionTest = "Description Test";
            var task = new TaskDto
            {
                Name = "Description Test"
            };

            // Act
            _taskService.Create(task);

            // Assert
            _taskRepositoryMock
                .Verify(c => c.Create(
                    It.Is<TaskModel>(model => model.Name == descriptionTest
                                                && model.Priority == secondTask.Priority + 1)),
                    Times.Once());
        }
    }
}
