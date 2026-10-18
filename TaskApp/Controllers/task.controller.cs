using Microsoft.AspNetCore.Mvc;
using TaskModels;
using TaskService;


[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _service;
    public TaskController(ITaskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IEnumerable<TaskModel>> getAll()
    {
        return await _service.getAll();
    }

    [HttpGet("{id}")]
    public async Task<TaskModel> getBy(int id)
    {
        return await _service.getBy(id);
    }

    [HttpPost]
    public async Task<TaskModel> create(TaskCreate dto)
    {
        return await _service.create(dto);
    }

    [HttpPut("{dto}")]
    public async Task<ActionResult> update(TaskUpdate dto)
    {
        var updated=await _service.update(dto);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> delete(int id)
    {
        await _service.delete(id);
        return Ok("Deleted successfully");
    }

}