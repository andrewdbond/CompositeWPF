//===============================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
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
using Microsoft.Practices.Composite.Modularity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Tests.Modularity
{
    [TestClass]
    public class ModuleInfoFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullAssemblyFileThrows()
        {
            ModuleInfo moduleInfo = new ModuleInfo(null, "moduleType", "moduleName", true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyAssemblyFileThrows()
        {
            ModuleInfo moduleInfo = new ModuleInfo(string.Empty, "moduleType", "moduleName", true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullModuleTypeThrows()
        {
            ModuleInfo moduleInfo = new ModuleInfo("assemblyFile", null, "moduleName", true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyModuleTypeThrows()
        {
            ModuleInfo moduleInfo = new ModuleInfo("assemblyFile", string.Empty, "moduleName", true);
        }
    }
}