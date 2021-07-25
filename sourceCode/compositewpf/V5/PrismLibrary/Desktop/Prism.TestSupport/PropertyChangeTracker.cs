// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.Prism.TestSupport
{
    public class PropertyChangeTracker
    {
        private INotifyPropertyChanged changer;
        private System.Collections.Generic.List<string> notifications = new List<string>();

        public PropertyChangeTracker(INotifyPropertyChanged changer)
        {
            this.changer = changer;
            changer.PropertyChanged += (o, e) => { this.notifications.Add(e.PropertyName); };
        }

        /// <summary>
        /// Returns the changed properties in order fired.
        /// </summary>
        /// <remarks>
        /// Returns string[] since this will often be used with CollectionAssert() which
        /// does not work well with IEnumerable<>.
        /// </remarks>
        public string[] ChangedProperties
        {
            get { return this.notifications.ToArray(); }
        }

        public void Reset()
        {
            this.notifications.Clear();
        }
    }
}
