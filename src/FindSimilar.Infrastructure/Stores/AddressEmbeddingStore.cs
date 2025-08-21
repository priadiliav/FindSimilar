using FindSimilar.Application.Abstractions;
using Milvus.Client;

namespace FindSimilar.Infrastructure.Stores;

public class AddressEmbeddingStore(MilvusClient milvusClient) : IAddressEmbeddingStore
{
	private const string DatabaseName = "find_similar_db";
	private const string CollectionName = "address_vector_collection";

	private bool _initialized;

	private async Task EnsureInitializedAsync()
	{
		if (_initialized) 
			return;

		var databases = await milvusClient.ListDatabasesAsync();
		if (!databases.Contains(DatabaseName))
			await milvusClient.CreateDatabaseAsync(DatabaseName);

		await CreateCollectionIfNotExistsAsync();
		await milvusClient.GetCollection(CollectionName).LoadAsync();

		_initialized = true;
	}

	private async Task CreateCollectionIfNotExistsAsync()
	{
		if (!await milvusClient.HasCollectionAsync(CollectionName))
		{
			await milvusClient.CreateCollectionAsync(CollectionName, new CollectionSchema
			{
					Fields =
					{
							FieldSchema.Create<long>("address_id", isPrimaryKey: true),
							FieldSchema.CreateFloatVector("address_info_vector",
									dimension: 1536,
									description: "Vector representation of address information")
					},
					Description = "Collection for storing address embeddings",
					EnableDynamicFields = true
			});

			await milvusClient.GetCollection(CollectionName)
					.CreateIndexAsync(
							fieldName: "address_info_vector", 
							indexType: IndexType.IvfFlat, 
							metricType: SimilarityMetricType.L2, 
							extraParams: new Dictionary<string, string> { ["nlist"] = "1024" });
		}
	}

	public async Task SaveEmbeddingAsync(long addressId, ReadOnlyMemory<float> embedding)
	{
		await EnsureInitializedAsync();

		await milvusClient.GetCollection(CollectionName).InsertAsync(new FieldData[]
		{
			FieldData.Create("address_id", [addressId]), 
			FieldData.CreateFloatVector("address_info_vector", new List<ReadOnlyMemory<float>> { embedding })
		});
	}

	public async Task<IEnumerable<(long id, float score)>> SearchEmbeddingAsync(ReadOnlyMemory<float> queryEmbedding, int count = 5)
	{
		await EnsureInitializedAsync();
		
		var results = await milvusClient.GetCollection(CollectionName).SearchAsync(
				vectorFieldName: "address_info_vector",
				vectors: [queryEmbedding],
				metricType: SimilarityMetricType.L2,
				limit: ++count, // we add 1 to exclude the query itself
				parameters: new SearchParameters
				{
					OutputFields = { "address_id" },
					ConsistencyLevel = ConsistencyLevel.Strong,
					ExtraParameters = { ["nprobe"] = "1024" }
				});

		if (results.Ids.LongIds is null)
			return [];
		
		var pairs = results.Ids.LongIds
				.Zip(results.Scores, (id, score) => (id, score));

		return pairs;
	}
}