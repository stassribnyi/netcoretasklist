using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using TaskList.BLL.Interfaces;
using TaskList.Models;

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

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
