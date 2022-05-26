using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amido.Stacks.Data.Documents;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Amido.Stacks.DynamoDB.Tests;

public class DynamoDbObjectSearchTests
{
    ILogger<DynamoDbObjectSearch<FakeMenu>> logger;
    IDynamoDBContext context;
    IOptions<DynamoDbConfiguration> config;

    public DynamoDbObjectSearchTests()
    {
        logger = Substitute.For<ILogger<DynamoDbObjectSearch<FakeMenu>>>();
        context = Substitute.For<IDynamoDBContext>();
        config = Options.Create(new DynamoDbConfiguration
        {
            TableName = string.Empty,
            TablePrefix = string.Empty
        });
    }

    [Fact]
    public void ObjectSearch_NullLogger_ThrowsArgumentNullException()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => new DynamoDbObjectSearch<FakeMenu>(null!, context, config));
    }

    [Fact]
    public void ObjectSearch_NullContext_ThrowsArgumentNullException()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => new DynamoDbObjectSearch<FakeMenu>(logger, null!, config));
    }

    [Fact]
    public void ObjectSearch_NullConfig_ThrowsArgumentNullException()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => new DynamoDbObjectSearch<FakeMenu>(logger, context, null!));
    }

    [Fact]
    public async Task ObjectSearch_ScanAsync_Unsuccessful_When_ScanOperationConfigNull()
    {
        // Arrange
        var sut = new DynamoDbObjectSearch<FakeMenu>(logger, context, config);

        // Act
        var result = await sut.ScanAsync(null);

        // Assert
        Assert.IsType<OperationResult<List<FakeMenu>>>(result);
        Assert.False(result.IsSuccessful);
    }

    [Fact]
    public async Task ObjectSearch_QueryAsync_Unsuccessful_When_QueryOperationConfigNull()
    {
        // Arrange
        var sut = new DynamoDbObjectSearch<FakeMenu>(logger, context, config);

        // Act
        var result = await sut.QueryAsync(null);

        // Assert
        Assert.IsType<OperationResult<List<FakeMenu>>>(result);
        Assert.False(result.IsSuccessful);
    }
}