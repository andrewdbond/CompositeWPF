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
using StockTraderRI.Modules.Position.Orders;
using System.Diagnostics.CodeAnalysis;

namespace StockTraderRI.Modules.Position.Interfaces
{
    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification="This interface is used as a contract type in the MEF container.")]
    public interface IOrdersView
    {
        // The OrdersController.ShowOrdersView method adds an OrdersView to the action region.
        // The OrdersView implements this interface and is exported in the container.
        // Therefore, the OrdersController is abstracted of the concrete implementation of the view,
        // while still being able of retrieving it from the container and adding it to a region.
    }
}
