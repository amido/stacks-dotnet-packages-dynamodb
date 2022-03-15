# stacks-dotnet-packages-dynamodb

A thin layer on top of the AWS SDK for .NET using the [Object Persistence Model](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DotNetSDKHighLevel.html).

## Requirements

You need a DynamoDB instance in order to use this library. You can follow the official instructions provided by AWS [here](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/SettingUp.DynamoWebService.html).

Also the object(s) from your application that you want to store in DynamoDB have to conform to the [Object Persistence Model](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DotNetSDKHighLevel.html). That means your object and it's fields will need to have certain attribute annotations like `[DynamoDBTable("Menu")]` etc.

## Configuration

Relevant documentation pages that you can follow to set up your profile:

- [Configuration and credential file settings](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-files.html)

- [Named profiles](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-profiles.html)

This library assumes you'll use the `AWS CLI` tools and will have configured your access keys via the `aws configure` command.

## Usage

Using the library is simple. The only thing you have to do is call the `AddDynamoDb()` service collection extension method in the project where you want to connect to DynamoDB.

```csharp
services.AddDynamoDB();
```

This will give you access to the `IDynamoDbObjectStorage` which you'll use to perform CRUD operations with in DynamoDB.

### Data Converters

The package provides a converter from `Guid` to `string` since DynamoDb doesn't understand UUID's. The `DynamoDbGuidConverter` can be found in the `Amido.Stacks.DynamoDB.Converters` namespace.

```csharp
[DynamoDBProperty(typeof(DynamoDbGuidConverter))]
public Guid Id { get; private set; }
```

To map your own types you can create your own converters. Documentation on how to do that can be found [here](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DynamoDBContext.ArbitraryDataMapping.html).
