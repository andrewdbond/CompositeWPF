//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Presentation.Properties;
using Microsoft.Practices.Composite.Presentation.Regions.Behaviors;
using Microsoft.Practices.Composite.Regions;

namespace Microsoft.Practices.Composite.Presentation.Regions
{
    /// <summary>
    /// Adapter that creates a new <see cref="Region"/> and binds all
    /// the views to the adapted <see cref="TabControl"/>.
    /// </summary>
    /// <remarks>
    /// This adapter is needed on Silverlight because the <see cref="TabControl"/> doesn't 
    /// automatically create <see cref="TabItem"/>s when new items are added to 
    /// the <see cref="ItemsControl.Items"/> collection.
    /// </remarks>
    public class TabControlRegionAdapter : RegionAdapterBase<TabControl>
    {
        /// <summary>
        /// <see cref="Style"/> to set to the created <see cref="TabItem"/>.
        /// </summary>
        public static readonly DependencyProperty ItemContainerStyleProperty =
            DependencyProperty.RegisterAttached("ItemContainerStyle", typeof(Style), typeof(TabControlRegionAdapter), null);

        /// <summary>
        /// Initializes a new instance of the <see cref="TabControlRegionAdapter"/> class.
        /// </summary>
        /// <param name="regionBehaviorFactory">The factory used to create the region behaviors to attach to the created regions.</param>
        public TabControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {
        }

        /// <summary>
        /// Gets the <see cref="ItemContainerStyleProperty"/> property value.
        /// </summary>
        /// <param name="target">Target object of the attached property.</param>
        /// <returns>Value of the <see cref="ItemContainerStyleProperty"/> property.</returns>
        public static Style GetItemContainerStyle(DependencyObject target)
        {
            return (Style)target.GetValue(ItemContainerStyleProperty);
        }

        /// <summary>
        /// Sets the <see cref="ItemContainerStyleProperty"/> property value.
        /// </summary>
        /// <param name="target">Target object of the attached property.</param>
        /// <param name="value">Value to be set on the <see cref="ItemContainerStyleProperty"/> property.</param>
        public static void SetItemContainerStyle(DependencyObject target, Style value)
        {
            target.SetValue(ItemContainerStyleProperty, value);
        }

        /// <summary>
        /// Adapts a <see cref="TabControl"/> to an <see cref="IRegion"/>.
        /// </summary>
        /// <param name="region">The new region being used.</param>
        /// <param name="regionTarget">The object to adapt.</param>
        protected override void Adapt(IRegion region, TabControl regionTarget)
        {
            bool itemsSourceIsSet = regionTarget.ItemsSource != null;

            if (itemsSourceIsSet)
            {
                throw new InvalidOperationException(Resources.ItemsControlHasItemsSourceException);
            }
        }

        /// <summary>
        /// Attach new behaviors.
        /// </summary>
        /// <param name="region">The region being used.</param>
        /// <param name="regionTarget">The object to adapt.</param>
        /// <remarks>
        /// This class attaches the base behaviors and also keeps the <see cref="TabControl.SelectedItem"/> 
        /// and the <see cref="IRegion.ActiveViews"/> in sync.
        /// </remarks>
        protected override void AttachBehaviors(IRegion region, TabControl regionTarget)
        {
            base.AttachBehaviors(region, regionTarget);
            region.Behaviors.Add(TabControlRegionSyncBehavior.BehaviorKey, new TabControlRegionSyncBehavior { HostControl = regionTarget });
        }

        /// <summary>
        /// Creates a new instance of <see cref="Region"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="Region"/>.</returns>
        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }

        /// <summary>
        /// Behavior that generates <see cref="TabItem"/> containers for the added items
        /// and also keeps the <see cref="TabControl.SelectedItem"/> and the <see cref="IRegion.ActiveViews"/> in sync.
        /// </summary>
        private class TabControlRegionSyncBehavior : RegionBehavior, IHostAwareRegionBehavior
        {
            public const string BehaviorKey = "TabControlRegionSyncBehavior";
            
            private static readonly DependencyProperty IsGeneratedProperty =
                DependencyProperty.RegisterAttached("IsGenerated", typeof(bool), typeof(TabControlRegionSyncBehavior), null);
            
            private TabControl hostControl;

            public DependencyObject HostControl
            {
                get
                {
                    return this.hostControl;
                }

                set
                {
                    TabControl newValue = value as TabControl;
                    if (newValue == null)
                    {
                        throw new InvalidOperationException(Resources.HostControlMustBeATabControl);
                    }

                    if (IsAttached)
                    {
                        throw new InvalidOperationException(Resources.HostControlCannotBeSetAfterAttach);
                    }

                    this.hostControl = newValue;
                }
            }

            protected override void OnAttach()
            {
                if (this.hostControl == null)
                {
                    throw new InvalidOperationException(Resources.HostControlCannotBeNull);
                }

                this.SynchronizeItems();

                this.hostControl.SelectionChanged += this.OnSelectionChanged;
                this.Region.ActiveViews.CollectionChanged += this.OnActiveViewsChanged;
                this.Region.Views.CollectionChanged += this.OnViewsChanged;
            }

            private static object GetContainedItem(TabItem tabItem)
            {
                if ((bool)tabItem.GetValue(IsGeneratedProperty))
                {
                    return tabItem.Content;
                }

                return tabItem;
            }

            private static TabItem PrepareContainerForItem(object item, DependencyObject parent)
            {
                TabItem container = item as TabItem;
                if (container == null)
                {
                    container = new TabItem();
                    container.Content = item;
                    container.DataContext = GetDataContext(item);
                    container.Style = GetItemContainerStyle(parent);
                    container.SetValue(IsGeneratedProperty, true);
                }

                return container;
            }

            private static void ClearContainerForItem(TabItem tabItem)
            {
                if ((bool)tabItem.GetValue(IsGeneratedProperty))
                {
                    tabItem.Content = null;
                }
            }

            /// <summary>
            /// Return the appropriate data context.  If the item is a FrameworkElement it cannot be a data context in Silverlight, so we use its data context.
            /// Otherwise, we just us the item as the data context.
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            private static object GetDataContext(object item)
            {
                FrameworkElement frameworkElement = item as FrameworkElement;
                return frameworkElement == null ? item : frameworkElement.DataContext;
            }

            private static TabItem GetContainerForItem(object item, ItemCollection itemCollection)
            {
                TabItem container = item as TabItem;
                if (container != null && ((bool)container.GetValue(IsGeneratedProperty)) == false)
                {
                    return container;
                }

                foreach (TabItem tabItem in itemCollection)
                {
                    if ((bool)tabItem.GetValue(IsGeneratedProperty))
                    {
                        if (tabItem.Content == item)
                        {
                            return tabItem;
                        }
                    }
                }

                return null;
            }

            private void SynchronizeItems()
            {
                List<object> existingItems = new List<object>();
                if (this.hostControl.Items.Count > 0)
                {
                    // Control must be empty before "Binding" to a region
                    foreach (object childItem in this.hostControl.Items)
                    {
                        existingItems.Add(childItem);
                    }
                }

                foreach (object view in this.Region.Views)
                {
                    TabItem tabItem = PrepareContainerForItem(view, this.hostControl);
                    this.hostControl.Items.Add(tabItem);
                }

                foreach (object existingItem in existingItems)
                {
                    this.Region.Add(existingItem);
                }
            }

            private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                // e.OriginalSource == null, that's why we use sender.
                if (this.hostControl == sender)
                {
                    foreach (TabItem tabItem in e.RemovedItems)
                    {
                        object item = GetContainedItem(tabItem);

                        // check if the view is in both Views and ActiveViews collections (there may be out of sync)
                        if (this.Region.Views.Contains(item) && this.Region.ActiveViews.Contains(item))
                        {
                            this.Region.Deactivate(item);
                        }
                    }

                    foreach (TabItem tabItem in e.AddedItems)
                    {
                        object item = GetContainedItem(tabItem);
                        if (!this.Region.ActiveViews.Contains(item))
                        {
                            this.Region.Activate(item);
                        }
                    }
                }
            }

            private void OnActiveViewsChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    this.hostControl.SelectedItem = GetContainerForItem(e.NewItems[0], this.hostControl.Items);
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove
                    && this.hostControl.SelectedItem != null
                    && e.OldItems.Contains(GetContainedItem((TabItem)this.hostControl.SelectedItem)))
                {
                    this.hostControl.SelectedItem = null;
                }
            }

            private void OnViewsChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (object newItem in e.NewItems)
                    {
                        TabItem tabItem = PrepareContainerForItem(newItem, this.hostControl);
                        this.hostControl.Items.Add(tabItem);
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (object oldItem in e.OldItems)
                    {
                        TabItem tabItem = GetContainerForItem(oldItem, this.hostControl.Items);
                        this.hostControl.Items.Remove(tabItem);
                        ClearContainerForItem(tabItem);
                    }
                }
            }
        }
    }
}
