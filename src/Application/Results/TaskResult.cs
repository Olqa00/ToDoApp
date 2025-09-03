namespace ToDoApp.Application.Results;

public sealed record class TaskResult
{
    [JsonPropertyName("completed_At")]
    public DateTime? CompletedAt { get; init; }

    [JsonPropertyName("created_At")]
    public required DateTime CreatedAt { get; init; }

    [JsonPropertyName("description")]
    public required string Description { get; init; }

    [JsonPropertyName("expiry_date_time")]
    public required DateTime ExpiryDateTime { get; init; }

    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("is_completed")]
    public required bool IsCompleted { get; init; }

    [JsonPropertyName("percent_complete")]
    public required int PercentComplete { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; init; }
}
