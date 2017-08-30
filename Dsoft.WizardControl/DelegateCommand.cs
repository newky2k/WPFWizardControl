using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dsoft.WizardControl.WPF
{
    /// <summary>
    /// Delegated Command object
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private bool canExecute = true;
        private Action<object> action;

        public DelegateCommand(Action<object> action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }
        public void UpdateCanExecute(bool canExecute)
        {
            this.canExecute = canExecute;
            CanExecuteChanged(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public void Execute(object parameter)
        {
            action(parameter);
        }
    }
}
