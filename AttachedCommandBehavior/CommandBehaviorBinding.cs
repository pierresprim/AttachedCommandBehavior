/* Source: https://dzone.com/articles/how-get-eventargs ; Author of original code: Marlon Grech */

using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace AttachedCommandBehavior
{
    /// <summary>
    /// Defines the command behavior binding
    /// </summary>
    public class CommandBehaviorBinding : IDisposable
    {
        #region Properties

        /// <summary>
        /// Get the owner of the CommandBinding ex: a Button
        /// This property can only be set from the BindEvent Method
        /// </summary>
        public DependencyObject Owner { get; private set; }
        /// <summary>
        /// The event name to hook up to
        /// This property can only be set from the BindEvent Method
        /// </summary>
        public string EventName { get; private set; }
        /// <summary>
        /// The event info of the event
        /// </summary>
        public EventInfo Event { get; private set; }
        /// <summary>
        /// Gets the EventHandler for the binding with the event
        /// </summary>
        public Delegate EventHandler { get; private set; }

        #region Execution
        //stores the strategy of how to execute the event handler
        IExecutionStrategy strategy;

        /// <summary>
        /// Gets or sets a CommandParameter
        /// </summary>
        public object CommandParameter { get; set; }

        ICommand command;
        /// <summary>
        /// The command to execute when the specified event is raised
        /// </summary>
        public ICommand Command
        {
            get => command;
            set
            {
                command = value;
                //set the execution strategy to execute the command
                strategy = new CommandExecutionStrategy { Behavior = this };
            }
        }

        Action<object> action;

        /// <summary>
        /// Gets or sets the Action
        /// </summary>
        public Action<object> Action
        {
            get => action;
            set
            {
                action = value;
                // set the execution strategy to execute the action
                strategy = new ActionExecutionStrategy { Behavior = this };
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// Creates an <see cref="System. EventHandler"/> on runtime and registers that handler to the Event specified
        /// </summary>
        /// <param name="owner">The <see cref="DependencyObject"/> owner</param>
        /// <param name="eventName">The event name</param>
        public void BindEvent(DependencyObject owner, string eventName)
        {

            EventName = eventName;
            Owner = owner;
            Event = Owner.GetType().GetEvent(EventName, BindingFlags.Public | BindingFlags.Instance);
            if (Event == null)
                throw new InvalidOperationException(string.Format("Could not resolve event name {0}", EventName));

            //Create an event handler for the event that will call the ExecuteCommand method
            EventHandler = EventHandlerGenerator.CreateDelegate(
                Event.EventHandlerType, typeof(CommandBehaviorBinding).GetMethod(nameof(Execute), BindingFlags.Public | BindingFlags.Instance), this);
            //Register the handler to the Event
            Event.AddEventHandler(Owner, EventHandler);
        }

        /// <summary>
        /// Executes the strategy
        /// </summary>
        public void Execute() => strategy.Execute(CommandParameter);

        #region IDisposable Members

        bool disposed = false;

        /// <summary>
        /// Unregisters the <see cref="EventHandler"/> from the <see cref="Event"/>
        /// </summary>
        public void Dispose()
        {
            if (disposed)

                return;

            Event.RemoveEventHandler(Owner, EventHandler);
            disposed = true;
        }

        #endregion
    }
}
