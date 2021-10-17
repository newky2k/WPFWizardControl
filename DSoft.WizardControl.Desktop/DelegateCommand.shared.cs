using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSoft.WizardControl
{
    public class DelegateCommand : ICommand
    {
        #region Static Properties

        public static Action<EventHandler, bool> ExecuteChanged { get; set; }

        /// <summary>
        /// If set, the ViewModels will requery an ICommand properties when NotifyPropertyChanged is set
        /// </summary>
        public static bool RequeryCommandsOnChange { get; set; }
        #endregion

        #region Fields
        private ExecuteMethod executeMethod;
        private ExecuteMethodWithParameter executeMethodWithParam;
        private Func<object, bool> canExecute;
        #endregion

        #region Properties
        public delegate void ExecuteMethod();
        public delegate void ExecuteMethodWithParameter(object parameter);
        #endregion

        #region Events


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
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="exec">The execute method</param>
        public DelegateCommand(ExecuteMethod exec)
        {
            executeMethod = exec;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="exec">The execute method that takes a parameter</param>
        public DelegateCommand(ExecuteMethodWithParameter exec)
        {
            executeMethodWithParam = exec;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="exec">The execute method</param>
        /// <param name="canExecutePredicate">Predicate Function with object parameter</param>
        public DelegateCommand(ExecuteMethod exec, Func<object, bool> canExecutePredicate)
            : this(exec)
        {
            canExecute = canExecutePredicate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="exec">The execute method that takes a parameter</param>
        /// <param name="canExecutePredicate">Predicate Function with object parameter</param>
        public DelegateCommand(ExecuteMethodWithParameter exec, Func<object, bool> canExecutePredicate)
            : this(exec)
        {
            canExecute = canExecutePredicate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
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
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
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

        public void Execute(object parameter)
        {
            if (executeMethod != null)
                executeMethod();
            else if (executeMethodWithParam != null)
                executeMethodWithParam(parameter);
        }

        public void RaiseCanExecuteChanged()
        {

        }
        #endregion


    }
}
