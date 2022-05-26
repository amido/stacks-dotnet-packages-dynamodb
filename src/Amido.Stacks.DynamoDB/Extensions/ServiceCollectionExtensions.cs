using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amido.Stacks.Data.Documents.Abstractions;
using Amido.Stacks.DynamoDB.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Amido.Stacks.DynamoDB.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddDynamoDB(this IServiceCollection services)
	{
		services.AddAWSService<IAmazonDynamoDB>();
		services.AddTransient<IDynamoDBContext, DynamoDBContext>();
		services.AddTransient(typeof(IDynamoDbObjectStorage<>), typeof(DynamoDbObjectStorage<>));
		services.AddTransient(typeof(IDynamoDbObjectSearch<>), typeof(DynamoDbObjectSearch<>));
		return services;
	}
}