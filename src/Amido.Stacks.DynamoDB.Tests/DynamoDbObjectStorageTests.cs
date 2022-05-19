using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Amido.Stacks.DynamoDB.Tests;

public class DynamoDbObjectStorageTests
{
	ILogger<DynamoDbObjectStorage<FakeMenu>> logger;
	IDynamoDBContext context;
	IOptions<DynamoDbConfiguration> config;

	public DynamoDbObjectStorageTests()
	{
		logger = Substitute.For<ILogger<DynamoDbObjectStorage<FakeMenu>>>();
		context = Substitute.For<IDynamoDBContext>();
		config = Options.Create<DynamoDbConfiguration>(new DynamoDbConfiguration()
		{
			TableName = string.Empty,
			TablePrefix = string.Empty
		});
	}

	[Fact]
	public void ObjectStorage_NullLogger_ThrowsArgumentNullException()
	{
		// Arrange
		// Act
		// Assert
		Assert.Throws<ArgumentNullException>(() => new DynamoDbObjectStorage<FakeMenu>(null!, context, config));
	}

	[Fact]
	public void ObjectStorage_NullContext_ThrowsArgumentNullException()
	{
		// Arrange
		// Act
		// Assert
		Assert.Throws<ArgumentNullException>(() => new DynamoDbObjectStorage<FakeMenu>(logger, null!, config));
	}

	[Fact]
	public void ObjectStorage_NullConfig_ThrowsArgumentNullException()
	{
		// Arrange
		// Act
		// Assert
		Assert.Throws<ArgumentNullException>(() => new DynamoDbObjectStorage<FakeMenu>(logger, context, null!));
	}

	[Theory, AutoData]
	public async Task ObjectStorage_GetByIdAsync_Successful(Guid id)
	{
		// Arrange
		context.LoadAsync<FakeMenu>(Arg.Any<string>(), Arg.Any<DynamoDBOperationConfig>())
			.Returns(Task.FromResult(new FakeMenu(id)));

		var sut = new DynamoDbObjectStorage<FakeMenu>(logger, context, config);

		// Act
		var result = await sut.GetByIdAsync(string.Empty);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccessful);
		Assert.Equal(id, result.Content.Id);
	}

	[Fact]
	public async Task ObjectStorage_GetByIdAsync_ThrowsServiceException_NotSuccessul()
	{
		// Arrange
		context.LoadAsync<FakeMenu>(Arg.Any<string>(), Arg.Any<DynamoDBOperationConfig>())
			.Throws(new AmazonServiceException());

		var sut = new DynamoDbObjectStorage<FakeMenu>(logger, context, config);

		// Act
		var result = await sut.GetByIdAsync(string.Empty);

		// Assert
		Assert.NotNull(result);
		Assert.False(result.IsSuccessful);
	}

	[Fact]
	public async Task ObjectStorage_GetByIdAsync_ThrowsClientException_NotSuccessul()
	{
		// Arrange
		context.LoadAsync<FakeMenu>(Arg.Any<string>(), Arg.Any<DynamoDBOperationConfig>())
			.Throws(new AmazonClientException(string.Empty));

		var sut = new DynamoDbObjectStorage<FakeMenu>(logger, context, config);

		// Act
		var result = await sut.GetByIdAsync(string.Empty);

		// Assert
		Assert.NotNull(result);
		Assert.False(result.IsSuccessful);
	}

	[Fact]
	public async Task ObjectStorage_DeleteAsync_Successul()
	{
		// Arrange
		context.DeleteAsync<FakeMenu>(Arg.Any<string>(), Arg.Any<DynamoDBOperationConfig>())
			.Returns(Task.CompletedTask);

		var sut = new DynamoDbObjectStorage<FakeMenu>(logger, context, config);

		// Act
		var result = await sut.DeleteAsync(string.Empty);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public async Task ObjectStorage_DeleteAsync_ThrowsServiceException_NotSuccessul()
	{
		// Arrange
		context.DeleteAsync<FakeMenu>(Arg.Any<string>(), Arg.Any<DynamoDBOperationConfig>())
			.Throws(new AmazonServiceException());

		var sut = new DynamoDbObjectStorage<FakeMenu>(logger, context, config);

		// Act
		var result = await sut.DeleteAsync(string.Empty);

		// Assert
		Assert.NotNull(result);
		Assert.False(result.IsSuccessful);
	}

	[Fact]
	public async Task ObjectStorage_DeleteAsync_ThrowsClientException_NotSuccessul()
	{
		// Arrange
		context.DeleteAsync<FakeMenu>(Arg.Any<string>(), Arg.Any<DynamoDBOperationConfig>())
			.Throws(new AmazonClientException(string.Empty));

		var sut = new DynamoDbObjectStorage<FakeMenu>(logger, context, config);

		// Act
		var result = await sut.DeleteAsync(string.Empty);

		// Assert
		Assert.NotNull(result);
		Assert.False(result.IsSuccessful);
	}

	[Theory, AutoData]
	public async Task ObjectStorage_SaveAsync_Successul(FakeMenu menu)
	{
		// Arrange
		context.SaveAsync<FakeMenu>(Arg.Any<FakeMenu>(), Arg.Any<DynamoDBOperationConfig>())
			.Returns(Task.CompletedTask);

		var sut = new DynamoDbObjectStorage<FakeMenu>(logger, context, config);

		// Act
		var result = await sut.SaveAsync(string.Empty, menu);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccessful);
		Assert.Equal(menu.Id, result.Content.Id);
	}

	[Fact]
	public async Task ObjectStorage_SaveAsync_ThrowsServiceException_NotSuccessul()
	{
		// Arrange
		context.SaveAsync<FakeMenu>(Arg.Any<FakeMenu>(), Arg.Any<DynamoDBOperationConfig>())
			.Throws(new AmazonServiceException());

		var sut = new DynamoDbObjectStorage<FakeMenu>(logger, context, config);

		// Act
		var result = await sut.SaveAsync(string.Empty, new FakeMenu());

		// Assert
		Assert.NotNull(result);
		Assert.False(result.IsSuccessful);
	}

	[Fact]
	public async Task ObjectStorage_SaveAsync_ThrowsClientException_NotSuccessul()
	{
		// Arrange
		context.SaveAsync<FakeMenu>(Arg.Any<FakeMenu>(), Arg.Any<DynamoDBOperationConfig>())
			.Throws(new AmazonClientException(string.Empty));

		var sut = new DynamoDbObjectStorage<FakeMenu>(logger, context, config);

		// Act
		var result = await sut.SaveAsync(string.Empty, new FakeMenu());

		// Assert
		Assert.NotNull(result);
		Assert.False(result.IsSuccessful);
	}
}
