using System.Windows.Controls;

namespace RedisExplorer.UserControl
{
	using RedisExplorer.UserControl.ViewModel;

	using StackExchange.Redis;

	/// <summary>
	/// A specialy customized data grid which selects and scrolls to the newly added row.
	/// </summary>
	public class ScrollingDataGrid : DataGrid
	{
		/// <summary>
		/// Initialises a new instance of the <see cref="ScrollingDataGrid"/> class.
		/// </summary>
		public ScrollingDataGrid()
		{
			Messages.EntryAdded.Register(
				this, 
				type =>
				{
					// TODO: support for other list types to be added
					if (type == RedisType.List)
					{
						var values = (this.DataContext as DataViewModel).Values;
						if (values.Count > 0)
						{
							this.SelectedIndex = values.Count - 1;
							this.ScrollIntoView(values[values.Count - 1]);
						}
					}
				}
			);
		}
	}
}
