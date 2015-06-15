using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace RedisExplorer.UserControl.ViewModel
{
	using GalaSoft.MvvmLight;
	using GalaSoft.MvvmLight.Command;

	using RedisExplorer.Common.DataTypes;
	using RedisExplorer.DataTypes;

	using StackExchange.Redis;

	/// <summary>
	/// The view model of redis data.
	/// </summary>
	public class DataViewModel : ViewModelBase
	{
		/// <summary>
		/// Initialises a new instance of the <see cref="DataViewModel"/> class.
		/// </summary>
		/// <param name="redisData">
		/// The redis data.
		/// </param>
		public DataViewModel(RedisData redisData)
		{
			if (redisData == null)
			{
				throw new ArgumentNullException("redisData");
			}
			this.redisData = redisData;
			this.Value = redisData.Value;
			if (redisData.Values != null)
			{
				this.values = new ObservableCollection<string>(redisData.Values.ToStringArray());
			}
		}

		#region Private fields
		readonly RedisData redisData;

		readonly ObservableCollection<string> values;

		ICommand rowChangedCommand;

		string selectedItem;

		int selectedItemIndex;

		bool ignoreUpdatingValue = true;
		#endregion

		#region Commands

		/// <summary>
		/// The row changed command.
		/// </summary>
		public ICommand RowChangedCommand
		{
			get
			{
				return rowChangedCommand ?? (rowChangedCommand = new RelayCommand<SelectionChangedEventArgs>(RowChanged));
			}
		}
		#endregion

		#region Public properties

		/// <summary>
		/// Accessor to internal redis data.
		/// </summary>
		public RedisData RedisData
		{
			get
			{
				return this.redisData;
			}
		}

		/// <summary>
		/// The selected item when the redis data is not a single value.
		/// </summary>
		public string SelectedItem
		{
			get
			{
				return selectedItem;
			}
			set
			{
				Set(() => SelectedItem, ref selectedItem, value);
				if (!ignoreUpdatingValue)
				{
					this.Values[this.selectedItemIndex] = value;
					this.redisData.Values[this.selectedItemIndex] = value;
				}
			}
		}

		/// <summary>
		/// The redis data key.
		/// </summary>
		public string Key
		{
			get
			{
				return redisData.Key;
			}
			// ReSharper disable once ValueParameterNotUsed
			set
			{
			}
		}

		/// <summary>
		/// The single value.
		/// </summary>
		public RedisValue Value
		{
			get
			{
				return redisData.Value;
			}
			set
			{
				if (redisData.Value != value)
				{
					redisData.Value = value;
					Broadcast<string>(string.Empty, value, "Value");
				}
			}
		}

		/// <summary>
		/// The list values.
		/// </summary>
		public ObservableCollection<string> Values
		{
			get
			{
				return values;
			}
		}

		/// <summary>
		/// The hash values.
		/// </summary>
		// TODO: Not completed
		public HashEntry[] Hash
		{
			get
			{
				return redisData.Hash;
			}
		}

		/// <summary>
		/// The sorted set values.
		/// </summary>
		// TODO: Not completed
		public IEnumerable<SortedSetEntry> SortedSet
		{
			get
			{
				return redisData.SortedSet;
			}
		}

		/// <summary>
		/// The redis data type
		/// </summary>
		public RedisType Type
		{
			get
			{
				return redisData.Type;
			}
		}
		#endregion

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			return Text;
		}

		/// <summary>
		/// Type based string presentation of the data
		/// </summary>
		public string Text
		{
			get
			{
				switch (Type)
				{
					case RedisType.String:
						{
							return this.Value;
						}
					case RedisType.List:
					case RedisType.Set:
						{
							return String.Join(",", this.Values);
						}
					case RedisType.Hash:
						{
							return String.Join(",", this.Hash.Select(item => string.Format("{0}:{1}", item.Name, item.Value)).ToArray());
						}
					case RedisType.SortedSet:
						{
							return String.Join(",", this.SortedSet.Select(item => string.Format("{0}:{1}", item.Element, item.Score)).ToArray());
						}
				}
				return base.ToString();
			}
		}

		/// <summary>
		/// For data type other than single value the row changed event handler of datagrid.
		/// </summary>
		/// <param name="selectionChangedEventArgs">
		/// The selection changed event args.
		/// </param>
		public void RowChanged(SelectionChangedEventArgs selectionChangedEventArgs)
		{
			if (selectionChangedEventArgs == null)
			{
				throw new ArgumentNullException("selectionChangedEventArgs");
			}
			if (selectionChangedEventArgs.AddedItems.Count == 0)
			{
				return;
			}
			this.selectedItemIndex = (selectionChangedEventArgs.Source as DataGrid).SelectedIndex;
			// TODO: Find a better way of avoiding falling in a loop while updating the Selected Item
			ignoreUpdatingValue = true;
			try
			{
				this.SelectedItem = Values[selectedItemIndex];
			}
			finally
			{
				ignoreUpdatingValue = false;
			}
		}
	}
}
