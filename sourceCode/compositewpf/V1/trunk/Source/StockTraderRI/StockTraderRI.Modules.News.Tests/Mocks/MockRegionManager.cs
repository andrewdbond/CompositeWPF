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

namespace StockTraderRI.Modules.News.Tests.Mocks
{
    public class MockRegionManager : IRegionManager
    {
        private IDictionary<string, IRegion> _regions = new Dictionary<string, IRegion>();

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

    public class MockNewsRegion : IRegion
    {
        public object GetViewReturnValue;
        public string GetViewArgumentName;
        public object ShowArgumentView;
        public string AddArgumentName;
        public IList<object> AddedViews = new List<object>();

        #region IRegion Members

        public IRegionManager Add(object view)
        {
            AddedViews.Add(view);
            return null;
        }

        public void Remove(object view)
        {
            throw new NotImplementedException();
        }

        public IViewsCollection Views
        {
            get { throw new NotImplementedException(); }
        }

        public void Activate(object view)
        {
            ShowArgumentView = view;
        }

        public void Deactivate(object view)
        {
            throw new NotImplementedException();
        }

        public IRegionManager Add(object view, string viewName)
        {
            AddArgumentName = viewName;
            Add(view);
            return null;
        }

        public object GetView(string viewName)
        {
            GetViewArgumentName = viewName;
            return GetViewReturnValue;
        }

        public IRegionManager Add(object view, string viewName, bool createRegionManagerScope)
        {
            throw new NotImplementedException();
        }

        public IRegionManager RegionManager
        {
            set { throw new NotImplementedException(); }
            get { throw new NotImplementedException();}
        }

        public IViewsCollection ActiveViews
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}