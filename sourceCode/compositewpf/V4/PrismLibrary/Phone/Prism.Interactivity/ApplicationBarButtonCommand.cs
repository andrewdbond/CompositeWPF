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
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Microsoft.Practices.Prism.Interactivity
{
    /// <summary>
    /// Associates a command to an <see cref="ApplicationBarIconButton"/>.
    /// </summary>
    [CLSCompliant(false)]
    public class ApplicationBarButtonCommand : Behavior<PhoneApplicationPage>
    {
        ///<summary>
        /// The parameter to use when calling methods on the <see cref="ICommand"/> interface.
        ///</summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter",
                                        typeof(string),
                                        typeof(ApplicationBarButtonCommand),
                                        new PropertyMetadata(HandleCommandChanged));

        /// <summary>
        /// The binding for <see cref="ICommand"/> to invoke based on the ApplicationBarIconButton's events.
        /// </summary>
        public static readonly DependencyProperty CommandBindingProperty =
            DependencyProperty.Register("CommandBinding",
                                        typeof(ICommand),
                                        typeof(ApplicationBarButtonCommand),
                                        new PropertyMetadata(HandleCommandChanged));

        private ClickCommandBinding binding;

        /// <summary>
        /// The text indicating which <see cref="ApplicationBarIconButton"/> to bind with.
        /// </summary>
        public string ButtonText { get; set; }

        /// <summary>
        /// The <see cref="ICommand"/> associated with the instance of ApplicationBarIconButton. 
        /// </summary>
        public ICommand CommandBinding
        {
            get { return (ICommand)GetValue(CommandBindingProperty); }
            set { SetValue(CommandBindingProperty, value); }
        }

        /// <summary>
        /// the string based parameter to be passed to the <see cref="ICommand"/>.
        /// </summary>
        public string CommandParameter
        {
            get { return (string)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.CreateBinding();
        }

        /// <summary>
        /// Invoked when the <see cref="CommandBinding"/> changes.
        /// </summary>
        protected void OnCommandChanged()
        {
            this.CreateBinding();
        }

        private static void HandleCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ApplicationBarButtonCommand)sender).OnCommandChanged();
        }

        private void CreateBinding()
        {
            if (this.CommandBinding == null || this.AssociatedObject == null) return;

            if (this.binding != null)
            {
                this.binding.Detach();
            }

            this.binding = new ClickCommandBinding(
                this.AssociatedObject.ApplicationBar.FindButton(this.ButtonText),
                (ICommand)this.CommandBinding,
                () => this.CommandParameter);
        }

        /// <summary>
        /// Binds an <see cref="ApplicationBarIconButton"/> to a <see cref="ICommand"/>.
        /// </summary>
        private class ClickCommandBinding
        {
            private readonly ICommand command;
            private readonly ApplicationBarIconButton iconButton;
            private readonly Func<object> parameterGetter;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="iconButton"></param>
            /// <param name="command"></param>
            /// <param name="parameterGetter"></param>
            public ClickCommandBinding(ApplicationBarIconButton iconButton, ICommand command, Func<object> parameterGetter)
            {
                this.command = command;
                this.iconButton = iconButton;
                this.parameterGetter = parameterGetter;
                this.iconButton.IsEnabled = this.command.CanExecute(parameterGetter());

                this.command.CanExecuteChanged += this.CommandCanExecuteChanged;
                this.iconButton.Click += this.IconButtonClicked;
            }

            public void Detach()
            {
                this.iconButton.Click -= this.IconButtonClicked;
                this.command.CanExecuteChanged -= this.CommandCanExecuteChanged;
            }

            private void IconButtonClicked(object s, EventArgs e)
            {
                this.command.Execute(this.parameterGetter());
            }

            private void CommandCanExecuteChanged(object s, EventArgs ea)
            {
                this.iconButton.IsEnabled = this.command.CanExecute(this.parameterGetter());
            }
        }
    }
}
