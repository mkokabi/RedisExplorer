using System.Windows.Controls;

namespace DimensionData.RedisExplorer
{
	using DimensionData.RedisExplorer.ViewModel;

	/// <summary>
	/// Interaction logic for MyControl.xaml
	/// </summary>
	public partial class MyControl : UserControl
	{
		public MyControl()
		{
			InitializeComponent();
			this.DataContext = new ViewModelLocator().Main;
		}
	}
}