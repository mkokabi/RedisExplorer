namespace RedisExplorer.ViewModel
{
	using GalaSoft.MvvmLight;

	using RedisExplorer.DataTypes;

	public class ValueEditorViewModel
		: ViewModelBase
	{
		RedisData data;

		public ValueEditorViewModel()
		{
			data = new RedisData();
		}

		public RedisData Data
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
