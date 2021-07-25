// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcceptanceTestLibrary.TestEntityBase;
using AcceptanceTestLibrary.ApplicationHelper;
using AcceptanceTestLibrary.Common;
using System.Windows.Automation;

namespace BasicMVVMQuickstart.Tests.AcceptanceTest.TestEntities.Page
{

    public static class BasicMVVMQuickstartPage<TApp>
        where TApp : AppLauncherBase, new()
    {
        #region Desktop
        public static AutomationElement Window
        {
            get { return PageBase<TApp>.Window; }
            set { PageBase<TApp>.Window = value; }
        }

        public static AutomationElement ResetButton
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ResetButton"); }
        }

        public static AutomationElement SubmitButton
        {
            get { return PageBase<TApp>.FindControlByAutomationId("SubmitButton"); }
        }

        public static AutomationElement NameText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("NameText"); }
        }

        public static AutomationElement AgeText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("AgeText"); }
        }

        public static AutomationElement Question1Text
        {
            get { return PageBase<TApp>.FindControlByAutomationId("Question1Text"); }
        }

        public static AutomationElementCollection ColorsList
        {
            get { return PageBase<TApp>.FindControlByType("ColorsList"); }
        }

        #endregion

    }
}
