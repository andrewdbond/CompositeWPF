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
using UIComposition.Modules.Employee.Tests.Mocks;

namespace UIComposition.Modules.Employee.Tests
{
    [TestClass]
    public class EmployeesDetailsPresenterFixture
    {
        MockEmployeesDetailsView view;

        [TestInitialize]
        public void SetUp()
        {
            view = new MockEmployeesDetailsView();
        }

        [TestMethod]
        public void CanInitPresenter()
        {
            EmployeesDetailsPresenter presenter = CreatePresenter();

            Assert.AreEqual(view, presenter.View);
        }

        [TestMethod]
        public void ShouldSetModelOnDetailsView()
        {
            int employeeId = 10;
            BusinessEntities.Employee employee = new BusinessEntities.Employee(employeeId) { LastName = "Con", FirstName = "Aaron" };
            
            EmployeesDetailsPresenter presenter = CreatePresenter();

            Assert.IsNull(view.Model);
            
            presenter.SetSelectedEmployee(employee);

            Assert.IsNotNull(view.Model);
            Assert.AreEqual("Con", view.Model.SelectedEmployee.LastName);
            Assert.AreEqual("Aaron", view.Model.SelectedEmployee.FirstName);
            Assert.AreEqual(employeeId, view.Model.SelectedEmployee.EmployeeId);
        }

        private EmployeesDetailsPresenter CreatePresenter()
        {
            return new EmployeesDetailsPresenter(view);
        }
    }
}
