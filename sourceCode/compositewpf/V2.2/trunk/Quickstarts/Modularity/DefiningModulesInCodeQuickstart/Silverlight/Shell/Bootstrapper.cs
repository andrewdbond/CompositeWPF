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
using System.IO;
using System.Windows;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.UnityExtensions;
using Modules.ModuleA;
using Modules.ModuleB;
using Modules.ModuleC;
using Modules.ModuleD;

namespace DefiningModulesInCode
{
    public class Bootstrapper : UnityBootstrapper
    {
        // Define the name of ModuleB
        const string moduleBAssemblyQualifiedName = "Modules.ModuleB.ModuleB, Modules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

        // In silverlight, the REF value will likely be the URL to the XAP file where you can download the file
        // For example:
        // const string refValue = "url://servername/ModuleB.xap";
        // In this quickstart, this is not needed, because the ModuleB is already present. 

        // In WPF, the REF value will likely be the location where the system can find the file, if the path 
        // is not in the normal .net Assembly probe paths. For example:
        // const string refValue = "file://c:/Modules/ModuleB.dll";
      
        protected override IModuleCatalog GetModuleCatalog()
        {
            ModuleCatalog catalog = new ModuleCatalog();

            // There are two ways of adding modules to the catalog in code
            // 1: If the shell has a direct reference to the modules, you can use
            //    typeof(Module) to add the module. 
            catalog.AddModule(typeof (ModuleA), "ModuleD")
                .AddModule(typeof (ModuleD), "ModuleB")
                .AddModule(typeof (ModuleC), InitializationMode.OnDemand)
                ;

            // 2: If the shell does not have a direct reference to the module, you have to
            //    specify the Assembly qualified typename. You might also have to specify where the 
            //    file can be found by specifying a 'Ref' value. (In this example it's not needed,
            //    because the assembly is in the .net probe path)
            catalog.AddModule(new ModuleInfo("ModuleB", moduleBAssemblyQualifiedName));
                
            return catalog;
        }

        protected override DependencyObject CreateShell()
        {
            Shell shell = new Shell();
#if SILVERLIGHT
            Application.Current.RootVisual = shell;
#else
            Application.Current.MainWindow = shell;
            shell.Show();
#endif

            return shell;
        }
    }
}


