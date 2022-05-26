using Amazon.DynamoDBv2.DocumentModel;
using Amido.Stacks.Data.Documents;

namespace Amido.Stacks.DynamoDB.Abstractions;

public interface IDynamoDbObjectSearch<TEntity> where TEntity : class
{
	Task<OperationResult<List<TEntity>>> ScanAsync(ScanFilter scanFilter);
	Task<OperationResult<List<TEntity>>> QueryAsync(QueryFilter queryFilter);
}