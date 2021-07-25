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
using System.Configuration;
using System.Collections.Specialized;
using Modularity.AcceptanceTests.TestInfrastructure;
using System.Collections.Generic;
using System.IO;
using System.Collections.ObjectModel;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Xml;
using System.Data;

namespace Modularity.AcceptanceTests.Helpers
{
    /// <summary>
    /// Class use for handling the config file and XML Test Data reading
    /// </summary>
    public static class ConfigHandler
    {
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? String.Empty;
        }

        public static NameValueCollection GetConfigSection(string name)
        {
            return (NameValueCollection)ConfigurationManager.GetSection(name) ?? null;
        }
    }
}
