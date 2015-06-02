using System.Windows;
using System.Windows.Controls;

namespace DimensionData.RedisExplorer.Controls
{
	using DimensionData.RedisExplorer.ViewModel;

	/// <summary>
	/// Interaction logic for ValueEditor.xaml
	/// </summary>
	public partial class ValueEditor : UserControl
	{
		public ValueEditor()
		{
			InitializeComponent();
			this.DataContext = new ViewModelLocator().ValueEditorViewModel;
		}
	}
}
