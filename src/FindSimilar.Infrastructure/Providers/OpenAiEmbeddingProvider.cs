using FindSimilar.Application.Abstractions;
using OpenAI;

namespace FindSimilar.Infrastructure.Providers;

public class OpenAiEmbeddingProvider(OpenAIClient openAiClient) : IEmbeddingProvider
{
	public async Task<ReadOnlyMemory<float>> GenerateEmbeddingAsync(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
			throw new ArgumentException("Text cannot be null or empty.", nameof(text));

		var embeddingClient = openAiClient.GetEmbeddingClient("text-embedding-3-small");
		var response = await embeddingClient.GenerateEmbeddingAsync(text);

		return response.Value.ToFloats();
	}
}