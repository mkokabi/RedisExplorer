/*
  In App.xaml:
  <Application.Resources>
	  <vm:ViewModelLocator xmlns:vm="clr-namespace:RedisExplorer"
						   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

namespace RedisExplorer.ViewModel
{
	// TODO: Use Autofac
	// using Autofac;
	// using Autofac.Extras.CommonServiceLocator;

	using GalaSoft.MvvmLight.Ioc;

	using Microsoft.Practices.ServiceLocation;

	using RedisExplorer.Common;
	using RedisExplorer.Manager;
	using RedisExplorer.UserControl.ViewModel;

	/// <summary>
	/// This class contains static references to all the view models in the
	/// application and provides an entry point for the bindings.
	/// </summary>
	public class ViewModelLocator
	{
		// readonly IContainer container;

		/// <summary>
		/// Initializes a new instance of the ViewModelLocator class.
		/// </summary>
		public ViewModelLocator()
		{
			// var builder = new ContainerBuilder();
			   
			// builder.RegisterType<Manager>().As<IManager>();
			// builder.RegisterType<RedisExplorerViewModel>();
			// builder.RegisterType<ValueEditorViewModel>();
			   
			// var container = builder.Build();
			// ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
			
			// -------------------------------

			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
			SimpleIoc.Default.Register<IManager, Manager>();
			SimpleIoc.Default.Register<RedisExplorerViewModel>();
			SimpleIoc.Default.Register<ValueEditorViewModel>();
		}

		/// <summary>
		/// The accessor to main explorer view model
		/// </summary>
		public RedisExplorerViewModel Main
		{
			get
			{
				return ServiceLocator.Current.GetInstance<RedisExplorerViewModel>();
			}
		}

		/// <summary>
		/// The accessor to value editor view model
		/// </summary>
		public ValueEditorViewModel ValueEditorViewModel
		{
			get
			{
				return ServiceLocator.Current.GetInstance<ValueEditorViewModel>();
			}
		}
		
		/// <summary>
		/// Clean up the view model
		/// </summary>
		public static void Cleanup()
		{
			// TODO Clear the ViewModels
		}
	}
}