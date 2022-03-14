using Amazon.DynamoDBv2;
using Amido.Stacks.Data.Documents.Abstractions;
using Amido.Stacks.DynamoDB.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Amido.Stacks.DynamoDB.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddDynamoDB(this IServiceCollection services)
		{
			services.AddAWSService<IAmazonDynamoDB>();
			services.AddSingleton(typeof(IDynamoDbObjectStorage<>), typeof(DynamoDbObjectStorage<>));
			return services;
		}
	}
}