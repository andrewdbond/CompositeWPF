// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;

namespace Microsoft.Practices.Prism.MefExtensions.Regions.Behaviors
{
    /// <summary>
    /// Exports the AutoPopulateRegionBehavior using the Managed Extensibility Framework (MEF).
    /// </summary>
    /// <remarks>
    /// This allows the MefBootstrapper to provide this class as a default implementation.
    /// If another implementation is found, this export will not be used.
    /// </remarks>
    [Export(typeof(RegionMemberLifetimeBehavior))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MefRegionMemberLifetimeBehavior : RegionMemberLifetimeBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MefAutoPopulateRegionBehavior"/> class.
        /// </summary>
        [ImportingConstructor]
        public MefRegionMemberLifetimeBehavior()
        {
        }
    }
}
