using System.Collections.ObjectModel;
using System.Linq;

namespace RedisExplorer.UserControl
{
	using RedisExplorer.UserControl.ViewModel;

	/// <summary>
	/// An observable collection of DataViewModel.
	/// </summary>
	public class RedisDataCollection : ObservableCollection<DataViewModel>
	{
		/// <summary>
		/// Find the index of DataViewModel by the key.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <returns>
		/// The <see cref="int"/>.
		/// </returns>
		public int FindIndexByKey(string key)
		{
			return this.ToList().FindIndex(data => data.Key == key);
		}
	}
}
