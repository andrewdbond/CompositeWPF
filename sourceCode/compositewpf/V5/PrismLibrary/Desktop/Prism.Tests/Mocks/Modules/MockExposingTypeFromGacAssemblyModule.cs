// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.Prism.Modularity;

namespace Microsoft.Practices.Prism.Composition.Tests.Mocks.Modules
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
