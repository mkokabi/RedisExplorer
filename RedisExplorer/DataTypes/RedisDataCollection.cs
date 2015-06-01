﻿
namespace DimensionData.RedisExplorer
{
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Windows.Forms;

	public class RedisDataCollection : ObservableCollection<RedisData>, IEditableObject
	{
		/// <summary>
		/// Begins an edit on an object.
		/// </summary>
		public void BeginEdit()
		{
		}

		/// <summary>
		/// Pushes changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> or <see cref="M:System.ComponentModel.IBindingList.AddNew"/> call into the underlying object.
		/// </summary>
		public void EndEdit()
		{
			MessageBox.Show("EndEdit");
		}

		/// <summary>
		/// Discards changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
		/// </summary>
		public void CancelEdit()
		{
		}
	}
}
