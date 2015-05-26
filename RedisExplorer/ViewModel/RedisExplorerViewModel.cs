using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;


namespace DimensionData.RedisExplorer.ViewModel
{


	using GalaSoft.MvvmLight.Command;

	using StackExchange.Redis;

	public class RedisExplorerViewModel
		: INotifyPropertyChanged
	{
		#region Private fields
		string redisUrl;

		bool editMode;

		RedisData selectedItem;

		ICommand connectCommand;

		ICommand saveCommand;

		ICommand cancelCommand;

		ICommand valueChangedCommand;

		ICommand rowChangedCommand;

		ICommand gridDoubleClickCommand;
		#endregion

		public RedisExplorerViewModel()
		{
			KeyValueCollection = new RedisDataCollection();
			this.redisUrl = "localhost:6379";
			this.editMode = false;
		}

		#region Commands
		public ICommand ConnectCommand
		{
			get
			{
				return connectCommand ?? (connectCommand = new RelayCommand(() => { Connect(); }));
			}
		}

		public ICommand SaveCommand
		{
			get
			{
				return saveCommand ?? (saveCommand = new RelayCommand(() => { Save(); }));
			}
		}

		public ICommand CancelCommand
		{
			get
			{
				return cancelCommand ?? (cancelCommand = new RelayCommand(() => { Cancel(); }));
			}
		}

		public ICommand ValueChangedCommand
		{
			get
			{
				return valueChangedCommand ?? (valueChangedCommand = new RelayCommand<string>(textChanged => { ValueChanged(textChanged); }));
			}
		}

		public ICommand RowChangedCommand
		{
			get
			{
				return rowChangedCommand ?? (rowChangedCommand = new RelayCommand<IList>(items => { RowChanged(items); }));
			}
		}

		public ICommand GridDoubleClickCommand
		{
			get
			{
				return gridDoubleClickCommand ?? (gridDoubleClickCommand = new RelayCommand<RedisData>(selectedItem => { SwitchToEditMode(selectedItem); }));
			}
		} 
		#endregion

		#region Public properties
		public RedisData SelectedItem
		{
			get
			{
				return selectedItem;
			}
			set
			{
				if (value != selectedItem)
				{
					this.selectedItem = value;
					this.OnPropertyChanged("SelectedItem");
				}
			}
		}

		/// <summary>
		/// Gets or sets the key value collection.
		/// </summary>
		public RedisDataCollection KeyValueCollection
		{
			get;
			set;
		}

		public string RedisUrl
		{
			get
			{
				return this.redisUrl;
			}
			set
			{
				if (value != this.redisUrl)
				{
					this.redisUrl = value;
					this.OnPropertyChanged("RedisUrl");
				}
			}
		}

		public bool EditMode
		{
			get
			{
				return this.editMode;
			}
			set
			{
				if (value != this.editMode)
				{
					this.editMode = value;
					this.OnPropertyChanged("EditMode");
					this.OnPropertyChanged("BrowseMode");
					this.OnPropertyChanged("EditPanelVisibility");
				}
			}
		}

		public Visibility EditPanelVisibility
		{
			get
			{
				return editMode ? Visibility.Visible : Visibility.Collapsed;
			}
		}

		public bool BrowseMode
		{
			get
			{
				return !this.EditMode;
			}
		} 
		#endregion

		public void Connect()
		{
			try
			{
				ConnectionMultiplexer redisConnection = ConnectionMultiplexer.Connect(this.RedisUrl);
				EndPoint[] endpoints = redisConnection.GetEndPoints();
				IServer redisServer = redisConnection.GetServer(endpoints[0]);
				IEnumerable<RedisKey> keys = redisServer.Keys(pageSize: 10);
				var redisDatabase = redisConnection.GetDatabase();
				KeyValueCollection.Clear();
				keys.ToList().ForEach(key =>
					{
						try
						{
							if (redisDatabase.KeyType(key) == RedisType.String)
							{
								KeyValueCollection.Add(new RedisData(key, redisDatabase.StringGet(key)));
							}
							if (redisDatabase.KeyType(key) == RedisType.List)
							{
								KeyValueCollection.Add(new RedisData(key, redisDatabase.ListRange(key)));
							}
							if (redisDatabase.KeyType(key) == RedisType.Hash)
							{
								KeyValueCollection.Add(new RedisData(key, redisDatabase.HashGetAll(key)));
							}
						}
						catch (Exception ex)
						{
							KeyValueCollection.Add(new RedisData(key, ex.Message));
						}
					});
				SelectedItem = KeyValueCollection.FirstOrDefault();
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format("Can not connect to {0}. {1}", this.RedisUrl,  ex.Message),
				   "Redis Explorer");
			}
		}

		public void SwitchToEditMode(RedisData redisData)
		{
			this.EditMode = true;
			MessageBox.Show("SwitchToEditMode " + redisData.Key + " " + redisData.Value);
		}
		
		public void Save()
		{
			MessageBox.Show("Save " + selectedItem.Key + " " + selectedItem.Value);
			this.EditMode = false;
		}

		public void Cancel()
		{
			this.EditMode = false;
		}

		public void ValueChanged(string textChanged)
		{
			try
			{
				// MessageBox.Show("ValueChanged " + selectedItem.Key + " " + textChanged);
				KeyValueCollection.FirstOrDefault(item => item.Key == selectedItem.Key).Value = textChanged;
			}
			catch (Exception)
			{
				MessageBox.Show("Can not write the value back to the Redis");
			}
		}

		public void RowChanged(IList selectedCellsChangedEventArgs)
		{
			if (selectedCellsChangedEventArgs.Count == 0)
			{
				return;
			}
			this.SelectedItem = (RedisData)selectedCellsChangedEventArgs[0];
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
