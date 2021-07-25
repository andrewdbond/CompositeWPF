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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Models;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Modules.Position.Tests.Mocks;

namespace StockTraderRI.Modules.Position.Tests.Orders
{
    /// <summary>
    /// Summary description for OrderCompositePresenterFixture
    /// </summary>
    [TestClass]
    public class OrderCompositePresentationModelFixture
    {
        [TestMethod]
        public void ShouldCreateOrderDetailsPresenter()
        {
            var detailsPresenter = new MockOrderDetailsPresentationModel();
            IOrderCompositeView compositeView = new MockOrderCompositeView();

            var composite = new OrderCompositePresentationModel(compositeView, detailsPresenter, new OrderCommandsView());

            composite.TransactionInfo = new TransactionInfo("FXX01", TransactionType.Sell);

            Assert.IsNotNull(detailsPresenter.TransactionInfo);
        }

        [TestMethod]
        public void ShouldAddDetailsViewAndControlsViewToContentArea()
        {
            MockOrderCompositeView compositeView = new MockOrderCompositeView();
            var detailsPresenter = new MockOrderDetailsPresentationModel();

            var composite = new OrderCompositePresentationModel(compositeView, detailsPresenter, new OrderCommandsView());

            Assert.AreEqual(detailsPresenter.View, composite.OrderDetailsView);
            Assert.IsNotNull(composite.OrderCommandsView as OrderCommandsView);
        }

        [TestMethod]
        public void PresenterExposesChildOrderPresentersCloseRequested()
        {
            var detailsPresenter = new MockOrderDetailsPresentationModel();
            MockOrderCompositeView compositeView = new MockOrderCompositeView();

            var composite = new OrderCompositePresentationModel(compositeView, detailsPresenter, new OrderCommandsView());

            var closeViewRequestedFired = false;
            composite.CloseViewRequested += delegate
                                                {
                                                    closeViewRequestedFired = true;
                                                };

            detailsPresenter.RaiseCloseViewRequested();

            Assert.IsTrue(closeViewRequestedFired);

        }

        [TestMethod]
        public void ShouldDelegateIsActivePropertyChangesToDetailView()
        {
            var detailsPresenter = new MockOrderDetailsPresentationModel();
            MockOrderCompositeView compositeView = new MockOrderCompositeView();

            var composite = new OrderCompositePresentationModel(compositeView, detailsPresenter, new OrderCommandsView());
            compositeView.IsActive = false;

            Assert.IsFalse(detailsPresenter.View.IsActive);
            compositeView.IsActive = true;
            Assert.IsTrue(detailsPresenter.View.IsActive);
        }

        [TestMethod]
        public void ShouldPassModelToView()
        {
            var view = new MockOrderCompositeView();
            var composite = new OrderCompositePresentationModel(view, new MockOrderDetailsPresentationModel(), new OrderCommandsView());

            composite.TransactionInfo = new TransactionInfo("FXX01", TransactionType.Sell);

            Assert.IsNotNull(view);
            Assert.AreEqual(composite, view.Model);
        }

        [TestMethod]
        public void ShouldSetHeaderInfo()
        {
            var composite = new OrderCompositePresentationModel(new MockOrderCompositeView(), new MockOrderDetailsPresentationModel(), new OrderCommandsView());

            composite.TransactionInfo = new TransactionInfo("FXX01", TransactionType.Sell);

            Assert.IsNotNull(composite.HeaderInfo);
            Assert.IsTrue(composite.HeaderInfo.Contains("FXX01"));
            Assert.IsTrue(composite.HeaderInfo.Contains("Sell"));
            Assert.AreEqual("Sell FXX01", composite.HeaderInfo);
        }

        [TestMethod]
        public void ShouldUpdateHeaderInfoWhenUpdatingTransactionInfo()
        {
            var orderDetailsPM = new MockOrderDetailsPresentationModel();
            var composite = new OrderCompositePresentationModel(new MockOrderCompositeView(), orderDetailsPM, new OrderCommandsView());

            composite.TransactionInfo = new TransactionInfo("FXX01", TransactionType.Sell);

            orderDetailsPM.TransactionInfo.TickerSymbol = "NEW_SYMBOL";
            Assert.AreEqual("Sell NEW_SYMBOL", composite.HeaderInfo);

            orderDetailsPM.TransactionInfo.TransactionType = TransactionType.Buy;
            Assert.AreEqual("Buy NEW_SYMBOL", composite.HeaderInfo);
        }

        [TestMethod]
        public void TransactionInfoAndSharesAndCommandsAreTakenFromOrderDetails()
        {
            var orderDetailsPM = new MockOrderDetailsPresentationModel();
            var composite = new OrderCompositePresentationModel(new MockOrderCompositeView(), orderDetailsPM, new OrderCommandsView());
            orderDetailsPM.Shares = 100;

            Assert.AreEqual(orderDetailsPM.Shares, composite.Shares);
            Assert.AreSame(orderDetailsPM.SubmitCommand, composite.SubmitCommand);
            Assert.AreSame(orderDetailsPM.CancelCommand, composite.CancelCommand);
            Assert.AreSame(orderDetailsPM.TransactionInfo, composite.TransactionInfo);
        }
	
    }

    internal class MockOrderCompositeView : IOrderCompositeView
    {
        private bool _IsActive = false;

        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                _IsActive = value;
                IsActiveChanged(this, EventArgs.Empty);

            }
        }

        public event EventHandler IsActiveChanged;

        public IOrderCompositePresentationModel Model { get; set; }
    }
}
