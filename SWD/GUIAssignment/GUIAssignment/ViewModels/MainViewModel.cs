using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GUIAssignment;
using GUIAssignment.ViewModel;
using GUIAssignment.Views;
using JetBrains.Annotations;

namespace GUIAssignment.ViewModels
{
	public class MainViewModel : ViewModelBase, INotifyPropertyChanged
	{
		private Window _childWindow;
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator] 
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private ICommand _morningSunWindowCommand;

		public ICommand MorningSunWindowCommand => _morningSunWindowCommand ??
		                                           (_morningSunWindowCommand = new RelayCommand(OpenMorningSunWindow, CanOpenMorningSunWindow));

		private void OpenMorningSunWindow()
		{
			if (_childWindow != null)
				_childWindow.Close();
			_childWindow = new MorningSunWindow();
			_childWindow.Show();

		}

		private bool CanOpenMorningSunWindow()
		{
			return true;
		}
	}
}
