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
using Microsoft.Practices.Composite.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UIComposition.Modules.Employee.Services;
using UIComposition.Modules.Employee.Tests.Mocks;

namespace UIComposition.Modules.Employee.Tests
{
    [TestClass]
    public class EmployeesListPresenterFixture
    {
        MockEmployeesListView view;
        MockEmployeeService employeeService;

        [TestInitialize]
        public void SetUp()
        {
            view = new MockEmployeesListView();
            employeeService = new MockEmployeeService();
        }

        [TestMethod]
        public void CanInitPresenter()
        {
            EmployeesListPresenter presenter = CreatePresenter();

            Assert.AreEqual(view, presenter.View);
        }

        [TestMethod]
        public void InitPresenterShouldSetModel()
        {
            Assert.IsNull(view.Model);
            Assert.IsFalse(employeeService.RetrieveEmployeesCalled);
            EmployeesListPresenter presenter = CreatePresenter();

            Assert.IsNotNull(view.Model);
            Assert.IsTrue(employeeService.RetrieveEmployeesCalled);
            Assert.AreEqual(employeeService.RetrieveEmployees().Count, view.Model.Count);
        }

        [TestMethod]
        public void RaiseEmployeeSelectedShouldRaiseEmployeeSelected()
        {
            int employeeId = 10;
            bool employeeSelectedRaised = false;
            BusinessEntities.Employee selectedEmployee = null;
            BusinessEntities.Employee employee = new BusinessEntities.Employee(employeeId) { LastName = "Con", FirstName = "Aaron" };
            EmployeesListPresenter presenter = CreatePresenter();
            presenter.EmployeeSelected += delegate(object sender, DataEventArgs<BusinessEntities.Employee> e)
            {
                employeeSelectedRaised = true;
                selectedEmployee = e.Value;
            };

            Assert.IsFalse(employeeSelectedRaised);
            view.RaiseEmployeeSelected(employee);
            Assert.IsTrue(employeeSelectedRaised);
            Assert.AreEqual("Con", selectedEmployee.LastName);
            Assert.AreEqual("Aaron", selectedEmployee.FirstName);
            Assert.AreEqual(employeeId, selectedEmployee.EmployeeId);
        }

        private EmployeesListPresenter CreatePresenter()
        {
            return new EmployeesListPresenter(view, employeeService);
        }
    }
}
