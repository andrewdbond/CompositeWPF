// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.Prism.Regions;

namespace Microsoft.Practices.Prism.UnityExtensions.Tests.Mocks
{
    class MockRegionManager : IRegionManager
    {
        #region IRegionManager Members

        public IRegionCollection Regions
        {
            get { throw new NotImplementedException(); }
        }

        public IRegionManager CreateRegionManager()
        {
            throw new NotImplementedException();
        }

        #endregion

        public bool Navigate(Uri source)
        {
            throw new NotImplementedException();
        }
    }
}
