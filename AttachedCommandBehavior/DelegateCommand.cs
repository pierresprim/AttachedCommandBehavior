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

        /// <summary>
        /// Gets or sets the Predicate to execute when the CanExecute of the command gets called
        /// </summary>
        public Predicate<object> CanExecuteDelegate { get; set; }

        /// <summary>
        /// Gets or sets the action to be called when the Execute method of the command gets called
        /// </summary>
        public Action<object> ExecuteDelegate { get; set; }

        #region ICommand Members

        /// <summary>
        /// Checks if the command Execute method can run
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        /// <returns>Returns true if the command can execute. By default true is returned so that if the user of SimpleCommand does not specify a CanExecuteCommand delegate the command still executes.</returns>
        public bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate != null)
                return CanExecuteDelegate(parameter);
            return true;// if there is no can execute default to true
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Executes the actual command
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
                ExecuteDelegate(parameter);
        }

        #endregion
    }

    /// <summary>
    /// Provides a base class for WPF commands.
    /// </summary>
    public class DelegateCommand<T> : ICommand
    {

        /// <summary>
        /// Gets or sets the Predicate to execute when the CanExecute of the command gets called
        /// </summary>
        public Predicate<T> CanExecuteDelegate { get; set; }

        /// <summary>
        /// Gets or sets the action to be called when the Execute method of the command gets called
        /// </summary>
        public Action<T> ExecuteDelegate { get; set; }

        #region ICommand Members

        /// <summary>
        /// Checks if the command Execute method can run
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        /// <returns>Returns true if the command can execute. By default true is returned so that if the user of SimpleCommand does not specify a CanExecuteCommand delegate the command still executes.</returns>
        public bool CanExecute(T parameter)
        {
            if (CanExecuteDelegate != null)
                return CanExecuteDelegate(parameter);
            return true;// if there is no can execute default to true
        }

        bool ICommand.CanExecute(object parameter) => CanExecute((T)parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Executes the actual command
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        public void Execute(T parameter)
        {
            if (ExecuteDelegate != null)
                ExecuteDelegate(parameter);
        }

        void ICommand.Execute(object parameter) => Execute((T)parameter);

        #endregion
    }
}
