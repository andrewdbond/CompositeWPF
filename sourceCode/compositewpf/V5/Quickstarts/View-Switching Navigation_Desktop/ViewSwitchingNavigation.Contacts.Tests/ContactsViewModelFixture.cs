// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ViewSwitchingNavigation.Contacts.Model;
using ViewSwitchingNavigation.Contacts.ViewModels;
using ViewSwitchingNavigation.Infrastructure;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace ViewSwitchingNavigation.Contacts.Tests
{
    [TestClass]
    public class ContactsViewModelFixture
    {
        [TestMethod]
        public void WhenCreated_ThenRequestsContacts()
        {
            var contactsServiceMock = new Mock<IContactsService>();
            var regionManager = new RegionManager();

            var viewModel = new ContactsViewModel(contactsServiceMock.Object, regionManager);

            contactsServiceMock.Verify(svc => svc.BeginGetContacts(It.IsAny<AsyncCallback>(), null));
            Assert.IsTrue(viewModel.Contacts.IsEmpty);
        }

        [TestMethod]
        public void WhenContactIsSelected_ThenEmailContactCommandIsEnabledAndNotifiesChange()
        {
            var contactsServiceMock = new Mock<IContactsService>();
            AsyncCallback callback = null;
            var resultMock = new Mock<IAsyncResult>();
            contactsServiceMock
                .Setup(svc => svc.BeginGetContacts(It.IsAny<AsyncCallback>(), null))
                .Callback<AsyncCallback, object>((cb, s) => callback = cb)
                .Returns(resultMock.Object);
            var contacts = new[] { new Contact { }, new Contact { } };
            contactsServiceMock
                .Setup(svc => svc.EndGetContacts(resultMock.Object))
                .Returns(contacts);

            var regionManager = new RegionManager();

            var viewModel = new TestContactsViewModel(contactsServiceMock.Object, regionManager, contacts);

            var notified = false;
            viewModel.EmailContactCommand.CanExecuteChanged += (s, o) => notified = true;
            Assert.IsFalse(viewModel.EmailContactCommand.CanExecute(null));
            viewModel.Contacts.MoveCurrentToFirst();
            Assert.IsTrue(viewModel.EmailContactCommand.CanExecute(null));
            Assert.IsTrue(notified);
        }

        [TestMethod]
        public void WhenSendingEmail_ThenNavigatesWithAToQueryParameter()
        {
            var contactsServiceMock = new Mock<IContactsService>();
            AsyncCallback callback = null;
            var resultMock = new Mock<IAsyncResult>();
            contactsServiceMock
                .Setup(svc => svc.BeginGetContacts(It.IsAny<AsyncCallback>(), null))
                .Callback<AsyncCallback, object>((cb, s) => callback = cb)
                .Returns(resultMock.Object);
            var contacts = new[] { new Contact { EmailAddress = "email" }, new Contact { } };
            contactsServiceMock
                .Setup(svc => svc.EndGetContacts(resultMock.Object))
                .Returns(contacts);

            Mock<IRegion> regionMock = new Mock<IRegion>();
            regionMock
                .Setup(x => x.RequestNavigate(new Uri("ComposeEmailView?To=email", UriKind.Relative), It.IsAny<Action<NavigationResult>>()))
                .Callback<Uri, Action<NavigationResult>>((s, c) => c(new NavigationResult(null, true)))
                .Verifiable();

            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.Regions.ContainsRegionWithName(RegionNames.MainContentRegion)).Returns(true);
            regionManagerMock.Setup(x => x.Regions[RegionNames.MainContentRegion]).Returns(regionMock.Object);

            IRegionManager regionManager = regionManagerMock.Object;

            var viewModel = new TestContactsViewModel(contactsServiceMock.Object, regionManager, contacts);

            viewModel.Contacts.MoveCurrentToFirst();

            viewModel.EmailContactCommand.Execute(null);

            regionMock.VerifyAll();
        }
    }

    class TestContactsViewModel : ContactsViewModel
    {
        public TestContactsViewModel(IContactsService contactsService, IRegionManager regionManager, Contact[] contacts) : 
            base(contactsService, regionManager)
        {
            var viewCollection = this.Contacts as ListCollectionView;

            foreach(var contact in contacts)
            {
                viewCollection.AddNewItem(contact);
            }

            viewCollection.MoveCurrentTo(null);
        }
    }
}
