using System.Windows;
using System.Windows.Controls;

namespace DimensionData.RedisExplorer.Controls
{
	using DimensionData.RedisExplorer.ViewModel;

	/// <summary>
	/// Interaction logic for StringValueEditor.xaml
	/// </summary>
	public partial class StringValueEditor : UserControl
	{
		public StringValueEditor()
		{
			InitializeComponent();
			this.DataContext = new ViewModelLocator().StringValueEditorViewModel;
		}

		//public RedisData Data
		//{
		//	get
		//	{
		//		return (RedisData)this.GetValue(DataProperty);
		//	}
		//	set
		//	{
		//		SetValue(DataProperty, value);
		//	}
		//}

		//public static readonly DependencyProperty DataProperty =
		//	DependencyProperty.Register(
		//	"Data",
		//	typeof(RedisData),
		//	typeof(StringValueEditor)
		//);
	}
}
