//===============================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.Windows;

namespace Microsoft.Practices.Composite.Wpf.Tests.Events
{
    public class WPFThreadHelper
    {
        /// <summary>
        /// Process messages in current thread's message pump
        /// </summary>
        public static void DoEvents()
        {
            //DispatcherFrame frame = new DispatcherFrame();
            ////Dispatcher.CurrentDispatcher.BeginInvoke(
            //Application.Current.RootVisual.Dispatcher.BeginInvoke(
            //    //DispatcherPriority.Background,
            //    (SendOrPostCallback)delegate(object arg)
            //                            {
            //                                ((DispatcherFrame)arg).Continue = false;
            //                            },
            //    frame
            //    );
            //Dispatcher.PushFrame(frame);
        }
    }
}