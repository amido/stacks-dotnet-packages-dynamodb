using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amido.Stacks.Data.Documents;
using Amido.Stacks.DynamoDB.Abstractions;
using Amido.Stacks.DynamoDB.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Amido.Stacks.DynamoDB;

public class DynamoDbObjectStorage<TEntity> : IDynamoDbObjectStorage<TEntity> where TEntity : class
{
	private ILogger<DynamoDbObjectStorage<TEntity>> logger;
	private readonly IDynamoDBContext context;
	private readonly IOptions<DynamoDbConfiguration> config;
	private readonly DynamoDBOperationConfig opearationConfig;

	public DynamoDbObjectStorage(ILogger<DynamoDbObjectStorage<TEntity>> logger, IDynamoDBContext context, IOptions<DynamoDbConfiguration> config)
	{
		this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		this.context = context ?? throw new ArgumentNullException(nameof(context));
		this.config = config ?? throw new ArgumentNullException(nameof(config));

		opearationConfig = new()
		{
			OverrideTableName = config.Value.TableName,
			TableNamePrefix = config.Value.TablePrefix
		};
	}

	public async Task<OperationResult> DeleteAsync(string partitionKey)
	{
		try
		{
			logger.DeleteRequested(partitionKey);

			await context.DeleteAsync<TEntity>(partitionKey, opearationConfig);

			logger.DeleteCompleted(partitionKey);

			return new OperationResult(true, null);
		}
		catch (AmazonServiceException ex)
		{
			logger.DeleteFailed(partitionKey, ex.Message, ex);

			return new OperationResult<TEntity>(
				false,
				default(TEntity)!,
				null);
		}
		catch (AmazonClientException ex)
		{
			logger.DeleteFailed(partitionKey, ex.Message, ex);

			return new OperationResult<TEntity>(
				false,
				default(TEntity)!,
				null);
		}
	}

	public async Task<OperationResult<TEntity>> GetByIdAsync(string partitionKey)
	{
		try
		{
			logger.GetByIdRequested(partitionKey);

			var result = await context.LoadAsync<TEntity>(partitionKey, opearationConfig);

			logger.GetByIdCompleted(partitionKey);

			return new OperationResult<TEntity>(true, result, null);
		}
		catch (AmazonServiceException ex)
		{
			logger.GetByIdFailed(partitionKey, ex.Message, ex);

			return new OperationResult<TEntity>(
				false,
				default(TEntity)!,
				null);
		}
		catch (AmazonClientException ex)
		{
			logger.GetByIdFailed(partitionKey, ex.Message, ex);

			return new OperationResult<TEntity>(
				false,
				default(TEntity)!,
				null);
		}
	}

	public async Task<OperationResult<TEntity>> SaveAsync(string partitionKey, TEntity document)
	{
		try
		{
			logger.SaveRequested(partitionKey);

			await context.SaveAsync(document, opearationConfig);

			logger.SaveCompleted(partitionKey);

			return new OperationResult<TEntity>(true, document, null);
		}
		catch (AmazonServiceException ex)
		{
			logger.SaveFailed(partitionKey, ex.Message, ex);

			return new OperationResult<TEntity>(
				false,
				default(TEntity)!,
				null);
		}
		catch (AmazonClientException ex)
		{
			logger.SaveFailed(partitionKey, ex.Message, ex);

			return new OperationResult<TEntity>(
				false,
				default(TEntity)!,
				null);
		}
	}
}
