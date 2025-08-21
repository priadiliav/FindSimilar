namespace FindSimilar.Domain.Abstraction;

public class Entity<TKey> : ITrackable where TKey : notnull
{
	public TKey Id { get; init; } = default!;
	
	public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
	public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}