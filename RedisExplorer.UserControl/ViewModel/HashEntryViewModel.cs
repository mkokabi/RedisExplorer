namespace RedisExplorer.UserControl.ViewModel
{
	using GalaSoft.MvvmLight;

	using StackExchange.Redis;

	/// <summary>
	/// The ViewModel of hash entry.
	/// </summary>
	public class HashEntryViewModel : ViewModelBase
	{
		#region private fields
		string _name;

		string _value;
		#endregion


		#region public properties

		/// <summary>
		/// The name of HashEntry.
		/// </summary>
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				Set(() => Name, ref _name, value);
			}
		}

		/// <summary>
		/// The value of HashEntry.
		/// </summary>
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				Set(() => Value, ref _value, value);
			}
		}
		#endregion

		/// <summary>
		/// Creates a new HashEntry from a HashEntryViewModel .
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <returns>
		/// </returns>
		public static implicit operator HashEntry(HashEntryViewModel value)
		{
			return new HashEntry(value.Name, value.Value);
		}
	}
}
