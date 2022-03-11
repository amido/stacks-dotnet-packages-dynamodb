using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Amido.Stacks.DynamoDB.Converters
{
	public class DynamoDbGuidConverter : IPropertyConverter
	{
		public DynamoDBEntry ToEntry(object value)
		{
			Guid id = Guid.Parse(value?.ToString());
			DynamoDBEntry entry = new Primitive
			{
				Value = id.ToString()
			};

			return entry;
		}

		public object FromEntry(DynamoDBEntry entry)
		{
			Primitive primitive = entry as Primitive;
			if (primitive == null || !(primitive.Value is String) || string.IsNullOrEmpty((string)primitive.Value))
				throw new ArgumentOutOfRangeException();

			Guid id = Guid.Parse(primitive.Value.ToString());
			return id;
		}
	}
}