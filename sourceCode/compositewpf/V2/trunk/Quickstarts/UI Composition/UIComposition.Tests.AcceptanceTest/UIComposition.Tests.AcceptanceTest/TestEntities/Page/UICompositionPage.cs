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
using AcceptanceTestLibrary.TestEntityBase;
using Core;
using Core.UIItems.WindowItems;
using AcceptanceTestLibrary.ApplicationHelper;
using Core.UIItems.ListBoxItems;
using Core.UIItems.TabItems;
using Core.UIItems;
using UIComposition.Tests.AcceptanceTest.TestInfrastructure;
using Core.UIItems.Finders;
using System.Windows.Automation;

namespace UIComposition.Tests.AcceptanceTest.TestEntities.Page
{
    public static class UICompositionPage<TApp>
        where TApp : AppLauncherBase, new()
    {
        #region Silverlight
        public static AutomationElement Window
        {
            get { return PageBase<TApp>.Window; }
            set { PageBase<TApp>.Window = value; }
        }

        public static AutomationElement GarageImage
        {
            get { return PageBase<TApp>.FindControlByAutomationId("GarageImgAutomation"); }
        }

        public static AutomationElement EmployeesListGrid
        {
            get { return PageBase<TApp>.FindControlByAutomationId("EmployeesList"); }
        }
        public static AutomationElement EmployeeDetailsTabControl
        {
            get { return PageBase<TApp>.FindControlByAutomationId("DetailsTabControl"); }
        }
        public static AutomationElement SilverLightFirstNameTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("FirstNameText"); }
        }

        public static AutomationElement SilverLightPhoneTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("PhoneText"); }
        }

        public static AutomationElement SilverLightLastNameTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("LastNameText"); }
        }

        public static AutomationElement SilverLightEmailTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("EmailText"); }
        }

        public static AutomationElement SilverLightProjectsGrid
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ProjectsList"); }
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

        public static ListView EmployeesList
        {
            get { return DesktopWindow.Get<ListView>(TestDataInfrastructure.GetControlId("EmployeesList")); }
        }

        public static Tab EmployeeDetailsTab
        {
            get { return DesktopWindow.Get<Tab>(TestDataInfrastructure.GetControlId("DetailsTabControl")); }
        }

        public static Label FirstNameLabel
        {
            get
            {
                SearchCriteria searchCriteria = SearchCriteria.ByText(TestDataInfrastructure.GetTestInputData("FirstNameLabelText")).AndControlType(typeof(Label));
                return DesktopWindow.Get<Label>(searchCriteria);
            }
        }

        public static Label LastNameLabel
        {
            get
            {
                SearchCriteria searchCriteria = SearchCriteria.ByText(TestDataInfrastructure.GetTestInputData("LastNameLabelText")).AndControlType(typeof(Label));
                return DesktopWindow.Get<Label>(searchCriteria);
            }
        }

        public static Label PhoneLabel
        {
            get
            {
                SearchCriteria searchCriteria = SearchCriteria.ByText(TestDataInfrastructure.GetTestInputData("PhoneLabelText")).AndControlType(typeof(Label));
                return DesktopWindow.Get<Label>(searchCriteria);
            }
        }

        public static Label EmailLabel
        {
            get
            {
                SearchCriteria searchCriteria = SearchCriteria.ByText(TestDataInfrastructure.GetTestInputData("EmailLabelText")).AndControlType(typeof(Label));
                return DesktopWindow.Get<Label>(searchCriteria);
            }
        }

        public static TextBox FirstNameTextbox
        {
            get { return DesktopWindow.Get<TextBox>(TestDataInfrastructure.GetControlId("FirstNameTextBox")); }
        }

        public static TextBox LastNameTextBox
        {
            get { return DesktopWindow.Get<TextBox>(TestDataInfrastructure.GetControlId("LastNameTextBox")); }
        }
        public static TextBox PhoneTextBox
        {
            get { return DesktopWindow.Get<TextBox>(TestDataInfrastructure.GetControlId("PhoneTextBox")); }
        }
        public static TextBox EmailTextBox
        {
            get { return DesktopWindow.Get<TextBox>(TestDataInfrastructure.GetControlId("EmailTextBox")); }
        }

        public static Label ProjectsLabel
        {
            get
            {
                SearchCriteria searchCriteria = SearchCriteria.ByText(TestDataInfrastructure.GetTestInputData("PartOfFollowingProjectsLabel")).AndControlType(typeof(Label));
                return DesktopWindow.Get<Label>(searchCriteria);
            }
        }

        public static ListView ProjectsList
        {
            get
            {
                return DesktopWindow.Get<ListView>(TestDataInfrastructure.GetControlId("CurrentProjectsList"));
            }
        }
        //public static List<AutomationElement> ElementsInActivityView
        //{
        //    get { return DesktopWindow.AutomationElement.FindSiblingsInTreeByName(GetDataFromResourceFile("ActivityView")); }
        //}

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
    }
}
