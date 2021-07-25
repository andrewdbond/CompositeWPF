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
using System.Collections.Generic;
using Microsoft.Practices.Composite.Regions;

namespace StockTraderRI.Modules.Position.Tests.Mocks
{
    public class MockRegionManager : IRegionManager
    {
        private Dictionary<string, IRegion> _regions = new Dictionary<string, IRegion>();

        public IDictionary<string, IRegion> Regions
        {
            get { return _regions; }
        }

        public void AttachNewRegion(object regionTarget, string regionName)
        {
            throw new NotImplementedException();
        }

        public IRegionManager CreateRegionManager()
        {
            throw new NotImplementedException();
        }
    }

    public class MockRegion : IRegion
    {
        public List<object> AddedViews = new List<object>();

        public IRegionManager Add(object view)
        {
            AddedViews.Add(view);
            return null;
        }

        public void Remove(object view)
        {
            AddedViews.Remove(view);
        }

        public IViewsCollection Views
        {
            get { throw new NotImplementedException(); }
        }

        public void Activate(object view)
        {
            SelectedItem = view;
        }

        public void Deactivate(object view)
        {
            throw new NotImplementedException();
        }

        public IRegionManager Add(object view, string viewName)
        {
            Add(view);
            return null;
        }

        public object GetView(string viewName)
        {
            return null;
        }

        public IRegionManager Add(object view, string viewName, bool createRegionManagerScope)
        {
            throw new NotImplementedException();
        }

        public IRegionManager RegionManager
        {
            get { throw new NotImplementedException(); }         
            set { throw new NotImplementedException(); }
        }

        public object SelectedItem { get; set; }

        public IViewsCollection ActiveViews
        {
            get { throw new NotImplementedException(); }
        }
    }
}
