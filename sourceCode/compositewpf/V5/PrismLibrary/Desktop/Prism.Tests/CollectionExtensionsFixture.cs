// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.Composition.Tests
{
    [TestClass]
    public class CollectionExtensionsFixture
    {
        [TestMethod]
        public void CanAddRangeToCollection()
        {
            Collection<object> col = new Collection<object>();
            List<object> itemsToAdd = new List<object>{"1", "2"};

            col.AddRange(itemsToAdd);

            Assert.AreEqual(2, col.Count);
            Assert.AreEqual("1", col[0]);
            Assert.AreEqual("2", col[1]);
        }
    }
}
