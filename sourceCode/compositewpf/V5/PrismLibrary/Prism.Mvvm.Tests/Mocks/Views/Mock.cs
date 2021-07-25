// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Practices.Prism.Mvvm.Tests.Mocks.Views
{
    public class Mock : IView
    {
        public object DataContext { get; set; }
    }
}
