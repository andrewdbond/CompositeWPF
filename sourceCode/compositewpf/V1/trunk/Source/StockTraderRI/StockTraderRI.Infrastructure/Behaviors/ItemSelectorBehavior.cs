//===============================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace StockTraderRI.Infrastructure.Behaviors
{
    public class ItemSelectorBehavior : DependencyObject
    {
        #region TrackItemSelection (Attached Property)
        public static readonly DependencyProperty TrackItemSelectionProperty =
          DependencyProperty.RegisterAttached("TrackItemSelection", typeof(bool), typeof(ItemSelectorBehavior),
            new FrameworkPropertyMetadata(false, (FrameworkPropertyMetadataOptions.None),
            new PropertyChangedCallback(TrackItemSelectionPropertyChanged)));

        public static void SetTrackItemSelection(DependencyObject d, bool value)
        {
            d.SetValue(TrackItemSelectionProperty, value);
        }

        public static bool GetTrackItemSelection(DependencyObject d)
        {
            return (bool)d.GetValue(TrackItemSelectionProperty);
        }

        private static void TrackItemSelectionPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Selector selector = sender as Selector;
            if (selector == null) return;

            bool newValue = (bool)e.NewValue;
            bool oldValue = (bool)e.OldValue;


            if (newValue != false && oldValue == false)
            {
                selector.PreviewMouseDown += selector_PreviewMouseDown;
            }
            else if (newValue == false && oldValue != false)
            {
                selector.PreviewMouseDown -= selector_PreviewMouseDown;
            }
        }

        static void selector_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FindAndSelectItem(e.OriginalSource as DependencyObject);
        }

        /// <summary>
        /// Sets the item that contains the element that raised the event
        /// </summary>
        /// <param name="dependencyObject">The originator of the event</param>
        static void FindAndSelectItem(DependencyObject dependencyObject)
        {

            if (dependencyObject == null) return;

            Selector selector = ItemsControl.ItemsControlFromItemContainer(dependencyObject) as Selector;
            if (selector != null)
            {
                object item = selector.ItemContainerGenerator.ItemFromContainer(dependencyObject);
                if (item != null)
                {
                    selector.SelectedItem = item;

                    // if we are at the attached selector exit else set selector as new dependencyObject
                    // so we don't have to walk potential "buffer" elements
                    if ((bool)selector.GetValue(ItemSelectorBehavior.TrackItemSelectionProperty) == true)
                    {
                        return;
                    }
                    else
                    {
                        dependencyObject = selector;
                    }
                }
            }

            DependencyObject parent = GetParent(dependencyObject as FrameworkElement);

            if (parent == null)
            {
                return;
            }
            else
            {
                FindAndSelectItem(parent);
            }
        }

        /// <summary>
        /// Walks up the tree for the next parent using the least taxing ways first.
        /// </summary>
        /// <param name="frameworkElement">Framework element in the tree</param>
        /// <returns>The parent dependency object found or null</returns>
        static DependencyObject GetParent(FrameworkElement frameworkElement)
        {
            if (frameworkElement == null) return null;

            DependencyObject parent = frameworkElement.Parent;

            if (parent == null)
            {
                parent = frameworkElement.TemplatedParent;
            }

            if (parent == null)
            {
                parent = VisualTreeHelper.GetParent(frameworkElement);
            }

            if (parent == null)
            {
                parent = LogicalTreeHelper.GetParent(frameworkElement);
            }

            if (parent is Selector)
            {
                if ((bool)parent.GetValue(ItemSelectorBehavior.TrackItemSelectionProperty) == true)
                    parent = null;
            }

            return parent;
        }

        #endregion

    }
}
