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

using System.Collections.ObjectModel;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Infrastructure.Converters;

namespace StockTraderRI.Infrastructure.Tests.Converters
{
    [TestClass]
    public class ErrorConverterFixture
    {
        [TestMethod]
        public void ShouldReturnEmptyStringIfValueIsNull()
        {
            ErrorConverter converter = new ErrorConverter();

            object value = null;

            object result = converter.Convert(value, null, null, null);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ShouldReturnEmptyStringIfCollectionIsEmpty()
        {
            ErrorConverter converter = new ErrorConverter();

            ObservableCollection<ValidationError> value = new ObservableCollection<ValidationError>();

            object result = converter.Convert(value, null, null, null);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ShouldReturnTheErrorContentOfTheFirstItemInTheCollection()
        {
            ErrorConverter converter = new ErrorConverter();

            ObservableCollection<ValidationError> errors = new ObservableCollection<ValidationError>();
            ValidationError error = new ValidationError(new ExceptionValidationRule(), new object());
            error.ErrorContent = "TestError";
            errors.Add(error);

            ReadOnlyObservableCollection<ValidationError> value = new ReadOnlyObservableCollection<ValidationError>(errors);
            object result = converter.Convert(value, null, null, null);

            Assert.AreEqual("TestError", result);
        }
    }
}
