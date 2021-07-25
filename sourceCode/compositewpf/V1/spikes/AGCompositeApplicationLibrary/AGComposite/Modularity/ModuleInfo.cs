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
using System.Collections.Generic;
using Microsoft.Practices.Composite.Properties;

namespace Microsoft.Practices.Composite.Modularity
{
    /// <summary>
    /// Defines the metadata that describes a module.
    /// </summary>
    // [Serializable]
    public class ModuleInfo
    {
        public IDictionary<string, object> Properties { get; set; }

        public ModuleInfo(IDictionary<string, object> properties)
        {
            Properties = properties;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ModuleInfo"/>.
        /// </summary>
        /// <param name="assemblyFile">The assembly file. Must be different than <see langword="null" />.</param>
        /// <param name="moduleType">The module's type.</param>
        /// <param name="moduleName">The module's name.</param>
        /// <param name="dependsOn">The names of the modules that this depends on.</param>
        public ModuleInfo(string assemblyFile, string moduleType, string moduleName, params string[] dependsOn)
            : this(assemblyFile, moduleType, moduleName, true, dependsOn)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ModuleInfo"/>.
        /// </summary>
        /// <param name="assemblyFile">The assembly file. Must be different than <see langword="null" />.</param>
        /// <param name="moduleType">The module's type.</param>
        /// <param name="moduleName">The module's name.</param>
        /// <param name="startupLoaded">Indicates whether this module should be loaded on startup.</param>
        /// <param name="dependsOn">The names of the modules that this depends on.</param>
        public ModuleInfo(string assemblyFile, string moduleType, string moduleName, bool startupLoaded, params string[] dependsOn)
        {
            this.Properties = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(assemblyFile))
                throw new ArgumentException(Resources.StringCannotBeNullOrEmpty, "assemblyFile");

            if (string.IsNullOrEmpty(moduleType))
                throw new ArgumentException(Resources.StringCannotBeNullOrEmpty, "moduleType");

            AssemblyFile = assemblyFile;
            ModuleType = moduleType;
            ModuleName = moduleName;
            StartupLoaded = startupLoaded;
            DependsOn = dependsOn != null ? new List<string>(dependsOn) : new List<string>();
        }

        /// <summary>
        /// Gets the assembly file where the module is located.
        /// </summary>
        /// <value>The assembly file where the module is located.</value>
        public string AssemblyFile
        {
            get { return Properties["AssemblyFile"].ToString(); }
            set { Properties["AssemblyFile"] = value; }
        }

        /// <summary>
        /// Gets the type of the module.
        /// </summary>
        /// <value>The type of the module.</value>
        public string ModuleType
        {
            get { return Properties["ModuleType"].ToString(); }
            set { Properties["ModuleType"] = value; }
        }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public string ModuleName
        {
            get { return Properties["ModuleName"].ToString(); }
            set { Properties["ModuleName"] = value; }
        }

        /// <summary>
        /// Gets the list of modules that this module depends upon.
        /// </summary>
        /// <value>The list of modules that this module depends upon.</value>
        public IList<string> DependsOn
        {
            get
            {
                if (Properties.ContainsKey("DependsOn"))
                {
                    return (IList<string>)Properties["DependsOn"];
                }
                return new List<string>();
            }
            set { Properties["DependsOn"] = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the module should be loaded at startup. 
        /// </summary>
        /// <value>A <see langword="bool"/> value indicating whether the module should be loaded at startup.</value>
        public bool StartupLoaded
        {
            get
            {
                if (!Properties.ContainsKey("StartupLoaded"))
                    return true;
                return bool.Parse(Properties["StartupLoaded"].ToString());
            }
            set { Properties["StartupLoaded"] = value.ToString(); }
        }
    }

    public static class ModuleInfoExtensions
    {
        public static void SetIsCore(this ModuleInfo moduleInfo, bool isCore)
        {
            moduleInfo.Properties["IsCore"] = isCore.ToString();
        }

        public static bool GetIsCore(this ModuleInfo moduleInfo)
        {
            if (!moduleInfo.Properties.ContainsKey("IsCore"))
                return false;

            return bool.Parse(moduleInfo.Properties["IsCore"].ToString());
        }

        public static void SetXapUri(this ModuleInfo moduleInfo, Uri xapUri)
        {
            if (xapUri != null)
            {
                moduleInfo.Properties["XapUri"] = xapUri.OriginalString;
            }
        }

        public static Uri GetXapUri(this ModuleInfo moduleInfo)
        {
            if (!moduleInfo.Properties.ContainsKey("XapUri"))
            {
                return null;
            }

            return new Uri(moduleInfo.Properties["XapUri"].ToString());
        }
    }
}
