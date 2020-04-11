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

using System;

namespace AttachedCommandBehavior
{
    /// <summary>
    /// Defines the interface for a strategy of execution for the CommandBehaviorBinding
    /// </summary>
    public interface IExecutionStrategy
    {
        /// <summary>
        /// Gets or sets the Behavior that we execute this strategy
        /// </summary>
        CommandBehaviorBinding Behavior { get; set; }

        /// <summary>
        /// Executes according to the strategy type
        /// </summary>
        /// <param name="parameter">The parameter to be used in the execution</param>
        void Execute(object parameter);
    }

    /// <summary>
    /// Executes a command 
    /// </summary>
    public class CommandExecutionStrategy : IExecutionStrategy
    {
        #region IExecutionStrategy Members
        /// <summary>
        /// Gets or sets the Behavior that we execute this strategy
        /// </summary>
        public CommandBehaviorBinding Behavior { get; set; }

        /// <summary>
        /// Executes the Command that is stored in the CommandProperty of the CommandExecution
        /// </summary>
        /// <param name="parameter">The parameter for the command</param>
        public void Execute(object parameter)
        {
            if (Behavior == null)
                throw new InvalidOperationException("Behavior property cannot be null when executing a strategy");
            if (Behavior.Command.CanExecute(Behavior.CommandParameter))
                Behavior.Command.Execute(Behavior.CommandParameter);
        }

        #endregion
    }

    /// <summary>
    /// executes a delegate
    /// </summary>
    public class ActionExecutionStrategy : IExecutionStrategy
    {

        #region IExecutionStrategy Members

        /// <summary>
        /// Gets or sets the Behavior that we execute this strategy
        /// </summary>
        public CommandBehaviorBinding Behavior { get; set; }

        /// <summary>
        /// Executes an Action delegate
        /// </summary>
        /// <param name="parameter">The parameter to pass to the Action</param>
        public void Execute(object parameter) => Behavior.Action(parameter);

        #endregion
    }

}
