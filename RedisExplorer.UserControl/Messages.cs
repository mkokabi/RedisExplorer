﻿using System;

namespace RedisExplorer.UserControl
{
	using GalaSoft.MvvmLight.Messaging;

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
			HashEntryValueChanged
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
	}
}
