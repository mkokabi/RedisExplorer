using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace RedisExplorer.ViewModel
{
	using GalaSoft.MvvmLight;
	using GalaSoft.MvvmLight.Command;

	using RedisExplorer.DataTypes;
	using RedisExplorer.UserControl.ViewModel;

	using MessageBox = System.Windows.MessageBox;

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
		
		#endregion

		public RedisExplorerViewModel(IManager manager)
		{
			KeyValueCollection = new ObservableCollection<DataViewModel>();
			Databases  = new ObservableCollection<string>();
			this.redisUrl = "localhost:6379";
			this.editMode = false;
			selectedValueEditorViewModel = new ViewModelLocator().ValueEditorViewModel;
			this.manager = manager;
			this.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == "CurrentDatabase")
				{
					if (Databases.Any())
					{
						this.SwitchDatabase(this.currentDatabase);
					}
				}
			};
		}

		#region Commands
		public ICommand ConnectCommand
		{
			get
			{
				return connectCommand ?? (connectCommand = new RelayCommand(Connect));
			}
		}

		public ICommand SaveCommand
		{
			get
			{
				return saveCommand ?? (saveCommand = new RelayCommand(Save));
			}
		}

		public ICommand CancelCommand
		{
			get
			{
				return cancelCommand ?? (cancelCommand = new RelayCommand(Cancel));
			}
		}

		public ICommand RowChangedCommand
		{
			get
			{
				return rowChangedCommand ?? (rowChangedCommand = new RelayCommand<IList>(RowChanged));
			}
		}

		public ICommand GridDoubleClickCommand
		{
			get
			{
				return gridDoubleClickCommand ?? (gridDoubleClickCommand = new RelayCommand<DataViewModel>(SwitchToEditMode));
			}
		} 
		#endregion

		#region Public properties
		public DataViewModel SelectedItem
		{
			get
			{
				return selectedItem;
			}
			set
			{
				Set(() => SelectedItem, ref selectedItem, value);
			}
		}

		public ValueEditorViewModel SelectedValueEditorViewModel
		{
			get
			{
				return selectedValueEditorViewModel;
			}
			set
			{
				Set(() => SelectedValueEditorViewModel, ref selectedValueEditorViewModel, value);
			}
		}

		public ObservableCollection<string> Databases
		{
			get;
			set;
		}

		public string CurrentDatabase
		{
			get
			{
				return this.currentDatabase;
			}
			set
			{
				Set(() => CurrentDatabase, ref currentDatabase, value);
			}
		}
		/// <summary>
		/// Gets or sets the key value collection.
		/// </summary>
		public ObservableCollection<DataViewModel> KeyValueCollection
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
				Set(() => RedisUrl, ref redisUrl, value);
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
					this.RaisePropertyChanged(() => EditMode);
					this.RaisePropertyChanged(() => BrowseMode);
					this.RaisePropertyChanged(() => EditPanelVisibility);
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
				manager.Connect(this.RedisUrl);
				Databases.Clear();
				manager.GetDatabases().ToList().ForEach(dbName => Databases.Add(dbName));
				
				this.CurrentDatabase = Databases[0];
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format("Can not connect to {0}. {1}", this.RedisUrl, ex.Message), "Redis Explorer");
			}
		}

		public void SwitchDatabase(string redisDatabase)
		{
			try
			{
				KeyValueCollection.Clear();
				manager.GetData(redisDatabase).ToList().ForEach(data => KeyValueCollection.Add(new DataViewModel(data)));
				SelectedItem = this.KeyValueCollection.FirstOrDefault();
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format("Can not connect to {0}. {1}", this.RedisUrl,  ex.Message),
				   "Redis Explorer");
			}
		}

		public void SwitchToEditMode(DataViewModel redisData)
		{
			this.EditMode = true;
			this.SelectedValueEditorViewModel.Data = this.SelectedItem;
		}
		
		public void Save()
		{
			this.manager.Update(currentDatabase, selectedItem.RedisData);
			this.EditMode = false;
		}

		public void Cancel()
		{
			this.EditMode = false;
		}

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
