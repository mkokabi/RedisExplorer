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

	using RedisExplorer.DataTypes;

	using StackExchange.Redis;

	public class DataViewModel : ViewModelBase
	{
		public DataViewModel(RedisData redisData)
		{
			this.redisData = redisData;
			this.Value = redisData.Value;
			if (redisData.Values != null)
			{
				this.values = new ObservableCollection<string>(redisData.Values.ToStringArray());
			}
		}

		#region Private fields
		ICommand rowChangedCommand;

		RedisData redisData;

		string selectedItem;

		int selectedItemIndex;

		readonly ObservableCollection<string> values;

		ICommand valueChangedCommand;

		bool ignoreUpdatingValue = true;
		#endregion

		#region Commands
		public ICommand RowChangedCommand
		{
			get
			{
				return rowChangedCommand ?? (rowChangedCommand = new RelayCommand<SelectionChangedEventArgs>(RowChanged));
			}
		}
		#endregion

		#region Public properties

		public RedisData RedisData
		{
			get
			{
				return this.redisData;
			}
		}

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

		public string Key
		{
			get
			{
				return redisData.Key;
			}
			set { }
		}

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
					Broadcast<string>("", value, "Value");
				}
			}
		}

		public ObservableCollection<string> Values
		{
			get
			{
				return values;
			}
			set { }
		}

		public HashEntry[] Hash
		{
			get
			{
				return redisData.Hash;
			}
			set { }
		}

		public IEnumerable<SortedSetEntry> SortedSet
		{
			get
			{
				return redisData.SortedSet;
			}
			set { }
		}

		public RedisType Type
		{
			get
			{
				return redisData.Type;
			}
			set { }
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

		public void RowChanged(SelectionChangedEventArgs selectionChangedEventArgs)
		{
			if (selectionChangedEventArgs.AddedItems.Count == 0)
			{
				return;
			}
			this.selectedItemIndex = (selectionChangedEventArgs.Source as DataGrid).SelectedIndex;
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
