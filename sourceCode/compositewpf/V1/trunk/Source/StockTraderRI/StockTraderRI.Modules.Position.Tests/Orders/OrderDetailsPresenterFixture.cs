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
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Models;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Modules.Position.Tests.Mocks;
using System.ComponentModel;

namespace StockTraderRI.Modules.Position.Tests.Orders
{
    [TestClass]
    public class OrderDetailsPresenterFixture
    {
        [TestMethod]
        public void PresenterProvidesViewModelToBindTo()
        {
            var view = new MockOrderDetailsView();
            var presenter = new OrderDetailsPresentationModel(view, null, null);

            Assert.IsNotNull(view.Model);
        }

        [TestMethod]
        public void PresenterCreatesPublicSubmitCommand()
        {
            var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(), null);

            Assert.IsNotNull(presenter.SubmitCommand);
        }

        [TestMethod]
        public void CanExecuteChangedIsRaisedForSubmitCommandWhenModelChanges()
        {
            var view = new MockOrderDetailsView();
            bool canExecuteChanged = false;

            var presenter = new TestableOrderDetailsPresenter(view, null);

            presenter.SubmitCommand.CanExecuteChanged += delegate { canExecuteChanged = true; };

            presenter.Shares = 2;

            Assert.IsTrue(canExecuteChanged);
        }

        [TestMethod]
        public void CannotSubmitWhenSharesIsNotPositive()
        {
            var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(), null);

            presenter.Shares = 2;
            presenter.StopLimitPrice = 2;
            Assert.IsTrue(presenter.SubmitCommand.CanExecute(null));

            presenter.Shares = 0;
            Assert.IsFalse(presenter.SubmitCommand.CanExecute(null));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubmitThrowsIfCanExecuteIsFalse()
        {
            var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(),
                                                              new MockAccountPositionService());
            presenter.Shares = 0;
            Assert.IsFalse(presenter.SubmitCommand.CanExecute(null));

            presenter.SubmitCommand.Execute(null);
        }

        [TestMethod]
        public void CancelRaisesCloseViewEvent()
        {
            bool closeViewRaised = false;

            var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(), null);
            presenter.CloseViewRequested += delegate
            {
                closeViewRaised = true;
            };

            presenter.CancelCommand.Execute(null);

            Assert.IsTrue(closeViewRaised);
        }

        [TestMethod]
        public void SubmitRaisesCloseViewEvent()
        {
            bool closeViewRaised = false;

            var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(),
                                                              new MockAccountPositionService());
            presenter.CloseViewRequested += delegate
            {
                closeViewRaised = true;
            };

            presenter.TransactionType = TransactionType.Buy;
            presenter.Shares = 1;
            presenter.StopLimitPrice = 1;
            Assert.IsTrue(presenter.SubmitCommand.CanExecute(null));
            presenter.SubmitCommand.Execute(null);

            Assert.IsTrue(closeViewRaised);
        }

        [TestMethod]
        public void CannotSubmitOnSellWhenSharesIsHigherThanCurrentPosition()
        {
            var accountPositionService = new MockAccountPositionService();
            accountPositionService.AddPosition(new AccountPosition("TESTFUND", 10m, 15));
            var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(), accountPositionService);

            presenter.TickerSymbol = "TESTFUND";
            presenter.TransactionType = TransactionType.Sell;
            presenter.Shares = 5;
            presenter.StopLimitPrice = 1;
            Assert.IsTrue(presenter.SubmitCommand.CanExecute(null));

            presenter.Shares = 16;
            Assert.IsFalse(presenter.SubmitCommand.CanExecute(null));
        }

        [TestMethod]
        public void PresenterCreatesCallSetOrderTypes()
        {
            var view = new MockOrderDetailsView();
            var presenter = new OrderDetailsPresentationModel(view, null, null);
            Assert.IsNotNull(presenter.AvailableOrderTypes);
            Assert.IsTrue(presenter.AvailableOrderTypes.Count > 0);
            Assert.AreEqual(Enum.GetValues(typeof(OrderType)).Length, presenter.AvailableOrderTypes.Count);

        }

        [TestMethod]
        public void PresenterCreatesCallSetTimeInForce()
        {
            var view = new MockOrderDetailsView();
            var presenter = new OrderDetailsPresentationModel(view, null, null);
            Assert.IsNotNull(presenter.AvailableTimesInForce);
            Assert.IsTrue(presenter.AvailableTimesInForce.Count > 0);
            Assert.AreEqual(Enum.GetValues(typeof(TimeInForce)).Length, presenter.AvailableTimesInForce.Count);
        }

        [TestMethod]
        public void SetTransactionInfoShouldUpdateTheModel()
        {
            var view = new MockOrderDetailsView();
            var presenter = new OrderDetailsPresentationModel(view, new MockAccountPositionService(), null);
            presenter.TransactionInfo = new TransactionInfo { TickerSymbol = "T000", TransactionType = TransactionType.Sell };

            Assert.AreEqual("T000", presenter.TickerSymbol);
            Assert.AreEqual(TransactionType.Sell, presenter.TransactionType);
        }

        [TestMethod]
        public void PresenterUpdatesCommandsBasedOnActiveChangedOfView()
        {
            var view = new MockOrderDetailsView();
            var presenter = new TestableOrderDetailsPresenter(view, null);
            view.IsActive = true;
            view.RaiseIsActiveChanged();

            Assert.IsTrue(presenter.CancelCommand.IsActive);

            view.IsActive = false;
            view.RaiseIsActiveChanged();

            Assert.IsFalse(presenter.CancelCommand.IsActive);

            view.IsActive = true;
            view.RaiseIsActiveChanged();

            Assert.IsTrue(presenter.CancelCommand.IsActive);
        }

        [TestMethod]
        public void PresenterInitializesCommandsBasedOnActiveView()
        {
            var view = new MockOrderDetailsView();
            view.IsActive = true;

            var presenter = new TestableOrderDetailsPresenter(view, null);

            Assert.IsTrue(presenter.CancelCommand.IsActive);
        }

        [TestMethod]
        public void SubmitCallsServiceWithCorrectOrder()
        {
            var ordersService = new MockOrdersService();
            var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(),
                                                              new MockAccountPositionService(), ordersService);
            presenter.Shares = 2;
            presenter.TickerSymbol = "AAAA";
            presenter.TransactionType = TransactionType.Buy;
            presenter.TimeInForce = TimeInForce.EndOfDay;
            presenter.OrderType = OrderType.Limit;
            presenter.StopLimitPrice = 15;

            Assert.IsNull(ordersService.SubmitArgumentOrder);
            presenter.SubmitCommand.Execute(null);

            var submittedOrder = ordersService.SubmitArgumentOrder;
            Assert.IsNotNull(submittedOrder);
            Assert.AreEqual("AAAA", submittedOrder.TickerSymbol);
            Assert.AreEqual(TransactionType.Buy, submittedOrder.TransactionType);
            Assert.AreEqual(TimeInForce.EndOfDay, submittedOrder.TimeInForce);
            Assert.AreEqual(OrderType.Limit, submittedOrder.OrderType);
            Assert.AreEqual(15, submittedOrder.StopLimitPrice);
        }

        [TestMethod]
        public void VerifyTransactionInfoModificationsInOrderDetails()
        {
            var view = new MockOrderDetailsView();
            var orderDetailsPresenter = new OrderDetailsPresentationModel(view, new MockAccountPositionService(), null);
            var transactionInfo = new TransactionInfo { TickerSymbol = "Fund0", TransactionType = TransactionType.Buy };
            orderDetailsPresenter.TransactionInfo = transactionInfo;
            orderDetailsPresenter.TransactionType = TransactionType.Sell;
            Assert.AreEqual(TransactionType.Sell, transactionInfo.TransactionType);

            orderDetailsPresenter.TickerSymbol = "Fund1";
            Assert.AreEqual("Fund1", transactionInfo.TickerSymbol);
        }


        [TestMethod]
        public void CannotSubmitIfStopLimitZero()
        {
            var accountPositionService = new MockAccountPositionService();
            accountPositionService.AddPosition(new AccountPosition("TESTFUND", 10m, 15));
            var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(), accountPositionService);

            presenter.TickerSymbol = "TESTFUND";
            presenter.TransactionType = TransactionType.Sell;
            presenter.Shares = 5;
            presenter.StopLimitPrice = 1;
            Assert.IsTrue(presenter.SubmitCommand.CanExecute(null));

            presenter.StopLimitPrice = 0;
            Assert.IsFalse(presenter.SubmitCommand.CanExecute(null));
        }

        [TestMethod]
        public void ShouldSetStopLimitPriceInModel()
        {
            var accountPositionService = new MockAccountPositionService();
            accountPositionService.AddPosition(new AccountPosition("TESTFUND", 10m, 15));
            var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(), accountPositionService);

            presenter.TickerSymbol = "TESTFUND";
            presenter.TransactionType = TransactionType.Sell;
            presenter.Shares = 5;
            presenter.StopLimitPrice = 0;

            Assert.AreEqual<string>("The stop limit price must be greater than 0", presenter["StopLimitPrice"]);
        }



        //[TestMethod]
        //public void DisposeUnregistersLocalCommandsFromGlobalCommands()
        //{
        //    var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(), null);
        //    Assert.IsTrue(StockTraderRICommands.SubmitOrderCommand.
        //}
        [TestMethod]
        public void PropertyChangedIsRaisedWhenSharesIsChanged()
        {
            var presenter = new OrderDetailsPresentationModel(new MockOrderDetailsView(),null,null);
            presenter.Shares = 5;

            bool sharesPropertyChangedRaised = false;
            presenter.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "Shares")
                    sharesPropertyChangedRaised = true;
            };
            presenter.Shares = 0;
            Assert.IsTrue(sharesPropertyChangedRaised);
        }
    }

    internal class MockOrdersService : IOrdersService
    {
        public Order SubmitArgumentOrder;

        public void Submit(Order order)
        {
            SubmitArgumentOrder = order;
        }
    }

    class TestableOrderDetailsPresenter : OrderDetailsPresentationModel
    {
        public TestableOrderDetailsPresenter(IOrderDetailsView view, IAccountPositionService accountPositionService)
            : this(view, accountPositionService, new MockOrdersService())
        {
        }

        public TestableOrderDetailsPresenter(IOrderDetailsView view, IAccountPositionService accountPositionService, IOrdersService ordersService)
            : base(view, accountPositionService, ordersService)
        {

        }
    }
}
