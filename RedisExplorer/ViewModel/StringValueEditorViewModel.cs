using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DimensionData.RedisExplorer.ViewModel
{
	public class StringValueEditorViewModel
		: INotifyPropertyChanged
	{
		RedisData data;

		public StringValueEditorViewModel()
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
				if (value != this.data)
				{
					this.data = value;
					this.OnPropertyChanged("Data");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
