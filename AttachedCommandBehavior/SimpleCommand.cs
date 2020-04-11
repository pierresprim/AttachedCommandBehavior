/* Source: https://dzone.com/articles/how-get-eventargs ; Author of original code: Marlon Grech
 * 
 * This is free and unencumbered software released into the public domain.
 *
 * Anyone is free to copy, modify, publish, use, compile, sell, or
 * distribute this software, either in source code form or as a compiled
 * binary, for any purpose, commercial or non-commercial, and by any
 * means.
 *
 * In jurisdictions that recognize copyright laws, the author or authors
 * of this software dedicate any and all copyright interest in the
 * software to the public domain. We make this dedication for the benefit
 * of the public at large and to the detriment of our heirs and
 * successors. We intend this dedication to be an overt act of
 * relinquishment in perpetuity of all present and future rights to this
 * software under copyright law.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * For more information, please refer to <http://unlicense.org> */

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Input;

//namespace AttachedCommandBehavior
//{
//    /// <summary>
//    /// Implements the ICommand and wraps up all the verbose stuff so that you can just pass 2 delegates 1 for the CanExecute and one for the Execute
//    /// </summary>
//    public class SimpleCommand : ICommand
//    {
//        /// <summary>
//        /// Gets or sets the Predicate to execute when the CanExecute of the command gets called
//        /// </summary>
//        public Predicate<object> CanExecuteDelegate { get; set; }

//        /// <summary>
//        /// Gets or sets the action to be called when the Execute method of the command gets called
//        /// </summary>
//        public Action<object> ExecuteDelegate { get; set; }

//        #region ICommand Members

//        /// <summary>
//        /// Checks if the command Execute method can run
//        /// </summary>
//        /// <param name="parameter">THe command parameter to be passed</param>
//        /// <returns>Returns true if the command can execute. By default true is returned so that if the user of SimpleCommand does not specify a CanExecuteCommand delegate the command still executes.</returns>
//        public bool CanExecute(object parameter)
//        {
//            if (CanExecuteDelegate != null)
//                return CanExecuteDelegate(parameter);
//            return true;// if there is no can execute default to true
//        }

//        public event EventHandler CanExecuteChanged
//        {
//            add { CommandManager.RequerySuggested += value; }
//            remove { CommandManager.RequerySuggested -= value; }
//        }

//        /// <summary>
//        /// Executes the actual command
//        /// </summary>
//        /// <param name="parameter">THe command parameter to be passed</param>
//        public void Execute(object parameter)
//        {
//            if (ExecuteDelegate != null)
//                ExecuteDelegate(parameter);
//        }

//        #endregion
//    }
//}
