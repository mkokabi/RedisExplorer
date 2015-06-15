namespace RedisExplorer.ViewModel
{
	using GalaSoft.MvvmLight;

	using RedisExplorer.UserControl.ViewModel;

	public class ValueEditorViewModel
		: ViewModelBase
	{
		#region private fields
		DataViewModel data;
		#endregion

		#region Commands

		#endregion

		public ValueEditorViewModel()
		{
		}

		public DataViewModel Data
		{
			get
			{
				return this.data;
			}
			set
			{
				Set(() => Data, ref data, value);
			}
		}
	}
}
