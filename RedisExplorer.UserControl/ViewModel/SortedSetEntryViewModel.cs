namespace RedisExplorer.UserControl.ViewModel
{
	using GalaSoft.MvvmLight;

	using StackExchange.Redis;

	/// <summary>
	/// The ViewModel of SortedSet entry.
	/// </summary>
	public class SortedSetEntryViewModel : ViewModelBase
	{
		#region private fields

		double score;

		RedisValue element;

		#endregion

		/// <summary>
		/// The score of SortedSet entry.
		/// </summary>
		public double Score
		{
			get
			{
				return this.score;
			}
			set
			{
				if (Set(() => Score, ref score, value))
				{
					Messages.SortedEntryScoreChanged.Send(value);
				}
			}
		}

		/// <summary>
		/// The value of SortedSet entry.
		/// </summary>
		public RedisValue Element
		{
			get
			{
				return this.element;
			}
			set
			{
				if (Set(() => this.Element, ref this.element, value))
				{
					Messages.SortedEntryValueChanged.Send(value);
				}
			}
		}

		/// <summary>
		/// Creates a new SortedSetEntry from a SortedSetEntryViewModel .
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <returns>
		/// </returns>
		public static implicit operator SortedSetEntry(SortedSetEntryViewModel value)
		{
			return new SortedSetEntry(value.Element, value.Score);
		}
	}
}
