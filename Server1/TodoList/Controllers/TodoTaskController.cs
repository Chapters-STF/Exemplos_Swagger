using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;
using TodoList.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoTaskController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        public TodoTaskController(IMemoryCache cache)
        {
            _cache = cache;
            if (_cache.Get<List<TodoTask>>("TodoTask") == null)
                _cache.Set(
                    "TodoTask",
                    new List<TodoTask>() {
                        new TodoTask { TodoTaskId = 1, Desc = "Escovar os dentes", Status = "Done" } ,
                        new TodoTask { TodoTaskId = 2, Desc = "Fazer café da manhã", Status = "In Progress" },
                        new TodoTask { TodoTaskId = 3, Desc = "Limpar a louça", Status = "To do" }
                    }
                );
        }

        // GET: api/<TodoTaskController>
        [HttpGet]
        public IEnumerable<TodoTask> Get()
        {
            return _cache.Get<List<TodoTask>>("TodoTask");
        }

        // GET api/<TodoTaskController>/5
        [HttpGet("{id}")]
        public TodoTask? Get(int id)
        {
            var item = _cache.Get<List<TodoTask>>("TodoTask").FirstOrDefault(x => x.TodoTaskId == id);
            if (item != null)
            {
                return item;
            }
            return null;
        }

        [SwaggerOperation(
            Summary = "Creates a task",
            Description = "Add to a task to existing Todo list",
            OperationId = "CreateTask",
            Tags = new[] { "TodoTask", "WeatherForecast" }
        )]
        [HttpPost]
        public void Post([FromBody, SwaggerRequestBody("The Task payload shoul be like this", Required = true)] TodoTask value)
        {
            var list = _cache.Get<List<TodoTask>>("TodoTask");
            list.Add(value);
            _cache.Set("TodoTask", list);

        }


        /// <summary>
        /// Update Todo Item
        /// </summary>
        /// <param name="id">Id to be deleted</param>
        /// <param name="value">Body with Todo Task</param>
        /// <returns>A newly created Todo Item</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /TodoTask/{id}
        ///     {
        ///         "todoTaskId": 1,
        ///         "desc": "Comer",
        ///         "status": "Done"
        ///     }
        ///
        /// </remarks>
        [SwaggerResponse(201, "The product was created", typeof(TodoTask))]
        [SwaggerResponse(400, "The product data is invalid",typeof(ErrorWrapper))]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] TodoTask value)
        {
            var list = _cache.Get<List<TodoTask>>("TodoTask");
            if (list.FirstOrDefault(x => x.TodoTaskId == id) != null)
            {
                list.First(x => x.TodoTaskId == id).Desc = value.Desc;
                list.First(x => x.TodoTaskId == id).Status = value.Status;
                _cache.Set("TodoTask", list);
            }
        }

        /// <summary>
        /// Deletes a specific Todo Item.
        /// </summary>
        /// <param name="id">Id to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var list = _cache.Get<List<TodoTask>>("TodoTask");
            if (list.FirstOrDefault(x => x.TodoTaskId == id) != null)
            {
                list.Remove(list.First(x => x.TodoTaskId == id));
                _cache.Set("TodoTask", list);
            }
        }
    }
}
