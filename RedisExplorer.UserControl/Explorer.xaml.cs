namespace RedisExplorer
{
	using RedisExplorer.ViewModel;

	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class Explorer : System.Windows.Controls.UserControl
	{
		public Explorer()
		{
			InitializeComponent();
			this.DataContext = new ViewModelLocator().Main;
		}
	}
}
