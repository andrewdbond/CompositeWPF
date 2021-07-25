// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Windows.Controls.Primitives;

namespace Microsoft.Practices.Prism.Composition.Tests.Mocks
{
    internal class MockClickableObject : ButtonBase
    {
        public void RaiseClick()
        {
            OnClick();
        }
    }
}