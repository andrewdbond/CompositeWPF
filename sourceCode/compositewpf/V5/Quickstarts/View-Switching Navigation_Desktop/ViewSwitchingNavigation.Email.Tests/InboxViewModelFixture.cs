// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ViewSwitchingNavigation.Email.Model;
using ViewSwitchingNavigation.Email.ViewModels;
using ViewSwitchingNavigation.Infrastructure;
using System.Threading;
using System.Windows.Data;

namespace ViewSwitchingNavigation.Email.Tests
{
    [TestClass]
    public class InboxViewModelFixture
    {
        [TestMethod]
        public void WhenCreated_ThenRequestsEmailsToService()
        {
            var emailServiceMock = new Mock<IEmailService>();
            var requested = false;
            emailServiceMock
                .Setup(svc => svc.BeginGetEmailDocuments(It.IsAny<AsyncCallback>(), null))
                .Callback(() => requested = true);

            var viewModel = new InboxViewModel(emailServiceMock.Object, new Mock<IRegionManager>().Object);

            Assert.IsTrue(requested);
        }

        [TestMethod]
        public void WhenExecutingTheNewCommand_ThenNavigatesToComposeEmailView()
        {
            var emailServiceMock = new Mock<IEmailService>();

            Mock<IRegion> regionMock = new Mock<IRegion>();
            regionMock
                .Setup(x => x.RequestNavigate(new Uri("ComposeEmailView", UriKind.Relative), It.IsAny<Action<NavigationResult>>()))
                .Callback<Uri, Action<NavigationResult>>((s, c) => c(new NavigationResult(null, true)))
                .Verifiable();

            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.Regions.ContainsRegionWithName(RegionNames.MainContentRegion)).Returns(true);
            regionManagerMock.Setup(x => x.Regions[RegionNames.MainContentRegion]).Returns(regionMock.Object);

            var viewModel = new InboxViewModel(emailServiceMock.Object, regionManagerMock.Object);

            viewModel.ComposeMessageCommand.Execute(null);

            regionMock.VerifyAll();
        }

        [TestMethod]
        public void WhenExecutingTheReplyCommand_ThenNavigatesToComposeEmailViewWithId()
        {
            var email = new EmailDocument();

            var emailServiceMock = new Mock<IEmailService>();
            AsyncCallback callback = null;
            var asyncResultMock = new Mock<IAsyncResult>();
            emailServiceMock
                .Setup(svc => svc.BeginGetEmailDocuments(It.IsAny<AsyncCallback>(), null))
                .Callback<AsyncCallback, object>((ac, o) => callback = ac)
                .Returns(asyncResultMock.Object);
            emailServiceMock
                .Setup(svc => svc.EndGetEmailDocuments(asyncResultMock.Object))
                .Returns(new[] { email });

            Mock<IRegion> regionMock = new Mock<IRegion>();
            regionMock
                .Setup(x => x.RequestNavigate(new Uri(@"ComposeEmailView?ReplyTo=" + email.Id.ToString("N"), UriKind.Relative), It.IsAny<Action<NavigationResult>>()))
                .Callback<Uri, Action<NavigationResult>>((s, c) => c(new NavigationResult(null, true)))
                .Verifiable();

            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.Regions.ContainsRegionWithName(RegionNames.MainContentRegion)).Returns(true);
            regionManagerMock.Setup(x => x.Regions[RegionNames.MainContentRegion]).Returns(regionMock.Object);

            EmailDocument[] emails = new EmailDocument[]{ email };
            var viewModel = new TestInboxViewModel(emailServiceMock.Object, regionManagerMock.Object, emails);

            Assert.IsFalse(viewModel.Messages.IsEmpty);

            viewModel.Messages.MoveCurrentToFirst();

            viewModel.ReplyMessageCommand.Execute(null);

            regionMock.VerifyAll();
        }

        [TestMethod]
        public void WhenExecutingTheOpenCommand_ThenNavigatesToEmailView()
        {
            var email = new EmailDocument();

            var emailServiceMock = new Mock<IEmailService>();

            Mock<IRegion> regionMock = new Mock<IRegion>();
            regionMock
                .Setup(x => x.RequestNavigate(new Uri(@"EmailView?EmailId=" + email.Id.ToString("N"), UriKind.Relative), It.IsAny<Action<NavigationResult>>()))
                .Callback<Uri, Action<NavigationResult>>((s, c) => c(new NavigationResult(null, true)))
                .Verifiable();

            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.Regions.ContainsRegionWithName(RegionNames.MainContentRegion)).Returns(true);
            regionManagerMock.Setup(x => x.Regions[RegionNames.MainContentRegion]).Returns(regionMock.Object);

            var viewModel = new InboxViewModel(emailServiceMock.Object, regionManagerMock.Object);

            viewModel.OpenMessageCommand.Execute(email);

            regionMock.VerifyAll();
        }
    }

    class TestInboxViewModel : InboxViewModel
    {
        public TestInboxViewModel(IEmailService emailService, IRegionManager regionManager, EmailDocument[] emails) :
            base(emailService, regionManager)
        {
            var viewCollection = this.Messages as ListCollectionView;

            foreach (var email in emails)
            {
                viewCollection.AddNewItem(email);
            }

            viewCollection.MoveCurrentTo(null);
        }
    }
}
