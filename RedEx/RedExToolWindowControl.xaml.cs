namespace RedEx
{
    using RedisExplorer.UserControl;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for RedExToolWindowControl.
    /// </summary>
    public partial class RedExToolWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedExToolWindowControl"/> class.
        /// </summary>
        public RedExToolWindowControl()
        {
            this.InitializeComponent();

            this.Content = new Explorer();
        }
    }
}