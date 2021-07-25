Composite Application Guidance for WPF - June 2008
Welcome to the June 2008 release of the Composite Application Guidance for WPF. This file contains late-breaking information that can be useful in using the guidance.

============
Known Issues
============
To see the Composite Application Library known issues, see: http://www.codeplex.com/CompositeWPF/Wiki/View.aspx?title=Known%20Issues%20%2f%20Fixes.


===================================
Running Acceptance Tests with White
===================================

To run the acceptance tests, you must have White assemblies in {Application directory}\Source\Lib\White folder. The acceptance tests have been developed and verified with the White 0.1.5.0 release. Although other releases of White might work too, it is recommended to use the aforementioned release to avoid any issues when running the tests.

Steps to run the acceptance tests:
1. Download White assemblies from http://www.codeplex.com/white 
   (To download White 0.1.5.0 release :  http://www.codeplex.com/white/Release/ProjectReleases.aspx?ReleaseId=12756)
2. Copy the assemblies to {Application directory}\Source\Lib\White folder
3. Open and Run the acceptance test solution files.

==================================================     
Compiling the Composite Application Library Source
==================================================
To compile the Composite Application Library source code, perform these steps:

	1. Double-click the "Open Composite Application Library Solution.bat" batch file to open the solution in Visual Studio.
	2. Build the solution. The Composite Application Library assemblies will be placed in the CAL\Composite.UnityExtensions\bin\Debug folder.

========================================================
Solutions Included in the Composite Application Guidance
========================================================
The Composite Application Guidance includes different sample solutions. To open them in Visual Studio, you can run the following batch files:

	* "Open Hello World Solution.bat"				             : Opens the Hello World Solution
	* "Open QS-Commanding Quickstart Solution.bat" 			     : Opens the Commanding QuickStart Solution
	* "Open QS-ConfigurationModularity Quickstart Solution.bat"  : Opens the Configuration Modularity QuickStart Solution
	* "Open QS-DirectoryLookupModularity Quickstart Solution.bat": Opens the Directory Lookup Modularity QuickStart Solution
	* "Open QS-EventAggregation Quickstart Solution.bat" 		 : Opens the EventAggregation QuickStart Solution
	* "Open QS-UI Composition Quickstart Solution.bat" 		     : Opens the User Interface Composition QuickStart Solution
	* "Open RI-StockTrader Reference Implementation Solution.bat": Opens the Stock Trader Reference Implementation Solution

====================================
Documentation
====================================
The Composite Application Guidance includes the following documentation:

	* Composite Application Guidance for WPF - June 2008.chm. This is the guidance documentation.
	* Composite Application Library Reference - June 2008.chm. This is the class library reference documentation.

======================================================
Composite Application Library and Code Access Security
======================================================
Composite Application Library uses all the default .NET settings with regards to signing assemblies and code access security. It is a recommended practice to strong name all your assemblies, including the Composite Application Library assemblies, shell assembly, and any modules you might want to create. This is not a requirement. It is possible to load assemblies that have not been signed into a (signed or unsigned) Composite Application Library application. You can change this behavior by applying a .NET security policy that disallows the use of unsigned assemblies or one that changes the trust level of an assembly. Please note that the .NET Framework does not allow you to load partially trusted assemblies, unless you add the AllowPartiallyTrustedCallers attribute to the Composite Application Library assemblies. 

For more information, check the Code Access Security section on MSDN (http://msdn.microsoft.com/en-us/library/930b76w0.aspx).