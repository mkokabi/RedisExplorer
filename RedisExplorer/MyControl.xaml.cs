using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace DimensionData.RedisExplorer
{
	using StackExchange.Redis;

	/// <summary>
	/// Interaction logic for MyControl.xaml
	/// </summary>
	public partial class MyControl : UserControl
	{
		public MyControl()
		{
			KeyValueCollection = new ObservableCollection<RedisData>();
			InitializeComponent();
			_dataGrid.DataContext = KeyValueCollection;
		}

		public class RedisData
		{
			public string Key
			{
				get;
				set;
			}

			public string Value
			{
				get;
				set;
			}

			public RedisData()
			{
			}

			public RedisData(string key, string value)
			{
				this.Key = key;
				this.Value = value;
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

		/// <summary>
		/// The private instance of redis database.
		/// </summary>
		IDatabase _redisDatabase;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
		private void Connect_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				ConnectionMultiplexer redisConnection = ConnectionMultiplexer.Connect(urlTextBox.Text);
				EndPoint[] endpoints = redisConnection.GetEndPoints();
				IServer redisServer = redisConnection.GetServer(endpoints[0]);
				IEnumerable<RedisKey> keys = redisServer.Keys(pageSize: 10);
				var redisDatabase = redisConnection.GetDatabase();
				KeyValueCollection.Clear();
				keys.ToList().ForEach(key => KeyValueCollection.Add(new RedisData(key, redisDatabase.StringGet(key))));
			}
			catch (Exception ex)
			{
				MessageBox.Show("Can not connect to " + ex.Message,
				   "Redis Explorer");
			}
		}
	}
}