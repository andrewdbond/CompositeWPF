// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.Prism.TestSupport
{
    public class CollectionChangedTracker
    {
        private readonly List<NotifyCollectionChangedEventArgs> eventList = new List<NotifyCollectionChangedEventArgs>();

        public CollectionChangedTracker(INotifyCollectionChanged collection)
        {
            collection.CollectionChanged += OnCollectionChanged;
        }

        public IEnumerable<NotifyCollectionChangedAction> ActionsFired { get { return this.eventList.Select(e => e.Action);  } }
        public IEnumerable<NotifyCollectionChangedEventArgs> NotifyEvents { get { return this.eventList; } }
        
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.eventList.Add(e);
        }
    }
}
