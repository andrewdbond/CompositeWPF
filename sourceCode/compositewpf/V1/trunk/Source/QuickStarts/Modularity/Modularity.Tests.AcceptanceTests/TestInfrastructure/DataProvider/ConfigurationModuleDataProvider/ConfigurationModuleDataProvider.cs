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
using Modularity.AcceptanceTests.Helpers;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Data;
using System.Globalization;

namespace Modularity.AcceptanceTests.TestInfrastructure
{
    public class ConfigurationModuleDataProvider : DataProviderBase<Module>
    {
        public override string GetDataFilePath()
        {
            return String.Concat(ConfigHandler.GetValue(ApplicationEnumeration.ConfigurationModularity.ToString()), ".config");
        }

        public override List<Module> GetData()
        {
            List<Module> modules = new List<Module>();
            Module module;
            string fileName = GetDataFilePath();

            XDocument xDoc = XDocument.Load(fileName);
            IEnumerable<XElement> modulesSection = xDoc.Descendants(TestDataInfrastructure.GetTestInputData("ModuleSectionName"));
            XAttribute startupLoaded;

            if (null != modulesSection)
            {
                foreach (XElement moduleElement in modulesSection)
                {
                    module = new Module(moduleElement.Attribute(TestDataInfrastructure.GetTestInputData("ModuleNameAttribute")).Value);
                    startupLoaded = moduleElement.Attribute(TestDataInfrastructure.GetTestInputData("StartupLoadingAttributeConfigDriven"));
                    module.AllowsDelayLoading = (null == startupLoaded) ? false : startupLoaded.Value.Equals("false");

                    foreach (XElement dependency in moduleElement.Descendants(TestDataInfrastructure.GetTestInputData("DependenciesNode"))
                        .Descendants(TestDataInfrastructure.GetTestInputData("DependencyNode")))
                    {
                        module.Dependencies.Add(dependency.Attribute(TestDataInfrastructure.GetTestInputData("ModuleNameAttribute")).Value);
                    }

                    modules.Add(module);
                }

                return modules ?? null;
            }
            else
            {
                return null;
            }
        }
    }
}
