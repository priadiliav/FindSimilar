namespace FindSimilar.Application.Abstractions;

public interface IEmbeddingProvider
{
	public Task<ReadOnlyMemory<float>> GenerateEmbeddingAsync(string text);
}