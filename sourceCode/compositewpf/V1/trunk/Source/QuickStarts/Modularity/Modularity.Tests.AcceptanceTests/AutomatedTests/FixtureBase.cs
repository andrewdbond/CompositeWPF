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
using Modularity.AcceptanceTests.ApplicationObserver;
using Core;
using Core.UIItems.WindowItems;
using Modularity.AcceptanceTests.TestInfrastructure;
using System.Collections.Specialized;
using Modularity.AcceptanceTests.Helpers;
using Core.Configuration;
using System.Globalization;

namespace Modularity.AcceptanceTests
{
    public abstract class FixtureBase : IStateObserver
    {
        private Application app;
        private Window window;
        private TestDataInfrastructure testDataInfrastructure;       

        public Window Window
        {
            get { return window; }
        }

        public TestDataInfrastructure TestDataInfrastructure
        {
            get { return testDataInfrastructure; }
        }

        public void TestInitialize()
        {
            SetupWhiteConfigParameters();

            // Instantiate and initiate the diagnosis process. Diagnosis steps are included
            // to identify the successful launch of the application window without any unexpected
            // exceptions.
            StateDiagnosis.Instance.StartDiagnosis(this);

            app = Application.Launch(GetApplicationToLaunch());
            window = app.GetWindow(GetApplicationWindowName(), Core.Factory.InitializeOption.NoCache);
            testDataInfrastructure = new TestDataInfrastructure();

            //Stop the diagnosis.
            StateDiagnosis.Instance.StopDiagnosis(this);
        }

        /// <summary>
        /// TestCleanup performs clean-up activities after each test method execution
        /// </summary>
        public void TestCleanup()
        {
            if (null != app)
            {
                app.Kill();
            }
        }

        public abstract string GetApplicationToLaunch();
        public abstract string GetApplicationWindowName();

        private static void SetupWhiteConfigParameters()
        {
            NameValueCollection collection = ConfigHandler.GetConfigSection("White/Core");

            Type coreAppXmlConfigType = CoreAppXmlConfiguration.Instance.GetType();
            foreach (string property in collection.Keys)
            {
                if (coreAppXmlConfigType.GetProperty(property).PropertyType.Equals(typeof(Int32)))
                {
                    coreAppXmlConfigType.GetProperty(property).SetValue(CoreAppXmlConfiguration.Instance, Convert.ToInt32(collection[property],CultureInfo.InvariantCulture), null);
                }
            }
        }

        #region IStateObserver Members

        public void Notify()
        {
            TestCleanup();
        }

        #endregion
    }
}
