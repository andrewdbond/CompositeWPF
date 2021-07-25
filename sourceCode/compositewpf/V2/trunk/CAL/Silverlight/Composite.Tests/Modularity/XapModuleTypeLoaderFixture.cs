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
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Tests.Modularity
{
    [TestClass]
    public class XapModuleTypeLoaderFixture
    {
        [TestMethod]
        public void ShouldCallDownloadAsync()
        {
            var mockFileDownloader = new MockFileDownloader();
            var remoteModuleUri = "http://MyModule.xap";
            var moduleInfo = new ModuleInfo() { Ref = remoteModuleUri };
            XapModuleTypeLoader typeLoader = new TestableXapModuleTypeLoader(mockFileDownloader);

            typeLoader.BeginLoadModuleType(moduleInfo, delegate { });

            Assert.IsTrue(mockFileDownloader.DownloadAsyncCalled);
            Assert.AreEqual(remoteModuleUri, mockFileDownloader.downloadAsyncArgumentUri.OriginalString);
        }

        [TestMethod]
        public void ShouldReturnErrorIfDownloadFails()
        {
            var mockFileDownloader = new MockFileDownloader();
            var remoteModuleUri = "http://MyModule.xap";
            var moduleInfo = new ModuleInfo() { Ref = remoteModuleUri };
            XapModuleTypeLoader typeLoader = new TestableXapModuleTypeLoader(mockFileDownloader);
            ManualResetEvent callbackEvent = new ManualResetEvent(false);
            Exception error = null;
            typeLoader.BeginLoadModuleType(
                moduleInfo,
                delegate(ModuleInfo callbackModuleInfo, Exception moduleError)
                {
                    error = moduleError;
                    callbackEvent.Set();
                });
            mockFileDownloader.InvokeOpenReadCompleted(new DownloadCompletedEventArgs(null, new InvalidOperationException("Mock Ex"), false, mockFileDownloader.DownloadAsyncArgumentUserToken));
            Assert.IsTrue(callbackEvent.WaitOne(500));

            Assert.IsNotNull(error);
            Assert.IsInstanceOfType(error, typeof(InvalidOperationException));
        }

        [TestMethod]
        public void ShouldLoadDownloadedAssemblies()
        {
            // NOTE: This test method uses a resource that is built in a pre-build event when building the project. The resource is a
            // dynamically generated XAP file that is built with the Mocks/Modules/createXap.bat batch file.
            // If this test is failing unexpectedly, it may be that the batch file is not working correctly in the current environment.

            var mockFileDownloader = new MockFileDownloader();
            const string moduleTypeName = "Microsoft.Practices.Composite.Tests.Mocks.Modules.RemoteModule, RemoteModuleA, Version=0.0.0.0";
            var remoteModuleUri = "http://MyModule.xap";
            var moduleInfo = new ModuleInfo() { Ref = remoteModuleUri };
            XapModuleTypeLoader typeLoader = new TestableXapModuleTypeLoader(mockFileDownloader);
            ManualResetEvent callbackEvent = new ManualResetEvent(false);
            typeLoader.BeginLoadModuleType(
                moduleInfo,
                delegate
                {
                    callbackEvent.Set();
                });

            Assert.IsNull(Type.GetType(moduleTypeName));

            Stream xapStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Microsoft.Practices.Composite.Tests.Mocks.Modules.RemoteModules.xap");
            mockFileDownloader.InvokeOpenReadCompleted(new DownloadCompletedEventArgs(xapStream, null, false, mockFileDownloader.DownloadAsyncArgumentUserToken));
            Assert.IsTrue(callbackEvent.WaitOne(500));

            Assert.IsNotNull(Type.GetType(moduleTypeName));
        }

        [TestMethod]
        public void ShouldDownloadOnlyOnceIfRetrievingTwoGroupsFromSameUri()
        {
            var mockFileDownloader = new MockFileDownloader();
            var remoteModuleUri = "http://MyPackage.xap";
            var moduleInfo1 = new ModuleInfo() { Ref = remoteModuleUri };
            var moduleInfo2 = new ModuleInfo() { Ref = remoteModuleUri };
            XapModuleTypeLoader typeLoader = new TestableXapModuleTypeLoader(mockFileDownloader);

            typeLoader.BeginLoadModuleType(moduleInfo1, delegate { });
            mockFileDownloader.DownloadAsyncCalled = false;
            typeLoader.BeginLoadModuleType(moduleInfo2, delegate { });

            Assert.IsFalse(mockFileDownloader.DownloadAsyncCalled);
        }

        [TestMethod]
        public void ShouldFireAllCallbacksCorrespondingToSameUri()
        {
            var mockFileDownloader = new MockFileDownloader();
            var remoteModuleUri = "http://MyPackage.xap";
            var moduleInfo1 = new ModuleInfo() { Ref = remoteModuleUri };
            var moduleInfo2 = new ModuleInfo() { Ref = remoteModuleUri };

            XapModuleTypeLoader typeLoader = new TestableXapModuleTypeLoader(mockFileDownloader);
            bool callback1Called = false;
            bool callback2Called = false;
            ModuleInfo callbackModuleInfo1 = null;
            typeLoader.BeginLoadModuleType(moduleInfo1, (m, e) =>
                                                           {
                                                               callbackModuleInfo1 = m;
                                                               callback1Called = true;
                                                           });
            ModuleInfo callbackModuleInfo2 = null;
            typeLoader.BeginLoadModuleType(moduleInfo2, (m, e) =>
                                                           {
                                                               callbackModuleInfo2 = m;
                                                               callback2Called = true;
                                                           });
            mockFileDownloader.InvokeOpenReadCompleted(new DownloadCompletedEventArgs(null, new Exception("Mock Ex"), false, mockFileDownloader.DownloadAsyncArgumentUserToken));

            Assert.IsTrue(callback1Called);
            Assert.IsTrue(callback2Called);
            Assert.AreSame(moduleInfo1, callbackModuleInfo1);
            Assert.AreSame(moduleInfo2, callbackModuleInfo2);
        }
    }

    internal class TestableXapModuleTypeLoader : XapModuleTypeLoader
    {
        public IFileDownloader FileDownloader;

        public TestableXapModuleTypeLoader(IFileDownloader downloader)
        {
            this.FileDownloader = downloader;
        }

        protected override IFileDownloader CreateDownloader()
        {
            return this.FileDownloader;
        }
    }

    internal class MockFileDownloader : IFileDownloader
    {
        public bool DownloadAsyncCalled;
        public Uri downloadAsyncArgumentUri;
        public object DownloadAsyncArgumentUserToken;

        public event EventHandler<DownloadCompletedEventArgs> DownloadCompleted;

        public void DownloadAsync(Uri uri, object userToken)
        {
            DownloadAsyncCalled = true;
            this.downloadAsyncArgumentUri = uri;
            DownloadAsyncArgumentUserToken = userToken;
        }

        public void InvokeOpenReadCompleted(DownloadCompletedEventArgs args)
        {
            DownloadCompleted.Invoke(this, args);
        }
    }
}
