Composite Application Guidance for WPF and Silverlight - February 2009
Welcome to the February 2009 release of the Composite Application Guidance for WPF and Silverlight. This file contains late-breaking information that can be useful in using the guidance.

============
Known Issues
============
To see the Composite Application Library known issues, see: http://www.codeplex.com/CompositeWPF/Wiki/View.aspx?title=Known%20Issues%20%2f%20Fixes.

============
System Requirements
============
This guidance was designed to run on the Microsoft Windows Vista, Windows XP Professional, Windows Server 2003, or Windows Server 2008 operating system. 
WPF applications built using this guidance will require the .NET Framework 3.5 SP1 to run and Silverlight applications will require the .NET Framework for Silverlight 2.

Before you can use the Composite Application Library, the following must be installed:
• Microsoft Visual Studio 2008 SP1
• Microsoft .NET Framework 3.5 SP1 (the .NET Framework 3.5 includes WPF): http://www.microsoft.com/downloads/details.aspx?familyid=AB99342F-5D1A-413D-8319-81DA479AB0D7&displaylang=en
• Microsoft Silverlight (this is required only if you are creating Silverlight applications): http://www.microsoft.com/silverlight/
• Microsoft Silverlight Tools for Visual Studio 2008 SP1 (this is required only if you are creating Silverlight applications): http://www.microsoft.com/downloads/details.aspx?FamilyId=c22d6a7b-546f-4407-8ef6-d60c8ee221ed&displaylang=en
	
You may also want to install the following:
• Microsoft Visual Studio 2008 Software Development Kit (SDK) 1.1 to compile Project Linker: http://www.microsoft.com/downloads/details.aspx?FamilyID=59ec6ec3-4273-48a3-ba25-dc925a45584d&DisplayLang=en
• Microsoft Silverlight Unit Test Framework to run the unit tests in Silverlight: http://code.msdn.microsoft.com/silverlightut
• White to run the acceptance tests. The acceptance tests have been developed and verified with the White 0.1.5.0 release: http://www.codeplex.com/white

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

	1. Double-click the "Desktop & Silverlight - Open Composite Application Library" batch file to open the solution in Visual Studio.
	2. Build the solution. 
		- The Composite Application Library assemblies for a Desktop application will be placed in the CAL\Desktop\Composite.UnityExtensions\bin\Debug folder.
		- The Composite Application Library assemblies for a Silverlight application will be placed in the CAL\Silverlight\Composite.UnityExtensions\bin\Debug folder.


========================================================
Solutions Included in the Composite Application Guidance
========================================================
The Composite Application Guidance includes different sample solutions. To open them in Visual Studio, you can run the following batch files:

 * "Desktop & Silverlight - Open Composite Application Library.bat"	
 * "Desktop & Silverlight - Open QS - Commanding QuickStart.bat"
 * "Desktop & Silverlight - Open QS - Defining Modules in Code QuickStart.bat"	
 * "Desktop & Silverlight - Open QS - EventAggregator QuickStart.bat"                                                                                                       
 * "Desktop & Silverlight - Open QS - MultiTargeting QuickStart.bat"                                                                                                        
 * "Desktop & Silverlight - Open QS - View Discovery QuickStart.bat"                                                                                                        
 * "Desktop & Silverlight - Open QS - View Injection QuickStart.bat"                                                                                                        
 * "Desktop & Silverlight - Open RI - StockTrader Reference Implementation.bat"                                                                                             
 * "Desktop only - Open Composite Application Library.bat"                                                                                                                  
 * "Desktop only - Open QS - Commanding QuickStart.bat"                                                                                                                     
 * "Desktop only - Open QS - Configuration Modularity QuickStart.bat"                                                                                                       
 * "Desktop only - Open QS - Defining Modules in Code QuickStart.bat"                                                                                                       
 * "Desktop only - Open QS - Directory Lookup Modularity QuickStart.bat"                                                                                                    
 * "Desktop only - Open QS - EventAggregator QuickStart.bat"                                                                                                                
 * "Desktop only - Open QS - Hello World QuickStart.bat"                                                                                                                    
 * "Desktop only - Open QS - View Discovery QuickStart.bat"                                                                                                                 
 * "Desktop only - Open QS - View Injection QuickStart.bat"                                                                                                                 
 * "Desktop only - Open RI - StockTrader Reference Implementation.bat"                                                                                                      
 * "Silverlight only - Open QS - Hello World QuickStart.bat"                                                                                                                
 * "Silverlight only - Open QS - Remote Module Loading QuickStart.bat"        


====================================
Documentation
====================================
The Composite Application Guidance includes the following documentation:

	* Composite Application Guidance for WPF and Silverlight - February 2009. This is the guidance documentation.
	* Composite Application Library Reference February 2009. This is the class library reference documentation for WPF and Silverlight.	

======================================================
Composite Application Library and Code Access Security
======================================================
Composite Application Library uses all the default .NET settings with regards to signing assemblies and code access security. It is a recommended practice to strong name all your assemblies, including the Composite Application Library assemblies, shell assembly, and any modules you might want to create. This is not a requirement. It is possible to load assemblies that have not been signed into a (signed or unsigned) Composite Application Library application. You can change this behavior by applying a .NET security policy that disallows the use of unsigned assemblies or one that changes the trust level of an assembly. Please note that the .NET Framework does not allow you to load partially trusted assemblies, unless you add the AllowPartiallyTrustedCallers attribute to the Composite Application Library assemblies. 

For more information, check the Code Access Security section on MSDN (http://msdn.microsoft.com/en-us/library/930b76w0.aspx).