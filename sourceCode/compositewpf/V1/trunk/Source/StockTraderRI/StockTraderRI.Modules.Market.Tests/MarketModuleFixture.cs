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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Market.Tests.Mocks;

namespace StockTraderRI.Modules.Market.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MarketModuleFixture
    {

        [TestMethod]
        [DeploymentItem("Data/MarketHistory.xml", "Data")]
        public void CanInitModule()
        {
            var container = new MockUnityContainer();
            MarketModule module = new MarketModule(container);

            module.Initialize();

            Assert.IsTrue(container.Types.ContainsKey(typeof(IMarketHistoryService)));
            Assert.IsTrue(container.Types.ContainsKey(typeof(IMarketFeedService)));
            Assert.IsTrue(container.Types.ContainsKey(typeof(ITrendLineView)));
            Assert.IsTrue(container.Types.ContainsKey(typeof(ITrendLinePresenter)));
        }
    }
}
