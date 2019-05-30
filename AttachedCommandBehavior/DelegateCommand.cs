/*
 * Source: http://wpftutorial.net/DelegateCommand.html and https://marlongrech.wordpress.com/2008/12/04/attachedcommandbehavior-aka-acb/
 */

using System;
using System.Windows.Input;

namespace AttachedCommandBehavior
{

    /// <summary>
    /// Provides a base class for WPF commands.
    /// </summary>
    public class DelegateCommand : ICommand
    {

        private bool _canExecute = true;

        /// <summary>
        /// Gets or sets the <see cref="Predicate{Object}"/> to execute when the CanExecute of the command gets called
        /// </summary>
        public Predicate<object> CanExecuteDelegate { get; set; } = null;

        /// <summary>
        /// Gets or sets the <see cref="Action{Object}"/> to be called when the <see cref="Execute"/> method of the command gets called
        /// </summary>
        public Action<object> ExecuteDelegate { get; set; } = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">The <see cref="Action{Object}"/> delegate</param>
        public DelegateCommand(Action<object> execute)
                       : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class using a custom <see cref="Predicate{Object}"/>.
        /// </summary>
        /// <param name="execute">The <see cref="Action{Object}"/> delegate</param>
        /// <param name="canExecute">The <see cref="Predicate{Object}"/> for this command</param>
        public DelegateCommand(Action<object> execute,
                       Predicate<object> canExecute)
        {
            ExecuteDelegate = execute;
            CanExecuteDelegate = canExecute;
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Checks if the command <see cref="Execute"/> method can run.
        /// </summary>
        /// <param name="parameter">The command parameter to be passed</param>
        /// <returns>Returns <see langword="true"/> if the command can execute. By default <see langword="true"/> is returned so that if the user of <see cref="DelegateCommand"/> does not specify a CanExecuteCommand delegate the command still executes.</returns>
        public virtual bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate == null)

                return true;

            bool result = CanExecuteDelegate(parameter);

            if (_canExecute != result)

                _canExecute = result;

            RaiseCanExecuteChanged();

            return result;
        }

        /// <summary>
        /// Executes the actual command.
        /// </summary>
        /// <param name="parameter">The command parameter to be passed</param>
        public virtual void Execute(object parameter) => ExecuteDelegate(parameter);

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        protected void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        #endregion
    }

    /// <summary>
    /// Provides a base class for WPF commands.
    /// </summary>
    public class DelegateCommand<T> : ICommand
    {

        private bool _canExecute = true;

        /// <summary>
        /// Gets or sets the <see cref="Predicate{T}"/> to execute when the CanExecute of the command gets called
        /// </summary>
        public Predicate<T> CanExecuteDelegate { get; set; } = null;

        /// <summary>
        /// Gets or sets the <see cref="Action{T}"/> to be called when the <see cref="Execute"/> method of the command gets called
        /// </summary>
        public Action<T> ExecuteDelegate { get; set; } = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">The <see cref="Action{T}"/> delegate</param>
        public DelegateCommand(Action<T> execute)
                       : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class using a custom <see cref="Predicate{Object}"/>.
        /// </summary>
        /// <param name="execute">The <see cref="Action{T}"/> delegate</param>
        /// <param name="canExecute">The <see cref="Predicate{T}"/> for this command</param>
        public DelegateCommand(Action<T> execute,
                       Predicate<T> canExecute)
        {
            ExecuteDelegate = execute;
            CanExecuteDelegate = canExecute;
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Checks if the command <see cref="Execute"/> method can run.
        /// </summary>
        /// <param name="parameter">The command parameter to be passed</param>
        /// <returns>Returns <see langword="true"/> if the command can execute. By default <see langword="true"/> is returned so that if the user of <see cref="DelegateCommand"/> does not specify a CanExecuteCommand delegate the command still executes.</returns>
        public virtual bool CanExecute(T parameter)
        {
            if (CanExecuteDelegate == null)

                return true;

            bool result = CanExecuteDelegate(parameter);

            if (_canExecute != result)

                _canExecute = result;

            RaiseCanExecuteChanged();

            return result;
        }

        /// <summary>
        /// Executes the actual command.
        /// </summary>
        /// <param name="parameter">The command parameter to be passed</param>
        public virtual void Execute(T parameter) => ExecuteDelegate(parameter);

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        protected void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        bool ICommand.CanExecute(object parameter) => CanExecute((T)parameter);

        void ICommand.Execute(object parameter) => Execute((T)parameter);

        #endregion
    }
}
