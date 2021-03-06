﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace RedisExplorer.UserControl.ViewModel
{
	using GalaSoft.MvvmLight;
	using GalaSoft.MvvmLight.Command;

	using RedisExplorer.Common.DataTypes;

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
				this.values = new ObservableCollection<string>(redisData.Values.ToArray().ToStringArray());
			}
			if (redisData.Hash != null)
			{
				this.hash = new ObservableCollection<HashEntryViewModel>();
				redisData.Hash.Select(hashEntry => new HashEntryViewModel
					{
						Name = hashEntry.Name, Value = hashEntry.Value
					}
				).ToList().ForEach(this.hash.Add);
			}
			if (redisData.SortedSet != null)
			{
				this.sortedSet = new ObservableCollection<SortedSetEntryViewModel>();
				redisData.SortedSet.Select(sortedSetEntry => new SortedSetEntryViewModel
				{
					Element = sortedSetEntry.Element,
					Score = sortedSetEntry.Score
				}
				).ToList().ForEach(this.sortedSet.Add);
			}
			if (this.Type == RedisType.Hash)
			{
				Messages.HashEntryNameChanged.Register(
					this, 
					newName =>
					{
						if (!this.redisData.Loaded)
						{
							return;
						}
						if (this.redisData.Hash.Count > this.selectedItemIndex)
						{
							string oldValue = this.redisData.Hash[this.selectedItemIndex].Value;
							this.redisData.Hash[this.selectedItemIndex] = new HashEntry(newName, oldValue);
						}
					});
				Messages.HashEntryValueChanged.Register(
					this, 
					newValue =>
						{
							if (!this.redisData.Loaded)
							{
								return;
							}
							if (this.redisData.Hash.Count > this.selectedItemIndex)
							{
								string oldName = this.redisData.Hash[this.selectedItemIndex].Name;
								this.redisData.Hash[this.selectedItemIndex] = new HashEntry(oldName, newValue);
							}
						}
				);
			}
			if (this.Type == RedisType.SortedSet)
			{
				Messages.SortedEntryScoreChanged.Register(
					this,
					newScore =>
					{
						if (!this.redisData.Loaded)
						{
							return;
						}
						if (this.redisData.SortedSet.Count > this.selectedItemIndex)
						{
							RedisValue oldValue = this.redisData.SortedSet[this.selectedItemIndex].Element;
							this.redisData.SortedSet[this.selectedItemIndex] = new SortedSetEntry(oldValue, newScore);
						}
					});
				Messages.SortedEntryValueChanged.Register(
					this,
					newValue =>
					{
						if (!this.redisData.Loaded)
						{
							return;
						}
						if (this.redisData.SortedSet.Count > this.selectedItemIndex)
						{
							double oldScore = this.redisData.SortedSet[this.selectedItemIndex].Score;
							this.redisData.SortedSet[this.selectedItemIndex] = new SortedSetEntry(newValue, oldScore);
						}
					}
				);
			}
		}

		#region Private fields
		readonly RedisData redisData;

		readonly ObservableCollection<string> values;

		readonly ObservableCollection<HashEntryViewModel> hash;

		readonly ObservableCollection<SortedSetEntryViewModel> sortedSet;

		ICommand rowChangedCommand;

		string selectedItem;

		int selectedItemIndex;

		bool ignoreUpdatingValue = true;

		HashEntryViewModel selectedHashEntry;

		SortedSetEntryViewModel selectedSortedSetEntry;

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
		/// The selected hash entry when the redis data is a hash.
		/// </summary>
		public HashEntryViewModel SelectedHashEntry
		{
			get
			{
				return this.selectedHashEntry;
			}
			set
			{
				Set(() => SelectedHashEntry, ref selectedHashEntry, value);
				if (!ignoreUpdatingValue)
				{
					this.Hash[this.selectedItemIndex] = value;
				}
				this.redisData.Hash[this.selectedItemIndex] = value;
			}
		}

		/// <summary>
		/// The selected sorted set entry when the redis data is a hash.
		/// </summary>
		public SortedSetEntryViewModel SelectedSortedSetEntry
		{
			get
			{
				return this.selectedSortedSetEntry;
			}
			set
			{
				Set(() => SelectedSortedSetEntry, ref selectedSortedSetEntry, value);
				if (!ignoreUpdatingValue)
				{
					this.SortedSet[this.selectedItemIndex] = value;
				}
				this.redisData.SortedSet[this.selectedItemIndex] = value;
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
			set
			{
				if (redisData.Key != value)
				{
					redisData.Key = value;
					this.Broadcast(string.Empty, value, "Key");
				}
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
		public ObservableCollection<HashEntryViewModel> Hash
		{
			get
			{
				return hash;
			}
		}

		/// <summary>
		/// The sorted set values.
		/// </summary>
		public ObservableCollection<SortedSetEntryViewModel> SortedSet
		{
			get
			{
				return this.sortedSet;
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
				if (!this.RedisData.Loaded)
				{
					return "<Not loaded>";
				}
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
				if (this.Type == RedisType.Hash)
				{
					this.SelectedHashEntry = Hash[selectedItemIndex];
				}
				else if (this.Type == RedisType.SortedSet)
				{
					this.SelectedSortedSetEntry = SortedSet[selectedItemIndex];
				}
				else
				{
					this.SelectedItem = Values[selectedItemIndex];
				}
			}
			finally
			{
				ignoreUpdatingValue = false;
			}
		}
	}
}
