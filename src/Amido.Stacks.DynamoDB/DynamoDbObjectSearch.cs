using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using Amido.Stacks.Data.Documents;
using Amido.Stacks.DynamoDB.Abstractions;
using Amido.Stacks.DynamoDB.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Amido.Stacks.DynamoDB;

public class DynamoDbObjectSearch<TEntity> : IDynamoDbObjectSearch<TEntity> where TEntity : class
{
    private ILogger<DynamoDbObjectSearch<TEntity>> logger;
    private readonly IDynamoDBContext context;
    private readonly IOptions<DynamoDbConfiguration> config;
    private readonly DynamoDBOperationConfig opearationConfig;

    public DynamoDbObjectSearch(ILogger<DynamoDbObjectSearch<TEntity>> logger, IDynamoDBContext context, IOptions<DynamoDbConfiguration> config)
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

    // Note:
    // ScanInternalAsync and QueryInternalAsync cannot be unit tested because IDynamoDBContext.GetTargetTable returns and object that cannot be mocked.
    // See here: https://github.com/aws/aws-sdk-net/issues/1310
    public async Task<OperationResult<List<TEntity>>> ScanAsync(ScanFilter scanFilter)
    {
        if (scanFilter is null)
        {
            return new OperationResult<List<TEntity>>(
                false,
                default!,
                null);
        }

        return await ScanInternalAsync(scanFilter);
    }

    private async Task<OperationResult<List<TEntity>>> ScanInternalAsync(ScanFilter scanFilter)
    {
        try
        {
            logger.ScanAsyncRequested();

            var dbResults = new List<TEntity>();
            var table = context.GetTargetTable<TEntity>();

            var search = table.Scan(scanFilter);

            var resultsSet = await search.GetNextSetAsync();

            if (resultsSet.Any())
            {
                dbResults.AddRange(context.FromDocuments<TEntity>(resultsSet));
            }

            logger.ScanAsyncCompleted();

            return new OperationResult<List<TEntity>>(
                true,
                dbResults,
                null);
        }
        catch (AmazonServiceException ex)
        {
            logger.ScanAsyncFailed(ex.Message, ex);

            return new OperationResult<List<TEntity>>(
                false,
                default!,
                null);
        }
        catch (AmazonClientException ex)
        {
            logger.ScanAsyncFailed(ex.Message, ex);

            return new OperationResult<List<TEntity>>(
                false,
                default!,
                null);
        }
    }

    public async Task<OperationResult<List<TEntity>>> QueryAsync(QueryFilter queryFilter)
    {
        if (queryFilter is null)
        {
            return new OperationResult<List<TEntity>>(
                false,
                default!,
                null);
        }

        return await QueryInternalAsync(queryFilter);
    }

    private async Task<OperationResult<List<TEntity>>> QueryInternalAsync(QueryFilter queryFilter)
    {
        try
        {
            logger.QueryAsyncRequested();

            var dbResults = new List<TEntity>();
            var table = context.GetTargetTable<TEntity>();

            var search = table.Query(queryFilter);

            var resultsSet = await search.GetNextSetAsync();

            if (resultsSet.Any())
            {
                dbResults.AddRange(context.FromDocuments<TEntity>(resultsSet));
            }

            logger.QueryAsyncCompleted();

            return new OperationResult<List<TEntity>>(
                true,
                dbResults,
                null);
        }
        catch (AmazonServiceException ex)
        {
            logger.QueryAsyncFailed(ex.Message, ex);

            return new OperationResult<List<TEntity>>(
                false,
                default!,
                null);
        }
        catch (AmazonClientException ex)
        {
            logger.QueryAsyncFailed(ex.Message, ex);

            return new OperationResult<List<TEntity>>(
                false,
                default!,
                null);
        }
    }
}