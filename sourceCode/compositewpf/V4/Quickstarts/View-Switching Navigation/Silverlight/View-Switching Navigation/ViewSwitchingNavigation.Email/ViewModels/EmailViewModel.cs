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
using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using ViewSwitchingNavigation.Email.Model;

namespace ViewSwitchingNavigation.Email.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class EmailViewModel : NotificationObject, INavigationAware
    {
        private readonly DelegateCommand goBackCommand;
        private readonly IEmailService emailService;
        private EmailDocument email;
        private IRegionNavigationJournal navigationJournal;
        private const string EmailIdKey = "EmailId";

        [ImportingConstructor]
        public EmailViewModel(IEmailService emailService)
        {
            this.goBackCommand = new DelegateCommand(this.GoBack);

            this.emailService = emailService;
        }

        public ICommand GoBackCommand
        {
            get { return this.goBackCommand; }
        }

        public EmailDocument Email
        {
            get
            {
                return this.email;
            }

            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    this.RaisePropertyChanged(() => this.Email);
                }
            }
        }


        private void GoBack()
        {
            // todo: 15 - Using the journal to navigate back.
            //
            // This view model offers a GoBack command and uses
            // the journal captured in OnNavigatedTo to
            // navigate back to the prior view.
            if (this.navigationJournal != null)
            {
                this.navigationJournal.GoBack();
            }
        }

        private Guid? GetRequestedEmailId(NavigationContext navigationContext)
        {
            var email = navigationContext.Parameters[EmailIdKey];
            Guid emailId;
            if (email != null && Guid.TryParse(email, out emailId))
            {
                return emailId;
            }

            return null;
        }

        #region INavigationAware

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            // todo: 13 - Determining if a view or view model handles the navigation request
            //
            // By implementing IsNavigationTarget, this view model can let the region
            // navigation service know that it is the item sought for navigation. 
            // 
            // If this view model is the one that was built to display the requested
            // EmailId (as a result of a prior navigation request), then this
            // method will return true.  
            //
            // Otherwise, it will return false and if no other EmailViewModel type returns true 
            // then the navigation service knows that no EmailView is already available that 
            // shows that email.  In this case, the navigation service will request a new one 
            // be constructed and added to the region.
            if (this.email == null)
            {
                return true;
            }

            var requestedEmailId = GetRequestedEmailId(navigationContext);

            return requestedEmailId.HasValue && requestedEmailId.Value == this.email.Id;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {
            // Intentionally not implemented.
        }

        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            // todo: 15 - Orient to the right context
            //
            // When this view model is navigated to, it gathers the
            // requested EmailId from the navigation context's parameters.
            //
            // It also captures the navigation Journal so it
            // can offer a 'go back' command.
            var emailId = GetRequestedEmailId(navigationContext);
            if (emailId.HasValue)
            {
                this.Email = this.emailService.GetEmailDocument(emailId.Value);
            }

            this.navigationJournal = navigationContext.NavigationService.Journal;
        }

        #endregion
    }
}
