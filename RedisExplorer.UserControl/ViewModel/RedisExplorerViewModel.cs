using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace RedisExplorer.UserControl.ViewModel
{
	using GalaSoft.MvvmLight;
	using GalaSoft.MvvmLight.Command;

	using RedisExplorer.Common;
	using RedisExplorer.Common.DataTypes;
	using RedisExplorer.ViewModel;

	using StackExchange.Redis;

	/// <summary>
	/// Redis explorer view model.
	/// </summary>
	public class RedisExplorerViewModel
		: ViewModelBase
	{
		#region Private fields
		string redisUrl;

		string currentDatabase;

		bool editMode;

		DataViewModel selectedItem;

		ICommand connectCommand;

		ICommand saveCommand;

		ICommand cancelCommand;

		ICommand rowChangedCommand;

		ICommand gridDoubleClickCommand;

		ValueEditorViewModel selectedValueEditorViewModel;

		IManager manager;

		ICommand newCommand;

		ICommand deleteCommand;

		bool isConnected;

		ICommand findKeyCommand;

		string keyRegex;

		int selectedIndex;

		bool isGridFocused;

		ICommand addEntryCommand;

		ICommand removeEntryCommand;

		#endregion

		/// <summary>
		/// Initialises a new instance of the <see cref="RedisExplorerViewModel"/> class.
		/// </summary>
		/// <param name="manager">
		/// The manager.
		/// </param>
		public RedisExplorerViewModel(IManager manager)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			this.KeyValueCollection = new RedisDataCollection();
			this.Databases  = new ObservableCollection<string>();
			this.redisUrl = "localhost:6379";
			this.editMode = false;
			this.selectedValueEditorViewModel = new ViewModelLocator().ValueEditorViewModel;
			this.manager = manager;
			this.isConnected = false;
			this.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == "CurrentDatabase")
				{
					if (this.Databases.Any())
					{
						this.SwitchDatabase(this.currentDatabase);
					}
				}
			};
		}

		#region Commands

		/// <summary>
		/// The connect command called by clicking on Connect button.
		/// </summary>
		public ICommand ConnectCommand
		{
			get
			{
				return this.connectCommand ?? (this.connectCommand = new RelayCommand(this.Connect));
			}
		}

		/// <summary>
		/// The add entry command called by clicking on New Entry button.
		/// </summary>
		public ICommand AddEntryCommand
		{
			get
			{
				return this.addEntryCommand ?? (this.addEntryCommand = new RelayCommand(this.AddEntry));
			}
		}

		/// <summary>
		/// The remove entry command called by clicking on Remove Entry button.
		/// </summary>
		public ICommand RemoveEntryCommand
		{
			get
			{
				return this.removeEntryCommand ?? (this.removeEntryCommand = new RelayCommand(this.RemoveEntry));
			}
		}

		/// <summary>
		/// The Save command called by clicking on Save button on the toolbar while in edit mode.
		/// </summary>
		public ICommand SaveCommand
		{
			get
			{
				return this.saveCommand ?? (this.saveCommand = new RelayCommand(this.Save));
			}
		}

		/// <summary>
		/// The new command called by clicking on New button on main toolbar.
		/// </summary>
		public ICommand NewCommand
		{
			get
			{
				return this.newCommand ?? (this.newCommand = new RelayCommand<RedisType>(this.New));
			}
		}

		/// <summary>
		/// The delete command called by clicking on Delete button on main toolbar.
		/// </summary>
		public ICommand DeleteCommand
		{
			get
			{
				return this.deleteCommand ?? (this.deleteCommand = new RelayCommand(this.Delete));
			}
		}

		/// <summary>
		/// The Cancel command called by clicking on Save button on the toolbar while in edit mode.
		/// </summary>
		public ICommand CancelCommand
		{
			get
			{
				return this.cancelCommand ?? (this.cancelCommand = new RelayCommand(this.Cancel));
			}
		}

		/// <summary>
		/// The row changed command while changing the selected row on the current database data grid.
		/// </summary>
		public ICommand RowChangedCommand
		{
			get
			{
				return this.rowChangedCommand ?? (this.rowChangedCommand = new RelayCommand<IList>(this.RowChanged));
			}
		}

		/// <summary>
		/// The double click command on the the current database data grid.
		/// </summary>
		public ICommand GridDoubleClickCommand
		{
			get
			{
				return this.gridDoubleClickCommand ?? (this.gridDoubleClickCommand = new RelayCommand<DataViewModel>(this.SwitchToEditMode));
			}
		}

		/// <summary>
		/// The find key command.
		/// </summary>
		public ICommand FindKeyCommand
		{
			get
			{
				return this.findKeyCommand ?? (this.findKeyCommand = new RelayCommand(this.FindKey));
			}
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The selected item in the current database data grid.
		/// </summary>
		public DataViewModel SelectedItem
		{
			get
			{
				return this.selectedItem;
			}
			set
			{
				if (this.Set(() => this.SelectedItem, ref this.selectedItem, value))
				{
					this.RaisePropertyChanged(() => IsListType);
				}
			}
		}

		/// <summary>
		/// The view model of selected value in current database.
		/// </summary>
		public ValueEditorViewModel SelectedValueEditorViewModel
		{
			get
			{
				return this.selectedValueEditorViewModel;
			}
			set
			{
				this.Set(() => this.SelectedValueEditorViewModel, ref this.selectedValueEditorViewModel, value);
			}
		}

		/// <summary>
		/// The list of databases on this Redis server.
		/// </summary>
		public ObservableCollection<string> Databases
		{
			get;
			set;
		}

		/// <summary>
		/// The current database on this Redis server.
		/// </summary>
		public string CurrentDatabase
		{
			get
			{
				return this.currentDatabase;
			}
			set
			{
				this.Set(() => this.CurrentDatabase, ref this.currentDatabase, value);
			}
		}
		/// <summary>
		/// The key value collection of data.
		/// </summary>
		public RedisDataCollection KeyValueCollection
		{
			get;
			set;
		}

		/// <summary>
		/// Redis url.
		/// </summary>
		public string RedisUrl
		{
			get
			{
				return this.redisUrl;
			}
			set
			{
				this.Set(() => this.RedisUrl, ref this.redisUrl, value);
			}
		}

		/// <summary>
		/// A flag indicating whether the explorer is in edit mode.
		/// </summary>
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
					this.RaisePropertyChanged(() => this.EditMode);
					this.RaisePropertyChanged(() => this.BrowseMode);
					this.RaisePropertyChanged(() => this.EditPanelVisibility);
				}
			}
		}

		/// <summary>
		/// Return the edit panel visibility.
		/// </summary>
		public Visibility EditPanelVisibility
		{
			get
			{
				return this.editMode ? Visibility.Visible : Visibility.Collapsed;
			}
		}

		/// <summary>
		/// Returns a flag indicating whether explorer is in browse mode.
		/// </summary>
		public bool BrowseMode
		{
			get
			{
				return !this.EditMode;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether Explorer is connected.
		/// </summary>
		public bool IsConnected
		{
			get
			{
				return this.isConnected;
			}
			private set
			{
				Set(() => IsConnected, ref this.isConnected, value);
			}
		}

		private static readonly RedisType[] ListTypes = 
					{
						RedisType.List, RedisType.Hash, RedisType.Set, RedisType.SortedSet
					};

		/// <summary>
		/// A flag indicating whether the current item is a list type.
		/// </summary>
		public Visibility IsListType
		{
			get
			{
				if (this.SelectedItem != null)
				{
					return ListTypes.Contains(this.SelectedItem.Type) ? Visibility.Visible : Visibility.Collapsed;
				}
				return Visibility.Collapsed;
			}
		}

		/// <summary>
		/// The regex string for looking up a key.
		/// </summary>
		public string KeyRegex
		{
			get
			{
				return this.keyRegex;
			}
			set
			{
				this.Set(() => KeyRegex, ref keyRegex, value);
			}
		}

		/// <summary>
		/// The selected index in the grid.
		/// </summary>
		public int SelectedIndex
		{
			get
			{
				return this.selectedIndex;
			}
			set
			{
				Set(() => SelectedIndex, ref selectedIndex, value);
			}
		}

		/// <summary>
		/// Set or reset the grid selection status.
		/// </summary>
		public bool IsGridFocused
		{
			get
			{
				return this.isGridFocused;
			}
			set
			{
				Set(() => this.IsGridFocused, ref this.isGridFocused, value);
			}
		}

		#endregion

		/// <summary>
		/// Connect to the redis server on the Url.
		/// </summary>
		public void Connect()
		{
			try
			{
				this.manager.Connect(this.RedisUrl);
				IsConnected = true;
				this.Databases.Clear();
				this.manager.GetDatabases().ToList().ForEach(dbName => this.Databases.Add(dbName));
				
				this.CurrentDatabase = this.Databases[0];
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format("Can not connect to {0}. {1}", this.RedisUrl, ex.Message), "Redis Explorer");
			}
		}

		/// <summary>
		/// Switch database.
		/// </summary>
		/// <param name="redisDatabase">
		/// The redis database name.
		/// </param>
		public void SwitchDatabase(string redisDatabase)
		{
			try
			{
				this.KeyValueCollection.Clear();
				this.manager.GetKeys(redisDatabase).ToList().ForEach(data => this.KeyValueCollection.Add(new DataViewModel(data)));
				this.SelectedItem = this.KeyValueCollection.FirstOrDefault();
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format("Can not connect to {0}. {1}", this.RedisUrl,  ex.Message),
				   "Redis Explorer");
			}
		}

		/// <summary>
		/// Switch from browse to edit mode.
		/// </summary>
		/// <param name="redisData">
		/// The redis data.
		/// </param>
		public void SwitchToEditMode(DataViewModel redisData)
		{
			var index = this.KeyValueCollection.FindIndexByKey(this.SelectedItem.Key);
			this.SelectedItem = new DataViewModel(this.manager.GetValue(this.currentDatabase, redisData.Type, redisData.Key));
			this.EditMode = true;
			this.SelectedValueEditorViewModel.Data = this.SelectedItem;

			if (!this.SelectedValueEditorViewModel.IsNew)
			{
				this.KeyValueCollection[index] = this.SelectedItem;
			}
		}

		/// <summary>
		/// The actual remove entry method
		/// </summary>
		public void RemoveEntry()
		{
			// TODO: Add the other list types
			if (this.SelectedItem.RedisData.Type == RedisType.List)
			{
				// TODO: Investigate how the following two commands can be merged
				this.SelectedItem.Values.Remove(this.SelectedValueEditorViewModel.Data.SelectedItem);
				this.SelectedItem.RedisData.Values.Remove(this.SelectedValueEditorViewModel.Data.SelectedItem);
			}			
		}

		/// <summary>
		/// The actual add entry method.
		/// </summary>
		public void AddEntry()
		{
			// TODO: Add the other list types
			if (this.SelectedItem.RedisData.Type == RedisType.List)
			{
				// TODO: Investigate how the following two commands can be merged
				this.SelectedItem.Values.Add(string.Empty);
				this.SelectedItem.RedisData.Values.Add(string.Empty);
			}
		}

		/// <summary>
		/// The actual save method behind save command.
		/// </summary>
		public void Save()
		{
			this.manager.Update(this.currentDatabase, this.selectedItem.RedisData);
			this.EditMode = false;
			this.SelectedItem = new DataViewModel(this.manager.GetValue(this.currentDatabase, this.selectedItem.RedisData.Type, this.selectedItem.RedisData.Key));

			if (this.SelectedValueEditorViewModel.IsNew)
			{
				this.KeyValueCollection.Add(this.SelectedItem);
			}
			else
			{
				var index = this.KeyValueCollection.FindIndexByKey(this.SelectedItem.Key);
				this.KeyValueCollection[index] = this.SelectedItem;
			}

			this.SelectedValueEditorViewModel.IsNew = false;
		}

		/// <summary>
		/// The actual new method behind the New command.
		/// </summary>
		/// <param name="type">
		/// The type of the key to be created.
		/// </param>
		public void New(RedisType type)
		{
			RedisData newRedisData = new RedisData(string.Empty, type);
			DataViewModel newDataViewModel = new DataViewModel(newRedisData);
			this.SelectedValueEditorViewModel.IsNew = true;
			this.SwitchToEditMode(newDataViewModel);
		}

		/// <summary>
		/// The actual delete method behind the Delete command.
		/// </summary>
		public void Delete()
		{
			if (MessageBox.Show(string.Format("{0} will be deleted, are you sure?", this.SelectedItem.Key), "Warning", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
			{
				try
				{
					this.manager.DeleteKey(this.CurrentDatabase, this.SelectedItem.Key);
				}
				catch (Exception ex)
				{
					MessageBox.Show(string.Format("Can not delete this key. {0}",  ex.Message),
						"Redis Explorer");
				}
				SwitchDatabase(this.CurrentDatabase);
			}
		}

		/// <summary>
		/// The actual cancel method behind cancel command.
		/// </summary>
		public void Cancel()
		{
			this.EditMode = false;
		}

		/// <summary>
		/// The find key.
		/// </summary>
		public void FindKey()
		{
			if (string.IsNullOrWhiteSpace(this.keyRegex))
			{
				MessageBox.Show(string.Format("Please enter a search string. {0}", this.keyRegex));
			}
			Regex regex = new Regex(this.keyRegex);
			// TODO: Add a method to RedisDataCollection
			var found = this.KeyValueCollection.FirstOrDefault(keyValue => regex.Match(keyValue.Key).Success);
			if (found == null)
			{
				MessageBox.Show(string.Format("Can not find and key as : {0}", this.keyRegex));
				return;
			}
			this.SelectedIndex = this.KeyValueCollection.FindIndexByKey(found.Key);
			this.IsGridFocused = true;
		}

		/// <summary>
		/// The actual row changed method.
		/// </summary>
		/// <param name="selectedCellsChangedEventArgs">
		/// The selected cells changed event args.
		/// </param>
		public void RowChanged(IList selectedCellsChangedEventArgs)
		{
			if (selectedCellsChangedEventArgs.Count == 0)
			{
				return;
			}
			this.SelectedItem = (DataViewModel)selectedCellsChangedEventArgs[0];
		}
	}
}
