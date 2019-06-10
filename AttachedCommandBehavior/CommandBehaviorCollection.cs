/* Source: https://dzone.com/articles/how-get-eventargs ; Author of original code: Marlon Grech */

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace AttachedCommandBehavior
{
    public class CommandBehaviorCollection
    {

        private const string BehaviorsInternal = "BehaviorsInternal";
        private const string StyleBehaviors = "StyleBehaviors";

        #region Behaviors

        /// <summary>
        /// Behaviors Read-Only Dependency Property
        /// As you can see the Attached readonly property has a name registered different (BehaviorsInternal) than the property name, this is a tricks os that we can construct the collection as we want
        /// Read more about this here http://wekempf.spaces.live.com/blog/cns!D18C3EC06EA971CF!468.entry
        /// </summary>
        private static readonly DependencyPropertyKey BehaviorsPropertyKey
            = DependencyProperty.RegisterAttachedReadOnly(BehaviorsInternal, typeof(BehaviorBindingCollection), typeof(CommandBehaviorCollection),
                new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty BehaviorsProperty
            = BehaviorsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the Behaviors property.  
        /// Here we initialze the collection and set the Owner property
        /// </summary>
        public static BehaviorBindingCollection GetBehaviors(DependencyObject d)
        {
            if (d == null)
                throw new InvalidOperationException("The dependency object trying to attach to is set to null");

            if (!(d.GetValue(BehaviorsProperty) is BehaviorBindingCollection collection))
            {
                collection = new BehaviorBindingCollection
                {
                    Owner = d
                };
                SetBehaviors(d, collection);
            }
            return collection;
        }

        /// <summary>
        /// Provides a secure method for setting the Behaviors property.  
        /// This dependency property indicates ....
        /// </summary>
        private static void SetBehaviors(DependencyObject d, BehaviorBindingCollection value)
        {
            d.SetValue(BehaviorsPropertyKey, value);

            ((INotifyCollectionChanged)value).CollectionChanged += CollectionChanged;
        }

        static int GetId(BehaviorBindingCollection sourceCollection) => sourceCollection.Count == 1 ? 1 : sourceCollection[sourceCollection.Count - 1].Id + 1;

        static void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            BehaviorBindingCollection sourceCollection = (BehaviorBindingCollection)sender;
            switch (e.Action)
            {
                //when an item(s) is added we need to set the Owner property implicitly
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                        foreach (Behavior item in e.NewItems)
                        {
                            item.Owner = sourceCollection.Owner;
                            item.Id = GetId(sourceCollection);
                        }
                    break;

                //when an item(s) is removed we should Dispose the BehaviorBinding
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:
                    if (e.OldItems != null)

                        TryRemoveStyleBehaviors(GetStyleBehaviors(sourceCollection.Owner), e.OldItems, true);

                    break;

                //here we have to set the owner property to the new item and unregister the old item
                case NotifyCollectionChangedAction.Replace:

                    TryReplaceStyleBehaviors(GetStyleBehaviors(sourceCollection.Owner), e.OldItems, e.NewItems, true);

                    break;

                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex == e.NewStartingIndex) break;
                    int difference = e.OldStartingIndex - e.NewStartingIndex;
                    int id;
                    FreezableCollection<Behavior> styleBehaviors = GetStyleBehaviors(sourceCollection.Owner);
                    foreach (Behavior item in e.OldItems)
                    {
                        id = item.Id;
                        item.Id -= difference;
                        foreach (Behavior styleItem in styleBehaviors)
                            if (styleItem.Id == id)
                                styleItem.Id = item.Id;
                    }
                    void updateId(int startIndex, int length)
                    {
                        int count = length + startIndex;
                        for (int i = startIndex; i < count; i++)
                        {
                            id = sourceCollection[startIndex].Id;
                            sourceCollection[startIndex].Id += startIndex + 1;
                            foreach (Behavior styleItem in styleBehaviors)
                                if (styleItem.Id == id)
                                    styleItem.Id = sourceCollection[startIndex].Id;
                        }
                    }
                    int _startIndex;
                    if (e.NewStartingIndex < e.OldStartingIndex)
                    {
                        _startIndex = e.NewStartingIndex + e.OldItems.Count;
                        updateId(_startIndex, e.OldStartingIndex + e.OldItems.Count - _startIndex + 1);
                    }
                    else
                    {
                        _startIndex = e.OldStartingIndex;
                        updateId(_startIndex, e.NewStartingIndex - _startIndex + 1);
                    }
                    break;
                default:
                    break;
            }
        }

        public static DependencyProperty StyleBehaviorsProperty = DependencyProperty.RegisterAttached(StyleBehaviors, typeof(BehaviorBindingCollection), typeof(CommandBehaviorCollection), new FrameworkPropertyMetadata(null, StyleBehaviorsChanged));

        public static BehaviorBindingCollection GetStyleBehaviors(DependencyObject obj) => (BehaviorBindingCollection)obj.GetValue(StyleBehaviorsProperty);

        public static void SetStyleBehaviors(DependencyObject obj, BehaviorBindingCollection value) => obj.SetValue(StyleBehaviorsProperty, value);

        private static void StyleBehaviorsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)

        {

            if (obj != null)

            {

                if (e.OldValue != null)

                {

                    ((INotifyCollectionChanged)e.OldValue).CollectionChanged -= StyleCollectionChanged;

                    TryRemoveStyleBehaviors(GetBehaviors(obj), e.OldValue as FreezableCollection<Behavior>, false);

                }

                if (e.NewValue != null)

                {

                    ((INotifyCollectionChanged)e.NewValue).CollectionChanged += StyleCollectionChanged;

                    AddStyleBehaviors(GetBehaviors(obj), e.NewValue as FreezableCollection<Behavior>);

                }

            }

        }

        private static void AddStyleBehaviors(BehaviorBindingCollection behaviors, IList behaviorsToAdd)

        {

            foreach (Behavior behavior in behaviorsToAdd)

            {

                Behavior _behavior = behavior.Clone() as Behavior;

                behaviors.Add(_behavior as Behavior);

                behavior.Id = _behavior.Id;

            }

        }

        private static void TryRemoveStyleBehaviors(BehaviorBindingCollection behaviors, IList behaviorsToRemove, bool disposeBehaviors)

        {

            if (disposeBehaviors)

                foreach (BehaviorBinding behavior in behaviorsToRemove.OfType<BehaviorBinding>())

                    behavior.Behavior.Dispose();

            if (behaviors != null)

                for (int i = 0; i < behaviorsToRemove.Count; i++)

                    for (int j = 0; j < behaviors.Count; j++)

                        if (((Behavior)behaviorsToRemove[i]).Id == behaviors[j].Id)

                            // We don't need to dispose the behaviors from the 'behaviors' colelction parameter here, because they will be disposed automatically by the CollectionChanged event handler

                            behaviors.RemoveAt(j);

        }

        private static void TryReplaceStyleBehaviors(BehaviorBindingCollection behaviors, IList oldBehaviors, IList newBehaviors, bool disposeBehaviors)

        {

            for (int i = 0; i < oldBehaviors.Count; i++)
            {
                Behavior oldItem = (Behavior)oldBehaviors[i];
                Behavior newItem = (Behavior)newBehaviors[i];
                Behavior clonedBehavior = newItem.Clone() as Behavior;
                newItem.Owner = clonedBehavior.Owner = behaviors.Owner;
                newItem.Id = oldItem.Id;
                if (disposeBehaviors)
                    (oldItem as BehaviorBinding)?.Behavior.Dispose();
                for (int j = 0; j < behaviors.Count; j++)
                    if (behaviors[j].Id == oldItem.Id)
                        behaviors[j] = clonedBehavior;
            }

        }

        static void StyleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            BehaviorBindingCollection sourceCollection = (BehaviorBindingCollection)sender;
            switch (e.Action)
            {
                //when an item(s) is added we need to set the Owner property implicitly
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)

                        AddStyleBehaviors(GetBehaviors(sourceCollection.Owner), e.NewItems);

                    break;

                //when an item(s) is removed we should Dispose the BehaviorBinding
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:
                    if (e.OldItems != null)

                        TryRemoveStyleBehaviors(GetBehaviors(sourceCollection.Owner), e.OldItems, false);

                    break;

                //here we have to set the owner property to the new item and unregister the old item
                case NotifyCollectionChangedAction.Replace:

                    TryReplaceStyleBehaviors(GetBehaviors(sourceCollection.Owner), e.OldItems, e.NewItems, false);

                    break;

                case NotifyCollectionChangedAction.Move:

                    throw new InvalidOperationException("Move is not supported for style behaviors");

                default:
                    break;
            }
        }

        #endregion

    }

    /// <summary>
    /// Collection to store the list of behaviors. This is done so that you can intiniate it from XAML
    /// This inherits from freezable so that it gets inheritance context for DataBinding to work
    /// </summary>
    public class BehaviorBindingCollection : FreezableCollection<Behavior>
    {
        /// <summary>
        /// Gets or sets the Owner of the binding
        /// </summary>
        public DependencyObject Owner { get; set; }
    }
}