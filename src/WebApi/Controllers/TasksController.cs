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

    [HttpPut, Route("CompleteTask")]
    public async Task<IActionResult> CompleteTask([FromBody] CompleteTask command, CancellationToken cancellationToken)
    {
        await this.mediator.Send(command, cancellationToken);

        return this.Accepted();
    }

    [HttpDelete, Route("DeleteTask")]
    public async Task<IActionResult> DeleteTask([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteTask
        {
            Id = id,
        };

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

    [HttpGet, Route("GetTasksForCurrentWeek")]
    public async Task<IReadOnlyList<TaskResult>> GetTasksForCurrentWeek(CancellationToken cancellationToken)
    {
        var query = new GetTasksForCurrentWeek();

        return await this.mediator.Send(query, cancellationToken);
    }

    [HttpGet, Route("GetTodayTasks")]
    public async Task<IReadOnlyList<TaskResult>> GetTodayTasks(CancellationToken cancellationToken)
    {
        var query = new GetTodayTasks();

        return await this.mediator.Send(query, cancellationToken);
    }

    [HttpGet, Route("GetTomorrowTasks")]
    public async Task<IReadOnlyList<TaskResult>> GetTomorrowTasks(CancellationToken cancellationToken)
    {
        var query = new GetTomorrowTasks();

        return await this.mediator.Send(query, cancellationToken);
    }

    [HttpGet, Route("GetUncompletedTasks")]
    public async Task<IReadOnlyList<TaskResult>> GetUncompletedTasks(CancellationToken cancellationToken)
    {
        var query = new GetUncompletedTasks();

        return await this.mediator.Send(query, cancellationToken);
    }

    [HttpPut, Route("RescheduleTask")]
    public async Task<IActionResult> RescheduleTask([FromBody] RescheduleTask command, CancellationToken cancellationToken)
    {
        await this.mediator.Send(command, cancellationToken);

        return this.Accepted();
    }

    [HttpPut, Route("SetPercentOfComplete")]
    public async Task<IActionResult> SetPercentOfComplete([FromBody] SetPercentOfComplete command, CancellationToken cancellationToken)
    {
        await this.mediator.Send(command, cancellationToken);

        return this.Accepted();
    }

    [HttpPut, Route("UpdateTask")]
    public async Task<IActionResult> UpdateTask([FromBody] UpdateTask command, CancellationToken cancellationToken)
    {
        await this.mediator.Send(command, cancellationToken);

        return this.Accepted();
    }
}
