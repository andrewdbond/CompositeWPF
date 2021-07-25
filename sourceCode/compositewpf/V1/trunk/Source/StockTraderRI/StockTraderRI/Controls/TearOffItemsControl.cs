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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockTraderRI.Controls
{
 
    /// <summary>
    /// This control provides the ability to extract out the contents into a seperate window for viewing
    /// </summary>
    [TemplatePart(Type=typeof(Path),Name="PART_HeaderButton")]
    public class TearOffItemsControl : ItemsControl
    {
        private Window tearOffWindow;

        static TearOffItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TearOffItemsControl), new FrameworkPropertyMetadata(typeof(TearOffItemsControl)));
        }

        #region HeaderText (Dependency Property)
        public static readonly DependencyProperty HeaderTextProperty =
          DependencyProperty.Register("HeaderText", typeof(string), typeof(TearOffItemsControl),
            new FrameworkPropertyMetadata(String.Empty,
            (FrameworkPropertyMetadataOptions.None)));

        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        #endregion 

        #region HeaderTextStyle (Dependency Property)
        public static readonly DependencyProperty HeaderTextStyleProperty =
          DependencyProperty.Register("HeaderTextStyle", typeof(Style), typeof(TearOffItemsControl),
            new FrameworkPropertyMetadata(null,
            (FrameworkPropertyMetadataOptions.None)));

        public Style HeaderTextStyle
        {
            get { return (Style)GetValue(HeaderTextStyleProperty); }
            set { SetValue(HeaderTextStyleProperty, value); }
        }

        #endregion 

        #region HeaderBackground (Dependency Property)
        public static readonly DependencyProperty HeaderBackgroundProperty =
          DependencyProperty.Register("HeaderBackground", typeof(Brush), typeof(TearOffItemsControl),
            new FrameworkPropertyMetadata(SystemColors.GrayTextBrush,
            (FrameworkPropertyMetadataOptions.None)));

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        #endregion 
        
        #region HeaderButtonBackground (Dependency Property)
        public static readonly DependencyProperty HeaderButtonBackgroundProperty =
          DependencyProperty.Register("HeaderButtonBackground", typeof(Brush), typeof(TearOffItemsControl),
            new FrameworkPropertyMetadata(SystemColors.GrayTextBrush,
            (FrameworkPropertyMetadataOptions.None)));

        public Brush HeaderButtonBackground
        {
            get { return (Brush)GetValue(HeaderButtonBackgroundProperty); }
            set { SetValue(HeaderButtonBackgroundProperty, value); }
        }

        #endregion 
        
        #region HeaderButtonRollOverBackground (Dependency Property)
        public static readonly DependencyProperty HeaderButtonRollOverBackgroundProperty =
          DependencyProperty.Register("HeaderButtonRollOverBackground", typeof(Brush), typeof(TearOffItemsControl),
            new FrameworkPropertyMetadata(SystemColors.HighlightBrush,
            (FrameworkPropertyMetadataOptions.None)));

        public Brush HeaderButtonRollOverBackground
        {
            get { return (Brush)GetValue(HeaderButtonRollOverBackgroundProperty); }
            set { SetValue(HeaderButtonRollOverBackgroundProperty, value); }
        }

 
        #endregion 

        #region IsCollapsed (Dependency Property)
        public static readonly DependencyProperty IsCollapsedProperty =
          DependencyProperty.Register("IsCollapsed", typeof(bool), typeof(TearOffItemsControl),
            new FrameworkPropertyMetadata(false,
            (FrameworkPropertyMetadataOptions.None)));

        public bool IsCollapsed
        {
            get { return (bool)GetValue(IsCollapsedProperty); }
            set { SetValue(IsCollapsedProperty, value); }
        }

        #endregion 

        #region WindowWidth (Dependency Property)
        public static readonly DependencyProperty WindowWidthProperty =
          DependencyProperty.Register("WindowWidth", typeof(double), typeof(TearOffItemsControl),
            new FrameworkPropertyMetadata(Double.NaN,
            (FrameworkPropertyMetadataOptions.None)));

        public double WindowWidth
        {
            get { return (double)GetValue(WindowWidthProperty); }
            set { SetValue(WindowWidthProperty, value); }
        }

        #endregion 

        #region WindowHeight (Dependency Property)
        public static readonly DependencyProperty WindowHeightProperty =
          DependencyProperty.Register("WindowHeight", typeof(double), typeof(TearOffItemsControl),
            new FrameworkPropertyMetadata(Double.NaN,
            (FrameworkPropertyMetadataOptions.None)));

        public double WindowHeight
        {
            get { return (double)GetValue(WindowHeightProperty); }
            set { SetValue(WindowHeightProperty, value); }
        }

        #endregion 


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Path headerButton = GetTemplateChild("PART_HeaderButton") as Path;
            HeaderButtonApplyTemplate(headerButton);
        }

        protected void HeaderButtonApplyTemplate(Path button)
        {
            if(button != null)
            {
                button.MouseLeftButtonDown += button_MouseLeftButtonDown;
            }
        }
        
     
        #region Event Processing
        void button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CreateTearOffWindow();

        }

        void CreateTearOffWindow()
        {
            tearOffWindow = new Window();
            tearOffWindow.Title = HeaderText;
            tearOffWindow.Width = WindowWidth;
            tearOffWindow.Height = WindowHeight;
            tearOffWindow.Topmost = true;
            ItemsControl itemsControl = new ItemsControl();
            IEnumerable itemsSource = this.ItemsSource;
            this.ItemsSource = null;
            itemsControl.ItemsSource = itemsSource;

            //ArrayList viewList = new ArrayList();
            //foreach (var item in this.Items)
            //{
            //    viewList.Add(item);
            //}

            //this.Items.Clear();

            //foreach (var view in viewList)
            //{
            //    itemsControl.Items.Add(view);
            //}

            tearOffWindow.Content = itemsControl;

            IsCollapsed = true;

            tearOffWindow.Closing += tearOffWindow_Closing;
            tearOffWindow.Show();
        }

        void tearOffWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ItemsControl itemsControl = tearOffWindow.Content as ItemsControl;
            if(itemsControl != null)
            {
                IEnumerable itemsSource = itemsControl.ItemsSource;
                itemsControl.ItemsSource = null;
                this.ItemsSource = itemsSource;
                
                //ArrayList viewList = new ArrayList();
                //foreach (var item in itemsControl.Items)
                //{
                //    viewList.Add(item);
                //}

                //itemsControl.Items.Clear();

                //foreach (var view in viewList)
                //{
                //    this.Items.Add(view);
                //}
            }

            tearOffWindow.Closing -= tearOffWindow_Closing;
            tearOffWindow = null;

            IsCollapsed = false;

        }
        #endregion


    }
}
