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
	}
}
