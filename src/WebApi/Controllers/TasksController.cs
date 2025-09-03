namespace ToDoApp.WebApi.Controllers;

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

    [HttpGet, Route("GetTasks")]
    public async Task<IReadOnlyList<TaskResult>> GetTasks(CancellationToken cancellationToken)
    {
        var query = new GetTasks();

        return await this.mediator.Send(query, cancellationToken);
    }
}
