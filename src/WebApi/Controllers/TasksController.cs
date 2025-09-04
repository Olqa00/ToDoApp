namespace ToDoApp.WebApi.Controllers;

using ToDoApp.Application.Commands;
using ToDoApp.Application.Queries;
using ToDoApp.Application.Results;

[ApiController, Route("[controller]")]
public sealed class TasksController : ControllerBase
{
    private readonly ISender mediator;

    public TasksController(ISender mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost, Route("AddTask")]
    public async Task<IActionResult> AddTask([FromBody] AddTask command, CancellationToken cancellationToken)
    {
        await this.mediator.Send(command, cancellationToken);

        return this.Accepted();
    }

    [HttpGet, Route("GetTaskById")]
    public async Task<TaskResult?> GetTaskById([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTaskById
        {
            Id = id,
        };

        return await this.mediator.Send(query, cancellationToken);
    }

    [HttpGet, Route("GetTasks")]
    public async Task<IReadOnlyList<TaskResult>> GetTasks(CancellationToken cancellationToken)
    {
        var query = new GetTasks();

        return await this.mediator.Send(query, cancellationToken);
    }

    [HttpGet, Route("GetTodayTasks")]
    public async Task<IReadOnlyList<TaskResult>> GetTodayTasks(CancellationToken cancellationToken)
    {
        var query = new GetTodayTasks();

        return await this.mediator.Send(query, cancellationToken);
    }
}
