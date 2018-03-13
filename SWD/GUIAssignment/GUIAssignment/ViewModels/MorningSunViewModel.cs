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
using GUIAssignment.Models;
using GUIAssignment.ViewModel;
using GUIAssignment.Views;
using JetBrains.Annotations;

namespace GUIAssignment.ViewModels
{
	public class MorningSunViewModel : ViewModelBase, INotifyPropertyChanged
	{
		private MorningSunModel _model = new MorningSunModel();
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}




	
		
		public int LightIntensity
		{
			get { return _model.LightIntensity; }
			set { _model.ChangeLightIntensity(value); OnPropertyChanged("LightIntensity"); }
		}

		public bool Powered
		{
			get { return _model.Powered; }
			
		}

		private ICommand _morningSunToggleCommand;

		public ICommand MorningSunToggleCommand => _morningSunToggleCommand ??
		                                           (_morningSunToggleCommand = new RelayCommand(ToggleLight));

		private void ToggleLight()
		{
			_model.ToggleLight();
			Debug.Write(_model.Powered);
			OnPropertyChanged("Powered");

		}
	}
}
