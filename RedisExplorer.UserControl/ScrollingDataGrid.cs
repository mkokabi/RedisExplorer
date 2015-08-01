using System.Windows.Controls;

namespace RedisExplorer.UserControl
{
	using System.Collections;

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
						DataViewModel dataViewModel = this.DataContext as DataViewModel;
						if (dataViewModel == null)
						{
							return;
						}
						IList values = null;
						if (type == RedisType.List || type == RedisType.Set)
						{
							values = dataViewModel.Values;
						}
						else if (type == RedisType.Hash)
						{
							values = dataViewModel.Hash;
						}
						else if (type == RedisType.SortedSet)
						{
							values = dataViewModel.SortedSet;
						}
						if (values == null)
						{
							return;
						}
						if (values.Count > 0)
						{
							this.SelectedIndex = values.Count - 1;
							this.ScrollIntoView(values[values.Count - 1]);
						}
				}
			);
		}
	}
}
