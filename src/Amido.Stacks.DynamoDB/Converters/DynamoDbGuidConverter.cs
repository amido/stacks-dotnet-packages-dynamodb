using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Amido.Stacks.DynamoDB.Converters;

public class DynamoDbGuidConverter : IPropertyConverter
{
	public DynamoDBEntry ToEntry(object value)
	{
		if (Guid.TryParse(value.ToString(), out Guid id))
		{
			DynamoDBEntry entry = new Primitive
			{
				Value = id.ToString()
			};

			return entry;
		}
		else
		{
			throw new ArgumentNullException();
		}
	}

	public object FromEntry(DynamoDBEntry entry)
	{
		Primitive primitive = entry.AsPrimitive();
		if (primitive == null || !(primitive.Value is String) || string.IsNullOrEmpty((string)primitive.Value))
			throw new ArgumentNullException();

		if (Guid.TryParse(primitive.Value.ToString(), out Guid id)) return id;
		else throw new ArgumentNullException();
	}
}
