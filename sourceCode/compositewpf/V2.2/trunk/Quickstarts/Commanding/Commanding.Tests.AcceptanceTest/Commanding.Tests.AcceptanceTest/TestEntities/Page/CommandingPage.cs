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
using System.Linq;
using System.Text;
using AcceptanceTestLibrary.Common;
using System.Windows.Automation;
using AcceptanceTestLibrary.TestEntityBase;
using Core.UIItems.WindowItems;
using Core;
using Core.UIItems.ListBoxItems;
using Core.UIItems;
using Core.UIItems.Finders;
using AcceptanceTestLibrary.ApplicationHelper;

namespace Commanding.Tests.AcceptanceTest.TestEntities.Page
{
    public static class CommandingPage<TApp>
        where TApp : AppLauncherBase, new()
    {
        #region Desktop
        private static Application app;
        public static void LaunchApplication(string applicationPath, string windowTitle)
        {
            try
            {
                app = Application.Launch(applicationPath);
                DesktopWindow = app.GetWindow(windowTitle, Core.Factory.InitializeOption.NoCache);
            }
            catch (Exception)
            {
                DisposeApplication();
            }
        }

        private static Window window;

        public static Window DesktopWindow
        {
            get { return window; }
            set { window = value; }
        }        
               
        public static Button SaveAllToolBarButton
        {
            get { return DesktopWindow.Get<Button>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("SaveAllToolBarButton")); }
        }
        public static Button SaveButton
        {
            get { return DesktopWindow.Get<Button>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("SaveButton")); }
        }
        public static ListBox OrderListView
        {
            get { return DesktopWindow.Get<ListBox>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("OrderListView")); }
        }
        public static Label DateLabel
        {
            get { return DesktopWindow.Get<Label>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("DateLabel")); }
        }
        public static Label OrderNameLabel
        {
            get { return DesktopWindow.Get<Label>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("OrderNameLabel")); }
        }
        public static Label PriceLabel
        {
            get { return DesktopWindow.Get<Label>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("PriceLabel")); }
        }
        public static Label QuantityLabel
        {
            get { return DesktopWindow.Get<Label>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("QuantityLabel")); }
        }
        public static Label ShippingLabel
        {
            get { return DesktopWindow.Get<Label>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("ShippingLabel")); }
        }
        public static Label TotalLabel
        {
            get { return DesktopWindow.Get<Label>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("TotalLabel")); }
        }
        public static TextBox DeliveryDateTextBox
        {
            get { return DesktopWindow.Get<TextBox>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("DeliveryDateTextBox"));}
        }
        public static TextBox PriceTextBox
        {
            get { return DesktopWindow.Get<TextBox>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("PriceTextBox"));}
        }
        public static TextBox QuantityTextBox
        {
            get { return DesktopWindow.Get<TextBox>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("QuantityTextBox"));}
        }
        public static TextBox ShippingTextBox
        {
            get { return DesktopWindow.Get<TextBox>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("ShippingTextBox"));}
        }
        public static TextBox TotalTextBox
        {
            get { return DesktopWindow.Get<TextBox>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("TotalTextBox"));}
        }
        
        public static void DisposeApplication()
        {
            if (app != null)
            {
                app.Kill();
            }
        }
        #endregion

        #region Silverlight
        public static AutomationElement Window
        {
            get { return PageBase<TApp>.Window; }
            set { PageBase<TApp>.Window = value; }
        }

        public static AutomationElement aeSaveAllToolBarButton
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("SaveAllToolBarButton"))); }
        }
        public static AutomationElement aeSaveButton
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("SaveButton"))); }
        }
        public static AutomationElement aeOrderListView
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("OrderListView"))); }
        }

        public static AutomationElement aeDateLabel
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("DateLabel"))); }
        }

        public static AutomationElement aeOrderNameLabel
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("OrderNameLabel"))); }
        }

        public static AutomationElement aePriceLabel
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("PriceLabel"))); }
        }

        public static AutomationElement aeQuantityLabel
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("QuantityLabel"))); }
        }

        public static AutomationElement aeShippingLabel
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("ShippingLabel"))); }
        }

        public static AutomationElement aeTotalLabel
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("TotalLabel"))); }
        }

        public static AutomationElement aeDeliveryDateTextBox
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("DeliveryDateTextBox"))); }
        }

        public static AutomationElement aePriceTextBox
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("PriceTextBox"))); }
        }

        public static AutomationElement aeQuantityTextBox
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("QuantityTextBox"))); }
        }

        public static AutomationElement aeShippingTextBox
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("ShippingTextBox"))); }
        }

        public static AutomationElement aeTotalTextBox
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("TotalTextBox"))); }
        }
        
        private static string GetDataFromResourceFile(string keyName)
        {
            return new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue(keyName);
        }
        #endregion 
    }
}
