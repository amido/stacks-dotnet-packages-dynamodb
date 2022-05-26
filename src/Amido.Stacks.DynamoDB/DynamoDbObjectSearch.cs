using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amido.Stacks.Data.Documents;
using Amido.Stacks.DynamoDB.Abstractions;
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
    public async Task<OperationResult<List<TEntity>>> ScanAsync(ScanOperationConfig scanOperationConfig)
    {
        if (scanOperationConfig is null)
            return new OperationResult<List<TEntity>>(false, new List<TEntity>(), null);

        return await ScanInternalAsync(scanOperationConfig);
    }

    private async Task<OperationResult<List<TEntity>>> ScanInternalAsync(ScanOperationConfig scanOperationConfig)
    {
        var dbResults = new List<TEntity>();
        var table = context.GetTargetTable<TEntity>();

        var search = table.Scan(scanOperationConfig);

        var resultsSet = await search.GetNextSetAsync();

        if (resultsSet.Any())
        {
            dbResults.AddRange(context.FromDocuments<TEntity>(resultsSet));
        }

        return new OperationResult<List<TEntity>>(true, dbResults, null);
    }

    public async Task<OperationResult<List<TEntity>>> QueryAsync(QueryOperationConfig queryOperationConfig)
    {
        if (queryOperationConfig is null)
            return new OperationResult<List<TEntity>>(false, new List<TEntity>(), null);

        return await QueryInternalAsync(queryOperationConfig);
    }

    private async Task<OperationResult<List<TEntity>>> QueryInternalAsync(QueryOperationConfig queryOperationConfig)
    {
        var dbResults = new List<TEntity>();
        var table = context.GetTargetTable<TEntity>();

        var search = table.Query(queryOperationConfig);

        var resultsSet = await search.GetNextSetAsync();

        if (resultsSet.Any())
        {
            dbResults.AddRange(context.FromDocuments<TEntity>(resultsSet));
        }

        return new OperationResult<List<TEntity>>(true, dbResults, null);
    }
}