// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Logging;

namespace Microsoft.Practices.Prism.Composition.Tests.Mocks
{
    internal class MockLogger : ILoggerFacade
    {
        public string LastMessage;
        public Category LastMessageCategory;
        public void Log(string message, Category category, Priority priority)
        {
            LastMessage = message;
            LastMessageCategory = category;
        }
    }
}