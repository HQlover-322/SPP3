using back.Models;
using back.Services;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers
{
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly ILogger<ToDoController> _logger;
        private readonly ToDoService _toDoService;
        public ToDoController(ILogger<ToDoController> logger, ToDoService toDo)
        {
            _logger = logger;
            _toDoService = toDo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //await _toDoService.GetNewTasks()
            return Ok(await _toDoService.GetNewTasks()); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _toDoService.GetNewTask(id));
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]ToDoItemViewModel model)
        {
            var item = await _toDoService.AddNewTask(model);
            return CreatedAtAction(nameof(Post),item);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id)
        {
            await _toDoService.UpdateStatus(id);
            return Ok(await _toDoService.GetNewTask(id));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _toDoService.Delete(id);
            return NoContent();
        }
    }
}
