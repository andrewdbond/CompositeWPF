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
using StockTraderRI.AcceptanceTests.TestInfrastructure;
using Core.UIItems.WindowItems;
using Core.UIItems.Finders;
using System.Globalization;

namespace StockTraderRI.AcceptanceTests.Helpers
{
    public static class UIItemExtensions
    {
        public static void Hover(this UIItem uiItem)
        {
            Core.InputDevices.Mouse.Instance.Location = Core.C.Center(uiItem.Bounds);
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// Finds out whether an object exists in the raw tree with the name or automation id.
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="name">Either name or automation id of the control</param>
        /// <returns></returns>
        public static AutomationElement SearchInRawTreeByName(this AutomationElement rootElement, string name)
        {
            AutomationElement elementNode = TreeWalker.RawViewWalker.GetFirstChild(rootElement);

            while (elementNode != null)
            {
                if (name.Equals(elementNode.Current.Name, StringComparison.OrdinalIgnoreCase)
                    ||(name.Equals(elementNode.Current.AutomationId, StringComparison.OrdinalIgnoreCase)))
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

        #region Overloaded TryGet methods

        public static TItem TryGet<TItem>(this Window wind) where TItem : UIItem
        {
            try
            {
                return wind.Get<TItem>();
            }
            catch
            {
                return null;
            }
        }

        public static TItem TryGet<TItem>(this Window wind, SearchCriteria searchCriteria) where TItem : UIItem
        {
            try
            {
                return wind.Get<TItem>(searchCriteria);
            }
            catch
            {
                return null;
            }
        }

        public static IUIItem TryGet(this Window wind, SearchCriteria searchCriteria)
        {
            try
            {
                return wind.Get(searchCriteria);
            }
            catch
            {
                return null;
            }
        }

        public static TItem TryGet<TItem>(this Window wind, string primaryIdentification) where TItem : UIItem
        {
            try
            {
                return wind.Get<TItem>(primaryIdentification);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        public static UIItem GetWatchListRegionHeader(this UIItemContainer rootElement)
        {
            Tab tab = rootElement.Get<Tab>(TestDataInfrastructure.GetControlId("CollapsibleRegion"));
            UIItem watchListTab = tab.Pages.Find(x => x.NameMatches(TestDataInfrastructure.GetControlId("WatchListHeader"))) as UIItem;
            return watchListTab;
        }

        public static UIItem GetCollapsibleRegionHeader(this UIItemContainer rootElement, string controlId)
        {
            Tab tab = rootElement.Get<Tab>(TestDataInfrastructure.GetControlId("CollapsibleRegion"));
            UIItem watchListTab = tab.Pages.Find(x => x.NameMatches(TestDataInfrastructure.GetControlId(controlId))) as UIItem;
            return watchListTab;
        }

        /// <summary>
        /// Get the required field data from the specified row of the Position Table
        /// </summary>
        /// <param name="list">the position table UI object</param>
        /// <param name="rowNumber">Row number from which data needs to fished out of the Position table</param>
        /// <param name="header">column header of data that is expected</param>
        /// <returns>value in the specified field of the specified row of the Position table</returns>
        public static object GetData(this ListView list, int rowNumber, PositionTableColumnHeader header)
        {
            switch (header)
            {
                case PositionTableColumnHeader.Symbol:
                    return list.Rows[rowNumber].Cells[TestDataInfrastructure.GetTestInputData("PositionTableSymbol")].Text;
                case PositionTableColumnHeader.NumberOfShares:
                    return Convert.ToInt32(list.Rows[rowNumber].Cells[TestDataInfrastructure.GetTestInputData("PositionTableShares")].Text, CultureInfo.CurrentCulture);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Get the required field data for the specified symbol from the Position Table
        /// </summary>
        /// <param name="list">the position table UI object</param>
        /// <param name="forSymbol">symbol for which data is required</param>
        /// <param name="header">column header of data that is expected</param>
        /// <returns>value in the specified field of the specified symbol row of the Position table</returns>
        public static object GetData(this ListView list, string forSymbol, PositionTableColumnHeader header)
        {
            switch (header)
            {
                case PositionTableColumnHeader.Symbol:
                    return forSymbol;
                case PositionTableColumnHeader.NumberOfShares:
                    return Convert.ToInt32(list.Rows.Find(r => r.Cells[TestDataInfrastructure.GetTestInputData("PositionTableSymbol")].Text.Equals(forSymbol))
                        .Cells[TestDataInfrastructure.GetTestInputData("PositionTableShares")].Text, CultureInfo.CurrentCulture);
                default:
                    return null;
            }
        }
    }
}
