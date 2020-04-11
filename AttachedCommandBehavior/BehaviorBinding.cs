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
using System.Windows;
using System.Windows.Input;

namespace AttachedCommandBehavior
{
    /// <summary>
    /// Provides a base class for behaviors.
    /// This inherits from freezable so that it gets inheritance context for DataBinding to work
    /// </summary>
    public abstract class Behavior : Freezable
    {

        internal int Id { get; set; }

        DependencyObject owner;

        /// <summary>
        /// Gets or sets the Owner of the binding
        /// </summary>
        public DependencyObject Owner
        {
            get => owner;
            set
            {
                owner = value;
                ResetBehavior();
            }
        }

        static void OwnerReset(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((BehaviorBinding)d).ResetBehavior();

        /// <summary>
        /// When overriden in a derived class, resets the behavior.
        /// </summary>
        protected abstract void ResetBehavior();

        /// <summary>
        /// This is not actually used. This is just a trick so that this object gets WPF Inheritance Context
        /// </summary>
        /// <returns></returns>
        protected override Freezable CreateInstanceCore() => throw new NotImplementedException();
    }

    /// <summary>
    /// Defines a Command Binding
    /// </summary>
    public class BehaviorBinding : Behavior

    {
        CommandBehaviorBinding behavior;

        /// <summary>
        /// Stores the Command Behavior Binding
        /// </summary>
        internal CommandBehaviorBinding Behavior => behavior ?? (behavior = new CommandBehaviorBinding());

        #region Command

        /// <summary>
        /// Command Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(BehaviorBinding),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnCommandChanged)));

        /// <summary>
        /// Gets or sets the Command property.  
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Handles changes to the Command property.
        /// </summary>
        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((BehaviorBinding)d).OnCommandChanged(e);

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Command property.
        /// </summary>
        protected virtual void OnCommandChanged(DependencyPropertyChangedEventArgs e) => Behavior.Command = Command;

        #endregion

        #region Action

        /// <summary>
        /// Action Dependency Property
        /// </summary>
        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.Register(nameof(Action), typeof(Action<object>), typeof(BehaviorBinding),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnActionChanged)));

        /// <summary>
        /// Gets or sets the Action property. 
        /// </summary>
        public Action<object> Action
        {
            get => (Action<object>)GetValue(ActionProperty);
            set => SetValue(ActionProperty, value);
        }

        /// <summary>
        /// Handles changes to the Action property.
        /// </summary>
        private static void OnActionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((BehaviorBinding)d).OnActionChanged(e);

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Action property.
        /// </summary>
        protected virtual void OnActionChanged(DependencyPropertyChangedEventArgs e) => Behavior.Action = Action;

        #endregion

        #region CommandParameter

        /// <summary>
        /// CommandParameter Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(BehaviorBinding),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnCommandParameterChanged)));

        /// <summary>
        /// Gets or sets the CommandParameter property.  
        /// </summary>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Handles changes to the CommandParameter property.
        /// </summary>
        private static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((BehaviorBinding)d).OnCommandParameterChanged(e);

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CommandParameter property.
        /// </summary>
        protected virtual void OnCommandParameterChanged(DependencyPropertyChangedEventArgs e) => Behavior.CommandParameter = CommandParameter;

        #endregion

        #region Event

        /// <summary>
        /// Event Dependency Property
        /// </summary>
        public static readonly DependencyProperty EventProperty =
            DependencyProperty.Register(nameof(Event), typeof(string), typeof(BehaviorBinding),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnEventChanged)));

        /// <summary>
        /// Gets or sets the Event property.  
        /// </summary>
        public string Event
        {
            get => (string)GetValue(EventProperty);
            set => SetValue(EventProperty, value);
        }

        /// <summary>
        /// Handles changes to the Event property.
        /// </summary>
        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((BehaviorBinding)d).OnEventChanged(e);

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Event property.
        /// </summary>
        protected virtual void OnEventChanged(DependencyPropertyChangedEventArgs e) => ResetBehavior();

        /// <summary>
        /// Resets the behavior.
        /// </summary>
        protected override void ResetBehavior()
        {

            if (Owner != null) //only do this when the Owner is set

            {

                //check if the Event is set. If yes we need to rebind the Command to the new event and unregister the old one
                if (Behavior.Event != null && Behavior.Owner != null)
                    Behavior.Dispose();

                //bind the new event to the command
                Behavior.BindEvent(Owner, Event);

            }

        }

        protected override Freezable CreateInstanceCore() => new BehaviorBinding();

        #endregion



    }
}
