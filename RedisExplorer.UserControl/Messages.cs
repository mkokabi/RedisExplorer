using System;

namespace RedisExplorer.UserControl
{
	using GalaSoft.MvvmLight.Messaging;

	using StackExchange.Redis;

	/// <summary>
	/// The application messages.
	/// </summary>
	public static class Messages
	{
		/// <summary>
		/// The messages types.
		/// </summary>
		public enum MessagesTypes
		{
			/// <summary>
			/// Hash entry name changed message type.
			/// </summary>
			HashEntryNameChanged,

			/// <summary>
			/// Hash entry value changed message type.
			/// </summary>
			HashEntryValueChanged,

			/// <summary>
			/// The entry added to a list key.
			/// </summary>
			EntryAdded,

			/// <summary>
			/// Sorted entry value changed message type.
			/// </summary>
			SoretedEntryValueChanged,

			/// <summary>
			/// Sorted entry score changed message type.
			/// </summary>
			SoretedEntryScoreChanged,

			/// <summary>
			/// The sorted entry added to a list key.
			/// </summary>
			SoretedEntryAdded
		}

		/// <summary>
		/// A message of hash entry changed.
		/// </summary>
		public static class HashEntryNameChanged
		{
			/// <summary>
			/// The name changed.
			/// </summary>
			/// <param name="newName">
			/// The new name.
			/// </param>
			public static void Send(string newName)
			{
				Messenger.Default.Send(newName, MessagesTypes.HashEntryNameChanged);
			}

			/// <summary>
			/// Register for hash entry name change message.
			/// </summary>
			/// <param name="recepient">
			/// The recepient.
			/// </param>
			/// <param name="action">
			/// The action.
			/// </param>
			public static void Register(object recepient, Action<string> action)
			{
				Messenger.Default.Register(recepient, MessagesTypes.HashEntryNameChanged, action);
			}
		}

		/// <summary>
		/// A message of hash entry changed.
		/// </summary>
		public static class HashEntryValueChanged
		{
			/// <summary>
			/// The value changed.
			/// </summary>
			/// <param name="newValue">
			/// The new name.
			/// </param>
			public static void Send(string newValue)
			{
				Messenger.Default.Send(newValue, MessagesTypes.HashEntryValueChanged);
			}

			/// <summary>
			/// Register for hash entry value change message.
			/// </summary>
			/// <param name="recepient">
			/// The recepient.
			/// </param>
			/// <param name="action">
			/// The action.
			/// </param>
			public static void Register(object recepient, Action<string> action)
			{
				Messenger.Default.Register(recepient, MessagesTypes.HashEntryValueChanged, action);
			}
		}

		/// <summary>
		/// A message of an entry get added to a list.
		/// </summary>
		public static class EntryAdded
		{
			/// <summary>
			/// Broadcast the entry is added.
			/// </summary>
			/// <param name="redisType">
			/// The redis type.
			/// </param>
			public static void Send(RedisType redisType)
			{
				Messenger.Default.Send(redisType, MessagesTypes.EntryAdded);
			}

			/// <summary>
			/// Register for entry added event.
			/// </summary>
			/// <param name="recepient">
			/// The recepient.
			/// </param>
			/// <param name="action">
			/// The action.
			/// </param>
			public static void Register(object recepient, Action<RedisType> action)
			{
				Messenger.Default.Register(recepient, MessagesTypes.EntryAdded, action);
			}
		}


		/// <summary>
		/// A message of sorted score changed.
		/// </summary>
		public static class SortedEntryScoreChanged
		{
			/// <summary>
			/// The name changed.
			/// </summary>
			/// <param name="newScore">
			/// The new name.
			/// </param>
			public static void Send(double newScore)
			{
				Messenger.Default.Send(newScore, MessagesTypes.SoretedEntryScoreChanged);
			}

			/// <summary>
			/// Register for sorted entry score change message.
			/// </summary>
			/// <param name="recepient">
			/// The recepient.
			/// </param>
			/// <param name="action">
			/// The action.
			/// </param>
			public static void Register(object recepient, Action<double> action)
			{
				Messenger.Default.Register(recepient, MessagesTypes.SoretedEntryScoreChanged, action);
			}
		}

		/// <summary>
		/// A message of sorted entry value changed.
		/// </summary>
		public static class SortedEntryValueChanged
		{
			/// <summary>
			/// The value changed.
			/// </summary>
			/// <param name="newValue">
			/// The new name.
			/// </param>
			public static void Send(RedisValue newValue)
			{
				Messenger.Default.Send(newValue, MessagesTypes.SoretedEntryValueChanged);
			}

			/// <summary>
			/// Register for sorted entry value change message.
			/// </summary>
			/// <param name="recepient">
			/// The recepient.
			/// </param>
			/// <param name="action">
			/// The action.
			/// </param>
			public static void Register(object recepient, Action<RedisValue> action)
			{
				Messenger.Default.Register(recepient, MessagesTypes.SoretedEntryValueChanged, action);
			}
		}

		/// <summary>
		/// A message of a sorted entry get added to a list.
		/// </summary>
		public static class SortedEntryAdded
		{
			/// <summary>
			/// Broadcast the entry is added.
			/// </summary>
			/// <param name="redisType">
			/// The redis type.
			/// </param>
			public static void Send(RedisType redisType)
			{
				Messenger.Default.Send(redisType, MessagesTypes.SoretedEntryAdded);
			}

			/// <summary>
			/// Register for entry added event.
			/// </summary>
			/// <param name="recepient">
			/// The recepient.
			/// </param>
			/// <param name="action">
			/// The action.
			/// </param>
			public static void Register(object recepient, Action<RedisType> action)
			{
				Messenger.Default.Register(recepient, MessagesTypes.SoretedEntryAdded, action);
			}
		}
	}
}
