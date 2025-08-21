namespace FindSimilar.Domain.Abstraction;

public interface ITrackable
{
	public DateTimeOffset CreatedAt { get; set; }
	public DateTimeOffset UpdatedAt { get; set; }
}