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
using Core.UIItems;
using System.Windows.Automation;
using Core.UIItems.TabItems;

namespace Modularity.AcceptanceTests.Helpers
{
    public static class UIItemExtensions
    {
        public static void Hover(this UIItem uiItem)
        {
            Core.InputDevices.Mouse.Instance.Location = Core.C.Center(uiItem.Bounds);
            System.Threading.Thread.Sleep(1000);
        }

        public static AutomationElement SearchInRawTreeByName(this AutomationElement rootElement, string name)
        {
            AutomationElement elementNode = TreeWalker.RawViewWalker.GetFirstChild(rootElement);

            while (elementNode != null)
            {
                if (name.Equals(elementNode.Current.Name, StringComparison.OrdinalIgnoreCase))
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

        public static void SelectEmployee(this ListView list, int rowNumber)
        {
            list.Rows[rowNumber].Select();
        }

        public static void SelectEmployee(this ListView list, string firstName)
        {
            list.Rows.Find(r => r.Cells[0].Text.Equals(firstName)).Select();
        }

        public static void SelectEmployee(this ListView list, string firstName, string lastName)
        {
            list.Rows.Find(r => r.Cells[0].Text.Equals(firstName) && r.Cells[1].Text.Equals(lastName)).Select();
        }
    }
}
