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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using System.Diagnostics;
using Core.UIItems.WindowItems;
using System.Globalization;
using Core.UIItems.TreeItems;
using Core.UIItems.MenuItems;
using Core.UIItems.Finders;
using System.Windows.Automation;
using System.IO;
using System.Threading;

namespace AcceptanceTestLibrary.VSHelper
{
    /// <summary>
    /// Helper Class for performing common white operations for visual studio IDE.
    /// </summary>
    public class WhiteHelper
    {
        # region VS Constants
        //VS Executable constant
        private const string VSAPP = "devenv.exe";

        //Project Linker Window constant
        private const string PROJECTLINKWINDOW = "Project Links";

        //Solution Viewer window for creating links 
        private const string SOLUTIONVIEWERWINDOW = "Select Source Project";

        //Solution Viewer window for file delete 
        private const string SOLUTIONVIEWERDELETEWINDOW = "Microsoft Visual Studio";
        
        //Solution Explorer Tree 
        private const string  SOLUTIONEXPLORER = "Solution Explorer";

        //Add New Item Window dialogue Format
        private const String NEWITEMTITLE = "Add New Item - {0}";

        //Add project Link menu constant
        private const string ADDPROJECTLINK = "Add project link...";

        # endregion

        # region Helper Methods for Visual Studio IDE Tasks

        /// <summary>
        /// Locates the Project Links window with in the given VS applicaion instance
        /// </summary>
        /// <param name="VsApp">VS App instance to look for the given instance </param>
        /// <returns>returns Window if available else returns returns null </returns>
        public static Window GetProjectLinksWindow(Application VsApp)
        {
            return FindWindow(VsApp, PROJECTLINKWINDOW);
        }

        /// <summary>
        /// Locates the Project Links window with in the given VS applicaion instance
        /// </summary>
        /// <param name="VsApp">VS App instance to look for the given instance</param>
        /// <returns>returns Window if available else returns null </returns>
        public static Window GetSolutionViewerWindow(Application VsApp)
        {
            return FindWindow(VsApp, SOLUTIONVIEWERWINDOW);
        }

        /// <summary>
        /// Finds the Window with WindowTitle with in the given VS applicaion instance
        /// </summary>
        /// <param name="VsApp"></param>
        /// <param name="WindowTitle"></param>
        /// <returns>returns Window if available else returns null if window with specified title is not available</returns>
        public static Window FindWindow(Application VsApp,String WindowTitle)
        {
            List<Window> availableWindows = VsApp.GetWindows();
            foreach(Window windowIndex in availableWindows)
            {
                if(string.Compare(windowIndex.Title,WindowTitle,true,CultureInfo.InvariantCulture)==0)
                    return windowIndex;
            }
            return null;
        }

        /// <summary>
        /// Gets Solution Explorer Tree
        /// </summary>
        /// <param name="vsWindow">Main VS window></param>
        /// <returns>Solution Explorer Tree</returns>
        public static Tree GetSolutionExplorerTree(Window vsWindow)
        {
            return vsWindow.Get<Tree>(SOLUTIONEXPLORER);
        }

        /// <summary>
        /// Adds New Class / other files to the project.
        /// </summary>
        /// <param name="SolutionExplorerTree">Solution Explorer which contains the project</param>
        /// <param name="ProjectName">Project Name to which to add the class to</param>
        /// <param name="Class">Name of the new class / other items to be added</param>
        public static void AddNewClass(Application VsApp,Window VsWindow, Tree SolutionExplorerTree, String ProjectName, String itemToAdd)
        {
            TreeNode solutionNode = SolutionExplorerTree.Nodes[0];
            
            TreeNode requiredTreeNode = null;
            foreach (TreeNode node in solutionNode.Nodes)
            {
                if (String.Compare(node.Name, ProjectName,true, CultureInfo.InvariantCulture) == 0)
                {
                    requiredTreeNode = node;
                    break;
                }
            }
            if (null == requiredTreeNode)
                throw new Exception("Project Not Found In Solution Explorer");
            requiredTreeNode.RightClick();

            //Check if the itemname passed in is of class type or other types.
            string extension = Path.GetExtension(itemToAdd);
            String[] itemType;
            String[] itemNodeType;
            if (String.Compare(extension, ".cs", true) == 0)
            {
                itemType = new String[] { "Add", "Class..." };
                itemNodeType = new String[] { "Visual C#", "Code" };
            }
            else
            {
                itemType = new String[] { "Add", "New Item..." };
                itemNodeType = new String[] { "Visual C#"};
            }

            Thread.Sleep(1000);
            Menu addNewItem = VsWindow.PopupMenu(itemType);
            addNewItem.Click();
            Window addNewItemWindow = FindWindow(VsApp,String.Format(NEWITEMTITLE,ProjectName));

            Core.UIItems.TreeItems.Tree addnewprojectTypes1 = addNewItemWindow.Get<Core.UIItems.TreeItems.Tree>(Core.UIItems.Finders.SearchCriteria.ByAutomationId("1207"));//"Project types:"));
            Core.UIItems.TreeItems.TreeNode addnewprojectTreeNode1 = addnewprojectTypes1.Node(itemNodeType);
            addnewprojectTreeNode1.Select();
            Core.UIItems.ListBoxItems.ListControl addnewlist1 = addNewItemWindow.Get<Core.UIItems.ListBoxItems.ListControl>(Core.UIItems.Finders.SearchCriteria.ByText("Templates:"));

            foreach (Core.UIItems.ListBoxItems.ListItem addnewlistItem1 in addnewlist1.Items)
            {
                if (addnewlistItem1.Text == "Class")
                {
                    addnewlistItem1.Select();
                    break;
                }

                if (addnewlistItem1.Text == "Silverlight User Control")
                {
                    addnewlistItem1.Select();
                    break;
                }

            }
            Core.UIItems.TextBox classText = addNewItemWindow.Get<Core.UIItems.TextBox>("Name:");
            classText.SetValue(itemToAdd);
            Core.UIItems.Button addnewokBtn1 = addNewItemWindow.Get<Core.UIItems.Button>("Add");
            addnewokBtn1.Click();
        }

        /// <summary>
        /// Adds New folder to the project.
        /// </summary>
        /// <param name="VsApp">Visual studio application</param>
        /// <param name="VsWindow">Visual studio IDE window</param>
        /// <param name="SolutionExplorerTree">Solution Explorer which contains the project</param>
        /// <param name="ProjectName">Project Name to which to add the folder to</param>
        /// <param name="folderName">Name of the new folder to be added</param>
        public static void AddNewFolder(Application VsApp, Window VsWindow, Tree SolutionExplorerTree, string ProjectName, string folderName)
        {
            TreeNode solutionNode = SolutionExplorerTree.Nodes[0];

            TreeNode requiredTreeNode = null;
            foreach (TreeNode node in solutionNode.Nodes)
            {
                if (String.Compare(node.Name, ProjectName, true, CultureInfo.InvariantCulture) == 0)
                {
                    requiredTreeNode = node;
                    break;
                }
            }
            if (null == requiredTreeNode)
                throw new Exception("Project Not Found In Solution Explorer");
            requiredTreeNode.RightClick();
            Menu addNewItem = VsWindow.PopupMenu(new String[] { "Add", "New Folder" });
            addNewItem.Click();
            requiredTreeNode.Click();
        }

        /// <summary>
        /// Deletes file / folder from a project
        /// </summary>
        /// <param name="VsApp">Visual studio application</param>
        /// <param name="VsWindow">Visual studio IDE window</param>
        /// <param name="SolutionExplorerTree">Solution Explorer which contains the project</param>
        /// <param name="ProjectName">Project Name from which to delete teh file from </param>
        /// <param name="itemNameToDelete">File name to delete</param>
        public static void DeleteProjectItem(Application VsApp, Window VsWindow, Tree SolutionExplorerTree, string ProjectName, string itemNameToDelete)
        {
            TreeNode solutionNode = SolutionExplorerTree.Nodes[0];

            TreeNode requiredTreeNode = null;
            TreeNode itemNode = null;

            foreach (TreeNode node in solutionNode.Nodes)
            {
                if (String.Compare(node.Name, ProjectName, true, CultureInfo.InvariantCulture) == 0)
                {
                    requiredTreeNode = node;
                    break;
                }
            }
            if (null == requiredTreeNode)
                throw new Exception("Project Not Found In Solution Explorer");

            foreach (TreeNode node in requiredTreeNode.Nodes)
            {
                if (String.Compare(node.Name, itemNameToDelete, true, CultureInfo.InvariantCulture) == 0)
                {
                    itemNode = node;
                    break;
                }
            }

            if (null == itemNode)
                throw new Exception("File / Folder Not Found In Project");
            
            //Right click Class and click on Delete
            itemNode.RightClick();
            Menu deleteItem = VsWindow.PopupMenu(new String[] {"Delete" });
            deleteItem.Click();

            //Get the handle of pop upWindow 
            Window deleteWindow = FindWindow(VsApp, SOLUTIONVIEWERDELETEWINDOW);

            Core.UIItems.Button deleteOkBtn = deleteWindow.Get<Core.UIItems.Button>("OK");
            deleteOkBtn.Click();
        }

        /// <summary>
        /// Unloads the given project from solution.
        /// </summary>
        /// <param name="vsApp">Visual Studio application.</param>
        /// <param name="VsWindow">Visual Studio window</param>
        /// <param name="solutionExplorerTree">Solution explorer tree</param>
        /// <param name="projectToUnload">Project to unload from solution</param>
        public static void UnLoadProject(Application vsApp, Window VsWindow, Tree solutionExplorerTree, string projectToUnload)
        {
            TreeNode solutionNode = solutionExplorerTree.Nodes[0];
            
            TreeNode requiredTreeNode = null;

            foreach (TreeNode node in solutionNode.Nodes)
            {
                if (String.Compare(node.Name, projectToUnload, true, CultureInfo.InvariantCulture) == 0)
                {
                    requiredTreeNode = node;
                    break;
                }
            }
            if (null == requiredTreeNode)
                throw new Exception("Project Not Found In Solution Explorer");

            requiredTreeNode.RightClick();
            Menu unloadProjectItem = VsWindow.PopupMenu(new String[] { "Unload Project" });
            unloadProjectItem.Click();

            Window unloadConfirmWindow = FindWindow(vsApp, "Microsoft Visual Studio");
            if (unloadConfirmWindow != null)
            {
                Core.UIItems.Button noButton = unloadConfirmWindow.Get<Core.UIItems.Button>(SearchCriteria.ByText("No"));
                noButton.Click();
            }
        }

        /// <summary>
        /// Creates Project Link for the specified Project
        /// </summary>
        /// <param name="SolutionTree"></param>
        /// <param name="ProjectName"></param>
        public static void CreateProjectLink(Window VsWindow,Tree SolutionExplorerTree,String ProjectName)
        {
            TreeNode solutionNode = SolutionExplorerTree.Nodes[0];
            TreeNode requiredTreeNode=null;
            foreach (TreeNode node in solutionNode.Nodes)
            {
                if (String.Compare(node.Name, ProjectName,true, CultureInfo.InvariantCulture) == 0)
                {
                    requiredTreeNode = node;
                    break;
                }
            }
            if (null == requiredTreeNode)
                throw new Exception("Project Not Found In Solution Explorer");
            requiredTreeNode.RightClick();
            Menu addNewItem = VsWindow.PopupMenu(new String[]{ADDPROJECTLINK});
            addNewItem.Click();
        }

        /// <summary>
        /// Helper method for selecting project with in Solution Picker Window
        /// </summary>
        /// <param name="SolutionPickerWindow">Solution picker window</param>
        /// <param name="SolutionName">Solution name of the project to be linked</param>
        /// <param name="ProjectNameToBeLinked">Name of the project to be linked.</param>
        public static void SelectProjectInAddProjectsLinkWindow(Window SolutionPickerWindow, String SolutionName, String ProjectNameToBeLinked)
        {
            Tree solutionTree= SolutionPickerWindow.Get<Tree>(SearchCriteria.ByAutomationId("solutionTree"));
            TreeNode requiredSolutionNode = null;
            foreach (TreeNode nodeIndex in solutionTree.Nodes)
            {
                if (nodeIndex.NameMatches(SolutionName))
                {
                    requiredSolutionNode = nodeIndex;
                    break;
                }
            }
            if (requiredSolutionNode == null)
            {
                throw new Exception("Not able to find solution Node");

            }
            TreeNode requiredProjectNode = null;
            foreach (TreeNode projectNodes in requiredSolutionNode.Nodes)
            {
                if (projectNodes.NameMatches(ProjectNameToBeLinked))
                {
                    requiredProjectNode = projectNodes;
                    break;
                }
            }
            if(requiredProjectNode==null)
            {
                throw new Exception("Not able to find project Node");

            }
           AutomationElement aeProject= requiredProjectNode.AutomationElement;
           Core.InputDevices.Mouse.Instance.Location = new System.Windows.Point((int)Math.Floor(aeProject.Current.BoundingRectangle.X), (int)Math.Floor(aeProject.Current.BoundingRectangle.Y));
           Core.InputDevices.Mouse.Instance.Click();
           Core.UIItems.Button okBtn = SolutionPickerWindow.Get<Core.UIItems.Button>(SearchCriteria.ByText("OK"));
           okBtn.Click();
        }

        /// <summary>
        /// Gets the project node for the project name.
        /// </summary>
        /// <param name="SolutionExplorerTree">Solution Explorer tree</param>
        /// <param name="ProjectName">Project name</param>
        /// <returns>Returns project node as tree node</returns>
        public static TreeNode GetProjectNode(Tree SolutionExplorerTree, String ProjectName)
        {
            TreeNode solutionNode = SolutionExplorerTree.Nodes[0];

            TreeNode requiredTreeNode = null;
            foreach (TreeNode node in solutionNode.Nodes)
            {
                if (String.Compare(node.Name, ProjectName, true, CultureInfo.InvariantCulture) == 0)
                {
                    requiredTreeNode = node;
                    break;
                }
            }
            return requiredTreeNode;
        }

        /// <summary>
        /// Verifies whether the item is part of the solution
        /// </summary>
        /// <param name="SolutionExplorerTree">Solution Explorer Tree</param>
        /// <param name="ProjectName">Parameter Name</param>
        /// <param name="entityName">Entity Name. It can be a Class name, Folder name.</param>
        /// <returns>return true if the entiry is found in the class, false otherwise</returns>
        public static bool IsEntityFound(Tree SolutionExplorerTree, string ProjectName, string entityName)
        {
            TreeNode projectNode = GetProjectNode(SolutionExplorerTree, ProjectName);
            if (null == projectNode)
                throw new Exception("Project Not Found In Solution Explorer");

            foreach (TreeNode node in projectNode.Nodes)
            {
                if (String.Compare(node.Name, entityName, true, CultureInfo.InvariantCulture) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Performs Unlink between the projects. 
        /// </summary>
        public static void PerformUnlink(Application VsApp,Window VsWindow)
        {
            Core.UIItems.MenuItems.Menu menuEditLink = VsWindow.MenuBar.MenuItem(new String[] { "Project", "Edit Links" });
            menuEditLink.Click();
            List<Core.UIItems.WindowItems.Window> windows = VsApp.GetWindows();
            Window editLinksWindow = null;
            foreach (Window win in windows)
            {
                if (win.Title == "Project Links")
                {
                    editLinksWindow = win;
                    break;
                }
            }
            if (null == editLinksWindow)
            {
                throw new Exception("Project Links window is null");
            }
            Core.UIItems.Button unlinkButton = editLinksWindow.Get<Core.UIItems.Button>(SearchCriteria.ByAutomationId("buttonUnbind"));
            unlinkButton.Click();
            Core.UIItems.Button okButton = editLinksWindow.Get<Core.UIItems.Button>(SearchCriteria.ByText("Ok"));
            okButton.Click();
        }
        
        # endregion
    }
}
