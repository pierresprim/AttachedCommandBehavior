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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;
using AttachedCommandBehavior;

namespace AttachedCommandBehaviorDemo
{
    public class DemoViewModel
    {
        /// <summary>
        /// Gets the list of events to bind to
        /// </summary>
        public IList<string> Events { get; private set; }

        /// <summary>
        /// Gets the list of Messages populated (The messages are the names of the events that execute the commands)
        /// </summary>
        public IList<string> Messages { get; private set; }

        /// <summary>
        /// Gets an action that adds a message
        /// </summary>
        public Action<object> DoSomething { get; private set; }

        /// <summary>
        /// Command that clears the list of messages
        /// </summary>
        public ICommand ClearMessagesCommand { get; private set; }

        /// <summary>
        /// Command that write the event name that executed the command
        /// </summary>
        public ICommand SomeCommand { get; private set; }

        public DemoViewModel()
        {
            DoSomething = x => Messages.Add("Action executed: " + x.ToString());
            Messages = new ObservableCollection<string>();
            Events = new[]
            {
                "PreviewMouseDown",
                "PreviewMouseUp",
                "PreviewMouseLeftButtonDown",
                "PreviewMouseLeftButtonUp",
                "PreviewMouseRightButtonDown",
                "PreviewMouseRightButtonUp",
                "MouseEnter",
                "MouseLeave"
            };

            SomeCommand = new DelegateCommand
            {
                //this will set the Message property to the value of the CommandParameter
                ExecuteDelegate = x => Messages.Add(x.ToString())
            };
            ClearMessagesCommand = new DelegateCommand
            {
                ExecuteDelegate = x => Messages.Clear(),
                CanExecuteDelegate = x => Messages.Count > 0
            };
            DoSomething = x => Messages.Add("Action executed: " + x.ToString());
        }
    }
}
