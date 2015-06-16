using System.Windows;

namespace RedisExplorer_WpfHostApp
{
	using RedisExplorer.UserControl;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// Initialises a new instance of the <see cref="MainWindow"/> class.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
			// this.Content = new MyControl();
			this.Content = new Explorer();
		}
	}
}
