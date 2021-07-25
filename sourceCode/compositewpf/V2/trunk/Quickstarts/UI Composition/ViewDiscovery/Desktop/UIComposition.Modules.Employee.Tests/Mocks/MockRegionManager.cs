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
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Regions;

namespace UIComposition.Modules.Employee.Tests.Mocks
{
    public class MockRegionManager : IRegionManager
    {
        public readonly MockRegionCollection MockRegionCollection = new MockRegionCollection();

        #region IRegionManager Members

        public IRegionManager CreateRegionManager()
        {
            throw new NotImplementedException();
        }

        public IRegionCollection Regions
        {
            get { return MockRegionCollection; }
        }

        public IRegion AttachNewRegion(object objectToAdapt, string regionName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class MockRegionCollection : IRegionCollection
    {
        public Dictionary<string, IRegion> regions = new Dictionary<string, IRegion>();

        public IEnumerator<IRegion> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IRegion this[string regionName]
        {
            get { return regions[regionName]; }
        }

        public void Add(IRegion region)
        {
            regions[region.Name] = region;
        }

        public bool Remove(string regionName)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsRegionWithName(string regionName)
        {
            throw new System.NotImplementedException();
        }
    }
}
