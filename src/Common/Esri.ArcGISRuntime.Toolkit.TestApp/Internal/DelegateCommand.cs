using System;
using System.Windows.Input;

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Internal
{
	internal class DelegateCommand : ICommand
	{
		private readonly Action<object> _execute;
		private readonly Func<object, bool> _canExecute; 

		public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute(parameter);
		}

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			_execute(parameter);
		}

		public void OnCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
				CanExecuteChanged(this, EventArgs.Empty);
		}
	}

}
