namespace ToDoApp.Infrastructure.Exceptions;

public abstract class InfrastructureException : Exception
{
    public Guid Id { get; protected init; } = Guid.Empty;

    protected InfrastructureException(string message) : base(message)
    {
    }
}
