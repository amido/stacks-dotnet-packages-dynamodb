using Amido.Stacks.Data.Documents;

namespace Amido.Stacks.DynamoDB.Abstractions;

public interface IDynamoDbObjectStorage<TEntity> where TEntity : class
{
	Task<OperationResult> DeleteAsync(string partitionKey);
	Task<OperationResult<TEntity>> GetByIdAsync(string partitionKey);
	Task<OperationResult<TEntity>> SaveAsync(string partitionKey, TEntity document);
}
