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
using Core.UIItems;

namespace AcceptanceTestLibrary.ApplicationHelper
{
    public static class WhiteExtensions
    {
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
