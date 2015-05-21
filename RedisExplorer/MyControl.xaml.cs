using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace DimensionData.RedisExplorer
{
	using DimensionData.RedisExplorer.ViewModel;

	using StackExchange.Redis;

	/// <summary>
	/// Interaction logic for MyControl.xaml
	/// </summary>
	public partial class MyControl : UserControl
	{
		public MyControl()
		{
			InitializeComponent();
			this.DataContext = new RedisExplorerViewModel();
			// _dataGrid.DataContext = KeyValueCollection;
		}
	}
}