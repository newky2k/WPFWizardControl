using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSoft.WizardControl
{
	/// <summary>
	/// Class DelegateCommand.
	/// Implements the <see cref="ICommand" />
	/// </summary>
	/// <seealso cref="ICommand" />
	internal class DelegateCommand : ICommand
    {
		#region Static Properties

		/// <summary>
		/// Gets or sets the execute changed.
		/// </summary>
		/// <value>The execute changed.</value>
		public static Action<EventHandler, bool> ExecuteChanged { get; set; }

		/// <summary>
		/// If set, the ViewModels will requery an ICommand properties when NotifyPropertyChanged is set
		/// </summary>
		/// <value><c>true</c> if [requery commands on change]; otherwise, <c>false</c>.</value>
		public static bool RequeryCommandsOnChange { get; set; }
        #endregion

        #region Fields
        private ExecuteMethod executeMethod;
        private ExecuteMethodWithParameter executeMethodWithParam;
        private Func<object, bool> canExecute;
		#endregion

		#region Properties
		/// <summary>
		/// Delegate ExecuteMethod
		/// </summary>
		public delegate void ExecuteMethod();
		/// <summary>
		/// Delegate ExecuteMethodWithParameter
		/// </summary>
		/// <param name="parameter">The parameter.</param>
		public delegate void ExecuteMethodWithParameter(object parameter);
		#endregion

		#region Events


		/// <summary>
		/// Occurs when changes occur that affect whether or not the command should execute.
		/// </summary>
		/// <returns></returns>
		public event EventHandler CanExecuteChanged
        {
            add
            {
                ExecuteChanged?.Invoke(value, true);

            }
            remove
            {
                ExecuteChanged?.Invoke(value, false);
            }
        }
		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateCommand" /> class.
		/// </summary>
		/// <param name="exec">The execute method</param>
		public DelegateCommand(ExecuteMethod exec)
        {
            executeMethod = exec;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateCommand" /> class.
		/// </summary>
		/// <param name="exec">The execute method that takes a parameter</param>
		public DelegateCommand(ExecuteMethodWithParameter exec)
        {
            executeMethodWithParam = exec;
        }


		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateCommand" /> class.
		/// </summary>
		/// <param name="exec">The execute method</param>
		/// <param name="canExecutePredicate">Predicate Function with object parameter</param>
		public DelegateCommand(ExecuteMethod exec, Func<object, bool> canExecutePredicate)
            : this(exec)
        {
            canExecute = canExecutePredicate;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateCommand" /> class.
		/// </summary>
		/// <param name="exec">The execute method that takes a parameter</param>
		/// <param name="canExecutePredicate">Predicate Function with object parameter</param>
		public DelegateCommand(ExecuteMethodWithParameter exec, Func<object, bool> canExecutePredicate)
            : this(exec)
        {
            canExecute = canExecutePredicate;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateCommand" /> class.
		/// </summary>
		/// <param name="exec">The execute method</param>
		/// <param name="canExecutePredicate">Predicate Function without object parameter</param>
		public DelegateCommand(ExecuteMethod exec, Func<bool> canExecutePredicate)
            : this(exec)
        {
            canExecute = (obj) =>
            {
                return canExecutePredicate.Invoke();
            };
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateCommand" /> class.
		/// </summary>
		/// <param name="exec">The execute method</param>
		/// <param name="canExecutePredicate">Predicate Function without object parameter</param>
		public DelegateCommand(ExecuteMethodWithParameter exec, Func<bool> canExecutePredicate)
            : this(exec)
        {
            canExecute = (obj) =>
            {
                return canExecutePredicate.Invoke();
            };
        }

		#endregion

		#region Methods

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		/// <returns>true if this command can be executed; otherwise, false.</returns>
		public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }
            else
            {
                return canExecute(parameter);
            }
        }

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public void Execute(object parameter)
        {
            if (executeMethod != null)
                executeMethod();
            else if (executeMethodWithParam != null)
                executeMethodWithParam(parameter);
        }

		/// <summary>
		/// Raises the can execute changed.
		/// </summary>
		public void RaiseCanExecuteChanged()
        {

        }
        #endregion


    }
}
