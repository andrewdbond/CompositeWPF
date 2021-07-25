// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ViewSwitchingNavigation.Calendar.Model;
using ViewSwitchingNavigation.Calendar.ViewModels;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Calendar.Tests
{
    [TestClass]
    public class CalendarViewModelFixture
    {
        [TestMethod]
        public void WhenCreated_ThenRequestsMeetingsToService()
        {
            var calendarServiceMock = new Mock<ICalendarService>();
            var requested = false;
            calendarServiceMock
                .Setup(svc => svc.BeginGetMeetings(It.IsAny<AsyncCallback>(), null))
                .Callback(() => requested = true);

            var viewModel = new CalendarViewModel(calendarServiceMock.Object, new Mock<IRegionManager>().Object);

            Assert.IsTrue(requested);
        }

        [TestMethod]
        public void WhenExecutingTheGotToEmailCommand_ThenNavigatesToEmailView()
        {
            var meeting = new Meeting { EmailId = Guid.NewGuid() };

            var calendarServiceMock = new Mock<ICalendarService>();

            Mock<IRegion> regionMock = new Mock<IRegion>();
            regionMock
                .Setup(x => x.RequestNavigate(new Uri(@"EmailView?EmailId=" + meeting.EmailId.ToString("N"), UriKind.Relative), It.IsAny<Action<NavigationResult>>()))
                .Callback<Uri, Action<NavigationResult>>((s, c) => c(new NavigationResult(null, true)))
                .Verifiable();

            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.Regions.ContainsRegionWithName(RegionNames.MainContentRegion)).Returns(true);
            regionManagerMock.Setup(x => x.Regions[RegionNames.MainContentRegion]).Returns(regionMock.Object);

            var viewModel = new CalendarViewModel(calendarServiceMock.Object, regionManagerMock.Object);

            viewModel.OpenMeetingEmailCommand.Execute(meeting);

            regionMock.VerifyAll();
        }
    }
}
