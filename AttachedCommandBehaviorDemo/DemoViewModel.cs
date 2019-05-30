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

            SomeCommand = new SimpleCommand
            {
                //this will set the Message property to the value of the CommandParameter
                ExecuteDelegate = x => Messages.Add(x.ToString())
            };
            ClearMessagesCommand = new SimpleCommand
            {
                ExecuteDelegate = x => Messages.Clear(),
                CanExecuteDelegate = x => Messages.Count > 0
            };
            DoSomething = x => Messages.Add("Action executed: " + x.ToString());
        }
    }
}
