// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.Prism.Composition.Tests.Regions
{
    [TestClass]
    public class NavigationAsyncExtensionsFixture
    {
        [TestMethod]
        public void WhenNavigatingWithANullThis_ThenThrows()
        {
            INavigateAsync navigate = null;
            string target = "";

            ExceptionAssert.Throws<ArgumentNullException>(
                () =>
                {
                    navigate.RequestNavigate(target);
                });
        }

        [TestMethod]
        public void WhenNavigatingWithANullStringTarget_ThenThrows()
        {
            INavigateAsync navigate = new Mock<INavigateAsync>().Object;
            string target = null;

            ExceptionAssert.Throws<ArgumentNullException>(
                () =>
                {
                    navigate.RequestNavigate(target);
                });
        }

        [TestMethod]
        public void WhenNavigatingWithARelativeStringTarget_ThenNavigatesToRelativeUri()
        {
            var navigateMock = new Mock<INavigateAsync>();
            navigateMock
                .Setup(nv =>
                    nv.RequestNavigate(
                        It.Is<Uri>(u => !u.IsAbsoluteUri && u.OriginalString == "relative"),
                        It.Is<Action<NavigationResult>>(c => c != null)))
                .Verifiable();

            string target = "relative";

            navigateMock.Object.RequestNavigate(target);

            navigateMock.VerifyAll();
        }

        [TestMethod]
        public void WhenNavigatingWithAnAbsoluteStringTarget_ThenNavigatesToAbsoluteUri()
        {
            var navigateMock = new Mock<INavigateAsync>();
            navigateMock
                .Setup(nv =>
                    nv.RequestNavigate(
                        It.Is<Uri>(u => u.IsAbsoluteUri && u.Host == "test" && u.AbsolutePath == "/path"),
                        It.Is<Action<NavigationResult>>(c => c != null)))
                .Verifiable();

            string target = "http://test/path";

            navigateMock.Object.RequestNavigate(target);

            navigateMock.VerifyAll();
        }

        [TestMethod]
        public void WhenNavigatingWithANullThisAndAUri_ThenThrows()
        {
            INavigateAsync navigate = null;
            Uri target = new Uri("test", UriKind.Relative);

            ExceptionAssert.Throws<ArgumentNullException>(
                () =>
                {
                    navigate.RequestNavigate(target);
                });
        }

        [TestMethod]
        public void WhenNavigatingWithAUri_ThenNavigatesToUriWithCallback()
        {
            Uri target = new Uri("relative", UriKind.Relative);

            var navigateMock = new Mock<INavigateAsync>();
            navigateMock
                .Setup(nv =>
                    nv.RequestNavigate(
                        target,
                        It.Is<Action<NavigationResult>>(c => c != null)))
                .Verifiable();


            navigateMock.Object.RequestNavigate(target);

            navigateMock.VerifyAll();
        }
    }
}
