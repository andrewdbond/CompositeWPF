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

namespace EventAggregation.Tests.AcceptanceTest.TestEntities.Page
{
    public static class EventAggregationPage<TApp>
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

        public static WPFComboBox CustomerComboBox
        {
            get { return DesktopWindow.Get<WPFComboBox>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("CustomerCombo")); }
        }

        public static WPFComboBox FundComboBox
        {
            get { return DesktopWindow.Get<WPFComboBox>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("FundCombo")); }
        }

        public static Button AddButton
        {
            get { return DesktopWindow.Get<Button>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("AddButton")); }
        }

        public static Label ActivityLabel
        {
            get { return DesktopWindow.Get<Label>(new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("ActivityLabel")); }
        }

        public static Label GetFundsLabel(string fundsText)
        {
            return (Label)DesktopWindow.Get(SearchCriteria.ByText(fundsText)
                                                             .AndControlType(typeof(Label)));
        }

        public static Label GetFundsLabelByAutomationId(string fundsText, string automationId)
        {
            return (Label)window.Get(SearchCriteria.ByAutomationId(automationId).
                        AndByText(fundsText).AndControlType(typeof(Label)));
        }

        public static List<AutomationElement> ElementsInActivityView
        {
            get { return DesktopWindow.AutomationElement.FindSiblingsInTreeByName(GetDataFromResourceFile("ActivityView")); }
        }

        public static void DisposeApplication()
        {
            if (app != null)
            {
                app.Kill();
            }
        }


        private static string GetDataFromResourceFile(string keyName)
        {
            return new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue(keyName);
        }

        #endregion

        #region Silverlight
        public static AutomationElement Window
        {
            get { return PageBase<TApp>.Window; }
            set { PageBase<TApp>.Window = value; }
        }

        public static AutomationElement CustomerCombo
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("CustomerCombo"))); } 
        }

        public static AutomationElement FundCombo
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("FundCombo"))); }
        }

        public static AutomationElement AddFundButton
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("AddButton"))); }
        }

        public static AutomationElement ActivityLabelElement
        {
            get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("ActivityLabel"))); }
        }

        public static AutomationElementCollection AllTextBoxes
        {
            get
            {
                PropertyCondition findText = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text);
                return Window.FindAll(TreeScope.Descendants, findText);
            }
        }

        #endregion
    }
}
