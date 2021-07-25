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
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UIComposition.Modules.Project.Services;

namespace UIComposition.Modules.Project.Tests.Services
{
    /// <summary>
    /// Summary description for ProjectServiceFixture
    /// </summary>
    [TestClass]
    public class ProjectServiceFixture
    {
        [TestMethod]
        public void ShouldReturnDefaultData()
        {
            IProjectService service = new ProjectService();
            
            int count = service.RetrieveProjects(1).Count;

            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void ShouldReturnNullIfEmployeeNotExists()
        {
            IProjectService service = new ProjectService();

            Assert.IsNull(service.RetrieveProjects(100));
        }
    }
}
