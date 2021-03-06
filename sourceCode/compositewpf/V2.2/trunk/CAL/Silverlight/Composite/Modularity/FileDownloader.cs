//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Net;

namespace Microsoft.Practices.Composite.Modularity
{
    /// <summary>
    /// Defines the component used to download files.
    /// </summary>
    /// <remarks>This is mainly a wrapper for the <see cref="WebClient"/> class that implements <see cref="IFileDownloader"/>.</remarks>
    public class FileDownloader : IFileDownloader
    {
        private readonly WebClient webClient = new WebClient();

        /// <summary>
        /// Event triggered when the file download is complete.
        /// </summary>
        public event EventHandler<DownloadCompletedEventArgs> DownloadCompleted
        {
            add
            {
                if (this._downloadCompleted == null)
                {
                    this.webClient.OpenReadCompleted += this.WebClient_OpenReadCompleted;
                }
                
                this._downloadCompleted += value;
            }

            remove
            {
                this._downloadCompleted -= value;
                if (this._downloadCompleted == null)
                {
                    this.webClient.OpenReadCompleted -= this.WebClient_OpenReadCompleted;
                }
            }
        }

        private event EventHandler<DownloadCompletedEventArgs> _downloadCompleted;

        /// <summary>
        /// Starts downloading asynchronously a file from <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">The location of the file to be downloaded.</param>
        /// <param name="userToken">Provides a user-specified identifier for the asynchronous task.</param>
        public void DownloadAsync(Uri uri, object userToken)
        {
            this.webClient.OpenReadAsync(uri, userToken);
        }

        private static DownloadCompletedEventArgs ConvertArgs(OpenReadCompletedEventArgs args)
        {
            return new DownloadCompletedEventArgs(args.Error == null ? args.Result : null, args.Error, args.Cancelled, args.UserState);
        }

        private void WebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            this._downloadCompleted(this, ConvertArgs(e));
        }
    }
}
