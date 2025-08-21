namespace FindSimilar.Application.Abstractions;

public interface IAddressEmbeddingStore
{
	Task SaveEmbeddingAsync(long addressId, ReadOnlyMemory<float> embedding);
	Task<IEnumerable<(long id, float score)>> SearchEmbeddingAsync(ReadOnlyMemory<float> queryEmbedding, int count = 5);
}