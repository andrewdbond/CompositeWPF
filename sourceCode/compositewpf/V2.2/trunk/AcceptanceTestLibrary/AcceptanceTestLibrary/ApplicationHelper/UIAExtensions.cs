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
using System.Windows.Automation;
using AcceptanceTestLibrary.Common.Desktop;
using AcceptanceTestLibrary.Common.Silverlight;
using AcceptanceTestLibrary.Common;

namespace AcceptanceTestLibrary.ApplicationHelper
{
    public static class UIAExtensions
    {
        private static List<AutomationElement> automationElementList = new List<AutomationElement>();
        #region Find Element Methods

        public static AutomationElement FindElementById(this AutomationElement ae, string automationId)
        {
            if (ae == null || String.IsNullOrEmpty(automationId))
            {
                throw new InvalidOperationException("invalid operation");
            }

            // Set a property condition that will be used to find the control.
            Condition c = new PropertyCondition(
                AutomationElement.AutomationIdProperty,
                automationId,
                PropertyConditionFlags.IgnoreCase);

            // Find the element.
            TreeWalker tw = new TreeWalker(c);
            return tw.GetFirstChild(ae);
        }

        public static AutomationElement FindElementByContent(this AutomationElement ae, string content)
        {
            if (ae == null || String.IsNullOrEmpty(content))
            {
                throw new InvalidOperationException("invalid operation");
            }

            // Set a property condition that will be used to find the control.
            Condition c = new PropertyCondition(
                AutomationElement.NameProperty,
                content);

            // Find the element.
            TreeWalker tw = new TreeWalker(c);
            return tw.GetFirstChild(ae);
        }


        public static List<AutomationElement> FindSiblingsInTreeByName(this AutomationElement rootElement, string name)
        {
            // Clear the automation element list.
            automationElementList.Clear();
            AutomationElement parentElement = rootElement;

            WalkThroughRawTreeAndPopulateAEList(parentElement, name);

            return automationElementList;
        }

        public static AutomationElement SearchInRawTreeByName(this AutomationElement rootElement, string name)
        {
            AutomationElement elementNode = TreeWalker.RawViewWalker.GetFirstChild(rootElement);

            while (elementNode != null)
            {
                if (name.Equals(elementNode.Current.Name, StringComparison.OrdinalIgnoreCase)
                      || (name.Equals(elementNode.Current.AutomationId, StringComparison.OrdinalIgnoreCase)))
                {
                    return elementNode;
                }
                AutomationElement returnedAutomationElement = elementNode.SearchInRawTreeByName(name);
                if (returnedAutomationElement != null)
                {
                    return returnedAutomationElement;
                }
                elementNode = TreeWalker.RawViewWalker.GetNextSibling(elementNode);
            }
            return null;
        }
        #endregion

        #region Get Handle Methods

        public static AutomationElement GetHandleById(this AutomationElement ae, string controlId)
        {
            if (ae == null || String.IsNullOrEmpty(controlId))
            {
                throw new InvalidOperationException("invalid operation");
            }

            return ae.FindElementById(
                new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue(controlId));
        }

        public static AutomationElement GetHandleById<TApp>(this AutomationElement ae, string controlId)
            where TApp : AppLauncherBase
        {
            if (ae == null || String.IsNullOrEmpty(controlId))
            {
                throw new InvalidOperationException("invalid operation");
            }

            controlId = GetAppSpecificString<TApp>(controlId);
            return ae.GetHandleById(controlId);
        }

        public static AutomationElement GetHandleByContent(this AutomationElement ae, string content)
        {
            if (ae == null || String.IsNullOrEmpty(content))
            {
                throw new InvalidOperationException("invalid operation");
            }

            return ae.FindElementByContent(
                new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue(content));
        }

        public static AutomationElement GetHandleByContent<TApp>(this AutomationElement ae, string content)
            where TApp : AppLauncherBase
        {
            if (ae == null || String.IsNullOrEmpty(content))
            {
                throw new InvalidOperationException("invalid operation");
            }

            content = GetAppSpecificString<TApp>(content);
            return ae.GetHandleByContent(content);
        }

        #endregion

        #region Private Helper Methods

        private static void WalkThroughRawTreeAndPopulateAEList(AutomationElement parentElement, string name)
        {
            AutomationElement element = SearchInRawTreeByName(parentElement, name);
            AutomationElement elementNode = null;
            if (element != null)
            {
                // Add the element to the list;
                automationElementList.Add(element);

                elementNode = TreeWalker.RawViewWalker.GetNextSibling(element);
                while (elementNode != null)
                {
                    // Add the elementNode to the list
                    if (elementNode.Current.AutomationId.Equals(name))
                        automationElementList.Add(elementNode);
                    else
                    {
                        WalkThroughRawTreeAndPopulateAEList(elementNode, name);
                    }
                    elementNode = TreeWalker.RawViewWalker.GetNextSibling(elementNode);
                }
            }
        }

        /// <summary>
        /// Private helper method for modifying the string (resource file key string) based on the app type
        /// </summary>
        /// <typeparam name="TApp"></typeparam>
        /// <param name="controlId"></param>
        /// <returns></returns>
        private static string GetAppSpecificString<TApp>(string controlId)
            where TApp : AppLauncherBase
        {
            if (typeof(WpfAppLauncher).Equals(typeof(TApp)))
            {
                controlId += "_Wpf";
            }
            else if (typeof(SilverlightAppLauncher).Equals(typeof(TApp)))
            {
                controlId += "_Silverlight";
            }

            return controlId;
        }

        #endregion
    }
}
