﻿namespace ToDoApp.Application.Exceptions;

public abstract class ApplicationException : Exception
{
    public Guid Id { get; protected init; } = Guid.Empty;

    protected ApplicationException(string message) : base(message)
    {
    }
}
