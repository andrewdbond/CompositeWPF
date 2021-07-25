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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Infrastructure.Converters;

namespace StockTraderRI.Infrastructure.Tests.Converters
{
	[TestClass]
	public class TwoDecimalPlaceConverterFixture
	{
        [TestMethod]
        public void TwoDecimalPlaceConverterRoundsToTwoDecimals()
        {
            TwoDecimalPlaceConverter converter = new TwoDecimalPlaceConverter();
            object value = converter.Convert(123.45678M, typeof(decimal), 2, null);
            
            Assert.AreEqual<decimal>(123.46M, Convert.ToDecimal(value));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void OnlyAcceptsDecimalValueArgument()
        {
            TwoDecimalPlaceConverter converter = new TwoDecimalPlaceConverter();
            converter.Convert("hello", null, 2, null);

        }
        
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void OnlyAcceptsIntegerNumDigitsArgument()
        {
            TwoDecimalPlaceConverter converter = new TwoDecimalPlaceConverter();
            converter.Convert(123.4467M, null, "foo", null);

        }
	
	}
}
