using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		string redisUrl;

		ICommand connectCommand;
		
		public RedisExplorerViewModel()
		{
			KeyValueCollection = new ObservableCollection<RedisData>();
			this.redisUrl = "localhost:6379";
		}

		public ICommand ConnectCommand
		{
			get
			{
				return connectCommand ?? (connectCommand = new RelayCommand(() => { Connect(); }));
			}
		}

		/// <summary>
		/// Gets or sets the key value collection.
		/// </summary>
		public ObservableCollection<RedisData> KeyValueCollection
		{
			get;
			set;
		}

		public string RedisUrl
		{
			set
			{
				if (value != this.redisUrl)
				{
					this.redisUrl = value;
					this.OnPropertyChanged("RedisUrl");
				}
			}
			get
			{
				return this.redisUrl; 
			}
		}
		
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
				keys.ToList().ForEach(key => KeyValueCollection.Add(new RedisData(key, redisDatabase.StringGet(key))));
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format("Can not connect to {0}. {1}", this.RedisUrl,  ex.Message),
				   "Redis Explorer");
			}
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
