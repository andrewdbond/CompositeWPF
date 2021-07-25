// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Windows;
using Microsoft.Practices.Prism.Regions;

namespace Microsoft.Practices.Prism.Composition.Tests.Mocks
{
    internal class MockRegionManagerAccessor : IRegionManagerAccessor
    {
        public Func<DependencyObject, string> GetRegionName;
        public Func<DependencyObject, IRegionManager> GetRegionManager;

        public event EventHandler UpdatingRegions;

        string IRegionManagerAccessor.GetRegionName(DependencyObject element)
        {
            return this.GetRegionName(element);
        }

        IRegionManager IRegionManagerAccessor.GetRegionManager(DependencyObject element)
        {
            if (this.GetRegionManager != null)
            {
                return this.GetRegionManager(element);
            }

            return null;
        }

        public void UpdateRegions()
        {
            if (this.UpdatingRegions != null)
            {
                this.UpdatingRegions(this, EventArgs.Empty);
            }
        }

        public int GetSubscribersCount()
        {
            return this.UpdatingRegions != null ? this.UpdatingRegions.GetInvocationList().Length : 0;
        }
    }
}