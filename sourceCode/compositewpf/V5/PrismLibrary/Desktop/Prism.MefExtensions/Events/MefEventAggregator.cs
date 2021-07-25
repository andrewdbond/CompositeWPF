// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.PubSubEvents;

namespace Microsoft.Practices.Prism.MefExtensions.Events
{
    /// <summary>
    /// Exports the EventAggregator using the Managed Extensibility Framework (MEF).
    /// </summary>
    /// <remarks>
    /// This allows the MefBootstrapper to provide this class as a default implementation.
    /// If another implementation is found, this export will not be used.
    /// </remarks>
    [Export(typeof(IEventAggregator))]
    public class MefEventAggregator : EventAggregator
    {
    }
}