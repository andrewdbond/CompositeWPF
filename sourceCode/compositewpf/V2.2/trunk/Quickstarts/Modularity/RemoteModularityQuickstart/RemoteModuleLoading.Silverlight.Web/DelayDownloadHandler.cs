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
using System.Threading;
using System.Web;

namespace RemoteModuleLoading.Silverlight.Web
{
    /// <summary>
    /// This <see cref="IHttpHandler"/> is used to delay the download of ModuleZ and simulate network latency
    /// </summary>
    public class DelayDownloadHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string path = context.Server.MapPath(context.Request.Path);
            if (context.Request.Path.Contains("ModuleZ"))
            {
                // Sleep to simulate network latency
                Thread.Sleep(2000);
            }

            context.Response.WriteFile(path);
        }
    }
}
