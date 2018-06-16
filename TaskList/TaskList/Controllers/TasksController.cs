using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using TaskList.BLL.Interfaces;
using TaskList.Models;
using TaskList.BLL.Dtos;

namespace TaskList.Controllers
{
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        /// <summary>
        /// The task service
        /// </summary>
        private ITaskService _taskService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TasksController"/> class.
        /// </summary>
        /// <param name="taskService">The task service.</param>
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET api/tasks
        [HttpGet]
        public IEnumerable<TaskModel> Get()
        {
            return _taskService.GetAll().Select(x => Mapper.Map<TaskModel>(x)).ToList();
        }

        // POST api/tasks
        [HttpPost]
        public void Post([FromBody]TaskModel model)
        {
            _taskService.Create(Mapper.Map<TaskDto>(model));
        }

        // PUT api/tasks/2/uppriority
        [HttpPut("{id}/uppriority")]
        public void PutUpPriority(int id)
        {
            _taskService.UpPriority(id);
        }

        // PUT api/tasks/2/downpriority
        [HttpPut("{id}/downpriority")]
        public void PutDownPriority(int id)
        {
            _taskService.DownPriority(id);
        }

        // PUT api/tasks/2/setpriority
        [HttpPut("{id}/setpriority")]
        public void PutSetPriority(int id, [FromBody]int priority)
        {
            _taskService.SetPriority(id, priority);
        }

        // DELETE api/tasks/2
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _taskService.Delete(id);
        }
    }
}
