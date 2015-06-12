using System;
using System.Windows;
using System.Windows.Input;

namespace RedisExplorer.ViewModel
{
	using GalaSoft.MvvmLight;
	using GalaSoft.MvvmLight.Command;

	using RedisExplorer.DataTypes;

	public class ValueEditorViewModel
		: ViewModelBase
	{
		#region private fields
		RedisData data;

		ICommand valueChangedCommand;
		#endregion

		#region Commands

		public ICommand ValueChangedCommand
		{
			get
			{
				return valueChangedCommand ?? (valueChangedCommand = new RelayCommand<string>(ValueChanged));
			}
		}
		#endregion

		public ValueEditorViewModel()
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
				Set(() => Data, ref data, value);
			}
		}


		public void ValueChanged(string textChanged)
		{
			try
			{
				// MessageBox.Show("ValueChanged " + selectedItem.Key + " " + textChanged);
				data.Value = textChanged;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Can not write the value back." + ex.Message);
			}
		}
	}
}
