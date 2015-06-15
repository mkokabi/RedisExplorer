namespace RedisExplorer.UserControl.ViewModel
{
	using GalaSoft.MvvmLight;

	/// <summary>
	/// The value editor view model.
	/// </summary>
	public class ValueEditorViewModel
		: ViewModelBase
	{
		#region private fields
		DataViewModel data;
		#endregion

		#region Commands

		#endregion

		/// <summary>
		/// The data.
		/// </summary>
		public DataViewModel Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.Set(() => this.Data, ref this.data, value);
			}
		}
	}
}
