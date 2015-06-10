namespace RedisExplorer.Controls
{
	using RedisExplorer.ViewModel;

	/// <summary>
	/// Interaction logic for ValueEditor.xaml
	/// </summary>
	public partial class ValueEditor : System.Windows.Controls.UserControl
	{
		public ValueEditor()
		{
			InitializeComponent();
			this.DataContext = new ViewModelLocator().ValueEditorViewModel;
		}
	}
}
