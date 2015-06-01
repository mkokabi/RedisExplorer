
namespace DimensionData.RedisExplorer.ViewModel
{
	public class StringValueEditorViewModel
	{
		public StringValueEditorViewModel()
		{
			this.SelectedItem = new RedisData()
				{
					Key = "Key111", Value = "Value111"
				};
		}

		public RedisData SelectedItem
		{
			get;
			set;
		}
	}
}
