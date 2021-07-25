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
using UIComposition.Modules.Employee.Tests.Mocks;
using UIComposition.Modules.Project;

namespace UIComposition.Modules.Employee.Tests
{
    [TestClass]
    public class EmployeesPresenterFixture
    {
        MockEmployeesView view;
        MockEmployeesListPresenter listPresenter;
        MockEmployeesController controller;

        [TestInitialize]
        public void SetUp()
        {
            view = new MockEmployeesView();
            controller = new MockEmployeesController();
        }

        [TestMethod]
        public void CanInitPresenter()
        {
            EmployeesPresenter presenter = CreatePresenter();

            Assert.IsNotNull(presenter);
            Assert.AreEqual<IEmployeesView>(view, presenter.View);
        }

        [TestMethod]
        public void RaiseEmployeeSelectedShouldCallController()
        {
            // Todo: Find out how to test this
            EmployeesPresenter presenter = CreatePresenter();

            Assert.IsFalse(controller.ShowEmployeeDetailsCalled);

            presenter.View.Model.SelectedEmployee = new BusinessEntities.Employee(1);

            Assert.IsTrue(controller.ShowEmployeeDetailsCalled);           
        }

        private EmployeesPresenter CreatePresenter()
        {
            EmployeesPresenter presenter = new EmployeesPresenter(view, controller);
            return presenter;
        }
    }
}