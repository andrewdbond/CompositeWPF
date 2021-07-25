// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Practices.Prism.Interactivity.Tests.Mocks
{
    public class MockFrameworkElement : FrameworkElement
    {
        public void RaiseLoaded()
        {
            this.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
        }

        public void RaiseUnloaded()
        {
            this.RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent));
        }
    }
}
