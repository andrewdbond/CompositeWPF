// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcceptanceTestLibrary.Common;
using AcceptanceTestLibrary.UIAWrapper;
using AcceptanceTestLibrary.ApplicationHelper;
using System.Windows.Automation;
using BasicMVVMQuickstart.Tests.AcceptanceTest.TestEntities.Page;
using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BasicMVVMQuickstart.Tests.AcceptanceTest.TestEntities.Assertion
{
    public static class BasicMVVMQuickstartAssertion<TApp>
     where TApp : AppLauncherBase, new()
    {
        # region Desktop
        public static void AssertDesktopControlLoad()
        {
            Assert.IsNotNull(BasicMVVMQuickstartPage<TApp>.NameText, "Name Text Box Not loaded");
            Assert.IsNotNull(BasicMVVMQuickstartPage<TApp>.AgeText, "Age Text Box Not loaded");
            Assert.IsNotNull(BasicMVVMQuickstartPage<TApp>.Question1Text, "Quest Text Box Not loaded");
            Assert.IsNotNull(BasicMVVMQuickstartPage<TApp>.ColorsList, "Colors List Not loaded");
            Assert.IsNotNull(BasicMVVMQuickstartPage<TApp>.ResetButton, "Reset Button Not loaded");
            Assert.IsNotNull(BasicMVVMQuickstartPage<TApp>.SubmitButton, "Submit Button Not loaded");
        }

        public static void AssertProcessQuestionaireByClickingSubmit()
        {
            PopulateQuestionaireWithData();

            AutomationElementCollection aeColors = BasicMVVMQuickstartPage<TApp>.ColorsList;
            aeColors[0].Select();

            AutomationElement submitButton = BasicMVVMQuickstartPage<TApp>.SubmitButton;
            //check if the submit button is enabled
            Assert.IsTrue(submitButton.Current.IsEnabled, "Submit Button Disabled for valid request.");
            //click on the submit button
            submitButton.Click();

            Assert.IsTrue(true);
        }
        public static void AssertProcessQuestionaireByClickingReset()
        {
           PopulateQuestionaireWithData();

            AutomationElementCollection aeColors = BasicMVVMQuickstartPage<TApp>.ColorsList;
            aeColors[0].Select();

            AutomationElement resetButton = BasicMVVMQuickstartPage<TApp>.ResetButton;
            //check if the reset button is enabled
            Assert.IsTrue(resetButton.Current.IsEnabled, "Reset Button Disabled for valid request.");
            //click on the reset button
            resetButton.Click();
            Assert.AreEqual(string.Empty, BasicMVVMQuickstartPage<TApp>.NameText.GetValue());
            Assert.AreEqual("0", BasicMVVMQuickstartPage<TApp>.AgeText.GetValue());
            Assert.AreEqual(string.Empty, BasicMVVMQuickstartPage<TApp>.Question1Text.GetValue());
           
        }

        # endregion

        # region Desktop Private Methods
        private static void PopulateQuestionaireWithData()
        {
            BasicMVVMQuickstartPage<TApp>.NameText.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("DefaultName"));
            BasicMVVMQuickstartPage<TApp>.AgeText.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("DefaultAge"));
            BasicMVVMQuickstartPage<TApp>.Question1Text.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("DefaultQuestion1"));
        }

        # endregion
    }
}
