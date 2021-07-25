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

namespace AcceptanceTestLibrary.VSHelper
{
    /// <summary>
    /// Helper Class for uncompressing mock solution template to the given destination path.
    /// </summary>
    public class UnCompressHelper
    {
        /// <summary>
        /// Helper method for performing uncompression
        /// </summary>
        /// <param name="compressedFileName">Compressed file name</param>
        /// <param name="destination">destination to be extracted</param>
        public static void UnCompressZip(string compressedFileName, string destination)
        {
            Shell32.ShellClass sc = new Shell32.ShellClass();
            Shell32.Folder SrcFlder = sc.NameSpace(compressedFileName);
            Shell32.Folder DestFlder = sc.NameSpace(destination);
            Shell32.FolderItems items = SrcFlder.Items();
            DestFlder.CopyHere(items, 10);
        }
    }
}
