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
using System.IO.IsolatedStorage;
using System.Text;
using Microsoft.Silverlight.Testing.Harness;

namespace Microsoft.Practices.Prism.TestSupport
{
    public class FileLogProvider : LogProvider
    {
        public const string TESTRESULTFILENAME = @"TestResults\testresults.txt";
        protected override void ProcessRemainder(LogMessage message)
        {
            base.ProcessRemainder(message);
            AppendToFile(message);
        }

        public override void Process(LogMessage logMessage)
        {
            AppendToFile(logMessage);
        }

        private void AppendToFile(LogMessage logMessage)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            var carriageReturnBytes = encoding.GetBytes(new[] { '\r', '\n' });

            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.DirectoryExists("TestResults"))
                {
                    store.CreateDirectory("TestResults");
                }
                using (IsolatedStorageFileStream isoStream =
                    store.OpenFile(TESTRESULTFILENAME, FileMode.Append))
                {
                    var byteArray = encoding.GetBytes(logMessage.Message);
                    isoStream.Write(byteArray, 0, byteArray.Length);
                    isoStream.Write(carriageReturnBytes, 0, carriageReturnBytes.Length);
                }
            }
        }
    }
}
