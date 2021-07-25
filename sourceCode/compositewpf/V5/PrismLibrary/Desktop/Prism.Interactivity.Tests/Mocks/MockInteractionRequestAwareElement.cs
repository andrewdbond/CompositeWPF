// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Practices.Prism.Interactivity.Tests.Mocks
{
    public class MockInteractionRequestAwareElement : FrameworkElement, IInteractionRequestAware
    {
        public INotification Notification { get; set; }

        public Action FinishInteraction { get; set; }
    }
}
