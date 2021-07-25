// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using Microsoft.Practices.Prism.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.Composition.Tests.Logging
{
    [TestClass]
    public class TextLoggerFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTextWriterThrows()
        {
            ILoggerFacade logger = new TextLogger(null);
        }

        [TestMethod]
        public void ShouldWriteToTextWriter()
        {
            TextWriter writer = new StringWriter();
            ILoggerFacade logger = new TextLogger(writer);

            logger.Log("Test", Category.Debug, Priority.Low);
            StringAssert.Contains(writer.ToString(), "Test");
            StringAssert.Contains(writer.ToString(), "DEBUG");
            StringAssert.Contains(writer.ToString(), "Low");
        }

        [TestMethod]
        public void ShouldDisposeWriterOnDispose()
        {
            MockWriter writer = new MockWriter();
            IDisposable logger = new TextLogger(writer);

            Assert.IsFalse(writer.DisposeCalled);
            logger.Dispose();
            Assert.IsTrue(writer.DisposeCalled);
        }
    }

    internal class MockWriter : TextWriter
    {
        public bool DisposeCalled;
        public override Encoding Encoding
        {
            get { throw new NotImplementedException(); }
        }

        protected override void Dispose(bool disposing)
        {
            DisposeCalled = true;
            base.Dispose(disposing);
        }
    }
}