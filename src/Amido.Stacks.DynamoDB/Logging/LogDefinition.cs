using Amido.Stacks.DynamoDB.Events;
using Microsoft.Extensions.Logging;

namespace Amido.Stacks.DynamoDB.Logging
{
	/// <summary>
	/// Contains log definitions for DynamoDB component
	/// LoggerMessage.Define() creates a unique template for each log type
	/// The log template reduces the number of allocations and write logs faster to destination - https://docs.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging
	/// </summary>
	public static class LogDefinition
	{
		/// Failures with exceptions should be logged to respective failures(i.e: getByIdFailed) and then to logException in order to show them as separate entries in the logs(trace + exception
		private static readonly Action<ILogger, string, Exception> logException =
			LoggerMessage.Define<string>(
				LogLevel.Error,
				new EventId((int)EventCode.GeneralException, nameof(EventCode.GeneralException)),
				"DynamoDB Exception: {Message}"
			);

		//GETById
		private static readonly Action<ILogger, string, Exception> getByIdRequested =
			LoggerMessage.Define<string>(
				LogLevel.Information,
				new EventId((int)EventCode.GetByIdRequested, nameof(EventCode.GetByIdRequested)),
				"DynamoDB: GetById requested for document (Partition:{Partition})"
			);

		private static readonly Action<ILogger, string, Exception> getByIdCompleted =
			LoggerMessage.Define<string>(
				LogLevel.Information,
				new EventId((int)EventCode.GetByIdCompleted, nameof(EventCode.GetByIdCompleted)),
				"DynamoDB: GetById completed for document (Partition:{Partition})"
			);

		private static readonly Action<ILogger, string, string, Exception> getByIdFailed =
			LoggerMessage.Define<string, string>(
				LogLevel.Warning,
				new EventId((int)EventCode.GetByIdFailed, nameof(EventCode.GetByIdFailed)),
				"DynamoDB: GetById failed for document (Partition:{Partition}). Reason: {Reason}"
			);

		//SAVE

		private static readonly Action<ILogger, string, Exception> saveRequested =
			LoggerMessage.Define<string>(
				LogLevel.Information,
				new EventId((int)EventCode.SaveRequested, nameof(EventCode.SaveRequested)),
				"DynamoDB: Save requested for document (Partition:{Partition})"
			);

		private static readonly Action<ILogger, string, Exception> saveCompleted =
			LoggerMessage.Define<string>(
				LogLevel.Information,
				new EventId((int)EventCode.SaveCompleted, nameof(EventCode.SaveCompleted)),
				"DynamoDB: Save completed for document (Partition:{Partition})"
			);

		private static readonly Action<ILogger, string, string, Exception> saveFailed =
			LoggerMessage.Define<string, string>(
				LogLevel.Warning,
				new EventId((int)EventCode.SaveFailed, nameof(EventCode.SaveFailed)),
				"DynamoDB: Save failed for document (Partition:{Partition}). Reason: {Reason}"
			);

		//DELETE

		private static readonly Action<ILogger, string, Exception> deleteRequested =
			LoggerMessage.Define<string>(
				LogLevel.Information,
				new EventId((int)EventCode.DeleteRequested, nameof(EventCode.DeleteRequested)),
				"DynamoDB: Delete requested for document (Partition:{Partition})"
			);

		private static readonly Action<ILogger, string, Exception> deleteCompleted =
			LoggerMessage.Define<string>(
				LogLevel.Information,
				new EventId((int)EventCode.DeleteCompleted, nameof(EventCode.DeleteCompleted)),
				"DynamoDB: Delete completed for document (Partition:{Partition}"
			);

		private static readonly Action<ILogger, string, string, Exception> deleteFailed =
			LoggerMessage.Define<string, string>(
				LogLevel.Warning,
				new EventId((int)EventCode.DeleteFailed, nameof(EventCode.DeleteFailed)),
				"DynamoDB: Delete failed for document (Partition:{Partition}). Reason: {Reason}"
			);

		//Exception

		/// <summary>
		/// When an exception is present in the failure, it will be logged as exception message instead of trace.
		/// Logging messages with an exception will make them an exception and the trace will lose an entry, making harder to debug issues
		/// </summary>
		private static void LogException(ILogger logger, Exception ex)
		{
			if (ex != null)
				logException(logger, ex.Message, ex);
		}

		// GETById

		public static void GetByIdRequested(this ILogger logger, string partitionKey)
		{
			getByIdRequested(logger, partitionKey, null!);
		}

		public static void GetByIdCompleted(this ILogger logger, string partitionKey)
		{
			getByIdCompleted(logger, partitionKey, null!);
		}

		public static void GetByIdFailed(this ILogger logger, string partitionKey, string reason, Exception ex)
		{
			getByIdFailed(logger, partitionKey, reason, null!);
			LogException(logger, ex);
		}

		// Save

		public static void SaveRequested(this ILogger logger, string partitionKey)
		{
			saveRequested(logger, partitionKey, null!);
		}

		public static void SaveCompleted(this ILogger logger, string partitionKey)
		{
			saveCompleted(logger, partitionKey, null!);
		}

		public static void SaveFailed(this ILogger logger, string partitionKey, string reason, Exception ex)
		{
			saveFailed(logger, partitionKey, reason, null!);
			LogException(logger, ex);
		}

		// Delete

		public static void DeleteRequested(this ILogger logger, string partitionKey)
		{
			deleteRequested(logger, partitionKey, null!);
		}

		public static void DeleteCompleted(this ILogger logger, string partitionKey)
		{
			deleteCompleted(logger, partitionKey, null!);
		}

		public static void DeleteFailed(this ILogger logger, string partitionKey, string reason, Exception ex)
		{
			deleteFailed(logger, partitionKey, reason, null!);
			LogException(logger, ex);
		}
	}
}
