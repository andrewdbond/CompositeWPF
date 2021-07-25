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
using System.Collections.ObjectModel;

namespace UIComposition.Modules.Employee.Services
{
    public class EmployeeService : IEmployeeService
    {
        public ObservableCollection<BusinessEntities.Employee> RetrieveEmployees()
        {
            ObservableCollection<BusinessEntities.Employee> employees = new ObservableCollection<BusinessEntities.Employee>();

            employees.Add(new BusinessEntities.Employee(1) { FirstName = "John", LastName = "Smith", Phone = "+1 (425) 555-0101", Email = "john.smith@example.com", Address = "One Microsoft Way", City = "Redmond", State = "WA" });
            employees.Add(new BusinessEntities.Employee(2) { FirstName = "Bonnie", LastName = "Skelly", Phone = "+1 (425) 555-0105", Email = "bonnie.skelly@example.com", Address = "One Microsoft Way", City = "Redmond", State = "WA"});

            return employees;
        }
    }
}
