using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RedisExplorer.UserControl.ViewModel
{
	using GalaSoft.MvvmLight;
	using GalaSoft.MvvmLight.Command;

	using RedisExplorer.Common;
	using RedisExplorer.ViewModel;

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
			this.KeyValueCollection = new ObservableCollection<DataViewModel>();
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
				return this.newCommand ?? (this.newCommand = new RelayCommand(this.New));
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
				this.Set(() => this.SelectedItem, ref this.selectedItem, value);
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
		public ObservableCollection<DataViewModel> KeyValueCollection
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
				this.manager.GetData(redisDatabase).ToList().ForEach(data => this.KeyValueCollection.Add(new DataViewModel(data)));
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
			this.EditMode = true;
			this.SelectedValueEditorViewModel.Data = this.SelectedItem;
		}

		/// <summary>
		/// The actual save method behind save command.
		/// </summary>
		public void Save()
		{
			this.manager.Update(this.currentDatabase, this.selectedItem.RedisData);
			this.EditMode = false;
		}

		/// <summary>
		/// The actual new method behind the New command.
		/// </summary>
		public void New()
		{
			MessageBox.Show("New");
		}

		/// <summary>
		/// The actual delete method behind the Delete command.
		/// </summary>
		public void Delete()
		{
			MessageBox.Show("Delete");
		}

		/// <summary>
		/// The actual cancel method behind cancel command.
		/// </summary>
		public void Cancel()
		{
			this.EditMode = false;
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
