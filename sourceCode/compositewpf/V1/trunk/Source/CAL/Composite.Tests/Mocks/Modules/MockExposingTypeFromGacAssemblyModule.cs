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
using Microsoft.Practices.Composite.Modularity;

namespace Microsoft.Practices.Composite.Tests.Mocks.Modules
{
    public class MockExposingTypeFromGacAssemblyModule : IModule
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }

    public class SomeContractReferencingTransactionsAssembly : System.Transactions.IDtcTransaction
    {
        public void Commit(int retaining, int commitType, int reserved)
        {
            throw new System.NotImplementedException();
        }

        public void Abort(IntPtr reason, int retaining, int async)
        {
            throw new System.NotImplementedException();
        }

        public void GetTransactionInfo(IntPtr transactionInformation)
        {
            throw new System.NotImplementedException();
        }
    }
}
