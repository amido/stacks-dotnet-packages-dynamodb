namespace Amido.Stacks.DynamoDB.Events;

public enum EventCode
{
	GeneralException = 123456899,

	GetByIdRequested = 123456801,
	GetByIdCompleted = 123456802,
	GetByIdFailed = 123456803,

	SaveRequested = 123456811,
	SaveCompleted = 123456812,
	SaveFailed = 123456813,

	DeleteRequested = 123456821,
	DeleteCompleted = 123456822,
	DeleteFailed = 123456823,

	ScanAsyncRequested = 123456831,
	ScanAsyncCompleted = 123456832,
	ScanAsyncFailed = 123456833,

	QueryAsyncRequested = 123456841,
	QueryAsyncCompleted = 123456842,
	QueryAsyncFailed = 123456843,
}