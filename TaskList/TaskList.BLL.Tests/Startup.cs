using AutoMapper;
using NUnit.Framework;
using TaskList.BLL.Dtos;
using TaskList.DAL.Models;

namespace TaskList.BLL.Tests
{
    [SetUpFixture]
    public class Startup
    {
        [OneTimeSetUp]
        public void Init()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TaskDto, TaskModel>();
            });
        }
    }
}
