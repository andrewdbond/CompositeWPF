// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Modularity;

namespace Microsoft.Practices.Prism.Composition.Tests.Mocks
{
    public class MockConfigurationStore : IConfigurationStore
    {
        private ModulesConfigurationSection _section = new ModulesConfigurationSection();

        public ModuleConfigurationElement[] Modules
        {
            set { _section.Modules = new ModuleConfigurationElementCollection(value); }
        }

        public ModulesConfigurationSection RetrieveModuleConfigurationSection()
        {
            return _section;
        }
    }
}