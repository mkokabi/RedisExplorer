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

	using MessageBox = System.Windows.MessageBox;

	public class RedisExplorerViewModel
		: ViewModelBase
	{
		#region Private fields
		string redisUrl;

		string currentDatabase;

		bool editMode;

		RedisData selectedItem;

		ICommand connectCommand;

		ICommand saveCommand;

		ICommand cancelCommand;

		ICommand valueChangedCommand;

		ICommand rowChangedCommand;

		ICommand gridDoubleClickCommand;

		ValueEditorViewModel selectedValueEditorViewModel;

		IManager manager;
		
		#endregion

		public RedisExplorerViewModel(IManager manager)
		{
			KeyValueCollection = new RedisDataCollection();
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

		public ICommand ValueChangedCommand
		{
			get
			{
				return valueChangedCommand ?? (valueChangedCommand = new RelayCommand<string>(ValueChanged));
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
				return gridDoubleClickCommand ?? (gridDoubleClickCommand = new RelayCommand<RedisData>(SwitchToEditMode));
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
				manager.GetData(redisDatabase).ToList().ForEach(KeyValueCollection.Add);
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
			this.SelectedValueEditorViewModel.Data = this.SelectedItem;
			// MessageBox.Show("SwitchToEditMode " + redisData.Key + " " + redisData.Value);
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
	}
}
