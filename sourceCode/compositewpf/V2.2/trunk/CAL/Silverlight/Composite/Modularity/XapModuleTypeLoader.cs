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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Resources;
using System.Xml;

namespace Microsoft.Practices.Composite.Modularity
{
    /// <summary>
    /// Component responsible for downloading remote modules 
    /// and load their <see cref="Type"/> into the current application domain.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Xap")]
    public class XapModuleTypeLoader : IModuleTypeLoader
    {
        private readonly Dictionary<Uri, List<ModuleTypeLoaderCallbackMetadata>> typeLoadingCallbacks = new Dictionary<Uri, List<ModuleTypeLoaderCallbackMetadata>>();

        /// <summary>
        /// Evaluates the <see cref="ModuleInfo.Ref"/> property to see if the current typeloader will be able to retrieve the <paramref name="moduleInfo"/>.
        /// </summary>
        /// <param name="moduleInfo">Module that should have it's type loaded.</param>
        /// <returns><see langword="true"/> if the current typeloader is able to retrieve the module, otherwise <see langword="false"/>.</returns>
        public bool CanLoadModuleType(ModuleInfo moduleInfo)
        {
            if (!string.IsNullOrEmpty(moduleInfo.Ref))
            {
                Uri uriRef;
                return Uri.TryCreate(moduleInfo.Ref, UriKind.RelativeOrAbsolute, out uriRef);
            }

            return false;
        }

        /// <summary>
        /// Starts retrieving the <paramref name="moduleInfo"/> and calls the <paramref name="callback"/> when it is done.
        /// </summary>
        /// <param name="moduleInfo">Module that should have it's type loaded.</param>
        /// <param name="callback">Delegate to be called when typeloading process completes or fails.</param>
        public void BeginLoadModuleType(ModuleInfo moduleInfo, ModuleTypeLoadedCallback callback)
        {
            Uri uriRef = new Uri(moduleInfo.Ref, UriKind.RelativeOrAbsolute);
            lock (this.typeLoadingCallbacks)
            {
                ModuleTypeLoaderCallbackMetadata callbackMetadata = new ModuleTypeLoaderCallbackMetadata()
                                                                 {
                                                                     Callback = callback,
                                                                     ModuleInfo = moduleInfo
                                                                 };

                List<ModuleTypeLoaderCallbackMetadata> callbacks;
                if (this.typeLoadingCallbacks.TryGetValue(uriRef, out callbacks))
                {
                    callbacks.Add(callbackMetadata);
                    return;
                }

                this.typeLoadingCallbacks[uriRef] = new List<ModuleTypeLoaderCallbackMetadata> { callbackMetadata };
            }

            IFileDownloader downloader = this.CreateDownloader();
            downloader.DownloadCompleted += this.OnDownloadCompleted;

            downloader.DownloadAsync(uriRef, uriRef);
        }

        /// <summary>
        /// Creates the <see cref="IFileDownloader"/> used to retrieve the remote modules.
        /// </summary>
        /// <returns>The <see cref="IFileDownloader"/> used to retrieve the remote modules.</returns>
        protected virtual IFileDownloader CreateDownloader()
        {
            return new FileDownloader();
        }

        private static IEnumerable<AssemblyPart> GetParts(Stream stream)
        {
            List<AssemblyPart> assemblyParts = new List<AssemblyPart>();

            using (var streamReader = new StreamReader(Application.GetResourceStream(
                                                           new StreamResourceInfo(stream, null),
                                                           new Uri("AppManifest.xaml", UriKind.Relative)).Stream))
            {
                using (XmlReader xmlReader = XmlReader.Create(streamReader))
                {
                    xmlReader.MoveToContent();
                    while (xmlReader.Read())
                    {
                        if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Deployment.Parts")
                        {
                            using (XmlReader xmlReaderAssemblyParts = xmlReader.ReadSubtree())
                            {
                                while (xmlReaderAssemblyParts.Read())
                                {
                                    if (xmlReaderAssemblyParts.NodeType == XmlNodeType.Element && xmlReaderAssemblyParts.Name == "AssemblyPart")
                                    {
                                        AssemblyPart assemblyPart = new AssemblyPart();
                                        assemblyPart.Source = xmlReaderAssemblyParts.GetAttribute("Source");
                                        assemblyParts.Add(assemblyPart);
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }

            return assemblyParts;
        }

        private static void LoadAssemblyFromStream(Stream sourceStream, AssemblyPart assemblyPart)
        {
            Stream assemblyStream = Application.GetResourceStream(
                new StreamResourceInfo(sourceStream, null),
                new Uri(assemblyPart.Source, UriKind.Relative)).Stream;

            assemblyPart.Load(assemblyStream);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void OnDownloadCompleted(object o, DownloadCompletedEventArgs e)
        {
            ((IFileDownloader)o).DownloadCompleted -= this.OnDownloadCompleted;
            Exception error = e.Error;
            if (e.Error == null)
            {
                try
                {
                    Debug.Assert(!e.Cancelled, "Download should not be cancelled");
                    Stream stream = e.Result;

                    foreach (AssemblyPart part in GetParts(stream))
                    {
                        LoadAssemblyFromStream(stream, part);
                    }
                }
                catch (Exception ex)
                {
                    error = ex;
                }
                finally
                {
                    e.Result.Close();
                }
            }

            List<ModuleTypeLoaderCallbackMetadata> callbacks;
            lock (this.typeLoadingCallbacks)
            {
                Uri requestUri = (Uri)e.UserState;
                Debug.Assert(requestUri != null, "UserState should hold a reference to the request uri");
                callbacks = this.typeLoadingCallbacks[requestUri];
                this.typeLoadingCallbacks.Remove(requestUri);
            }

            foreach (ModuleTypeLoaderCallbackMetadata loaderCallbackMetadata in callbacks)
            {
                loaderCallbackMetadata.Callback(loaderCallbackMetadata.ModuleInfo, error);
            }
        }

        private class ModuleTypeLoaderCallbackMetadata
        {
            public ModuleTypeLoadedCallback Callback { get; set; }

            public ModuleInfo ModuleInfo { get; set; }
        }
    }
}
