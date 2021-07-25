// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Modularity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.Composition.Tests.Modularity
{
    [TestClass]
    public class ModuleInfoGroupFixture
    {
        [TestMethod]
        public void ShouldForwardValuesToModuleInfo()
        {
            ModuleInfoGroup group = new ModuleInfoGroup();
            group.Ref = "MyCustomGroupRef";
            ModuleInfo moduleInfo = new ModuleInfo();
            Assert.IsNull(moduleInfo.Ref);

            group.Add(moduleInfo);

            Assert.AreEqual(group.Ref, moduleInfo.Ref);
        }
    }
}
