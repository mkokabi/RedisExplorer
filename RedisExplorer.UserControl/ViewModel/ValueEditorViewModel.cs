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

		bool isNew;

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

		/// <summary>
		/// A flag indicating whether this data is new.
		/// </summary>
		public bool IsNew
		{
			get
			{
				return this.isNew;
			}
			set
			{
				Set(() => IsNew, ref isNew, value);
				this.RaisePropertyChanged(() => ReadOnlyKey);
			}
		}

		/// <summary>
		/// A flag indicating whether the key should be read only.
		/// </summary>
		public bool ReadOnlyKey
		{
			get
			{
				return !this.IsNew;
			}
		}
	}
}
