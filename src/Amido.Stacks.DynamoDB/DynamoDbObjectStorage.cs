using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amido.Stacks.Data.Documents;
using Amido.Stacks.DynamoDB.Abstractions;

namespace Amido.Stacks.DynamoDB
{
	public class DynamoDbObjectStorage<TEntity> : IDynamoDbObjectStorage<TEntity> where TEntity : class
	{
		private readonly DynamoDBContext context;

		public DynamoDbObjectStorage(IAmazonDynamoDB dynamoDbClient)
		{
			context = new DynamoDBContext(dynamoDbClient);
		}

		public async Task<OperationResult> DeleteAsync(string partitionKey)
		{
			await context.DeleteAsync<TEntity>(partitionKey);

			return new OperationResult(true, null);
		}

		public async Task<OperationResult<TEntity>> GetByIdAsync(string partitionKey)
		{
			var result = await context.LoadAsync<TEntity>(partitionKey);

			return new OperationResult<TEntity>(true, result, null);
		}

		public async Task<OperationResult<TEntity>> SaveAsync(TEntity document)
		{
			await context.SaveAsync(document);

			return new OperationResult<TEntity>(true, document, null);
		}
	}
}