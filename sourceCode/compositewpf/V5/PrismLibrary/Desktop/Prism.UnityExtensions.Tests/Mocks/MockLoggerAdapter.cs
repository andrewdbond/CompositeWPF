// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Practices.Prism.Logging;

namespace Microsoft.Practices.Prism.UnityExtensions.Tests.Mocks
{
    internal class MockLoggerAdapter : ILoggerFacade
    {
        public IList<string> Messages = new List<string>();

        public void Log(string message, Category category, Priority priority)
        {
            Messages.Add(message);
        }
    }
}