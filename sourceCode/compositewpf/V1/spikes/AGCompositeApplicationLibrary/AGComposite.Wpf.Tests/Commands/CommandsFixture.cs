//===============================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

using System;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandProperties = Microsoft.Practices.Composite.Wpf.Commands.Commands;

namespace Microsoft.Practices.Composite.Wpf.Tests.Commands
{
    [TestClass]
    public class CommandsFixture
    {
        [TestMethod]
        public void ButtonClickExecutesCommand()
        {
            var mockCommand = new MockCommand();
            MockClickableButton button = new MockClickableButton();
            button.SetValue(CommandProperties.CommandProperty, mockCommand);
            Assert.IsFalse(mockCommand.ExecuteCalled);

            button.InvokeClick();

            Assert.IsTrue(mockCommand.ExecuteCalled);
        }

        [TestMethod]
        public void CommandParameterIsPassedToCommandInvocation()
        {
            var mockCommand = new MockCommand();
            MockClickableButton button = new MockClickableButton();
            button.SetValue(CommandProperties.CommandProperty, mockCommand);
            object parameter = new object();
            button.SetValue(CommandProperties.CommandParameterProperty, parameter);

            button.InvokeClick();

            Assert.IsNotNull(mockCommand.ExecuteArgumentParameter);
            Assert.AreEqual(parameter, mockCommand.ExecuteArgumentParameter);
        }

        [TestMethod]
        public void DeattachingFromCommandDoesNotInvokeItWhenClickingIt()
        {
            var mockCommand = new MockCommand();
            MockClickableButton button = new MockClickableButton();
            button.SetValue(CommandProperties.CommandProperty, mockCommand);

            button.SetValue(CommandProperties.CommandProperty, null);
            button.InvokeClick();

            Assert.IsFalse(mockCommand.ExecuteCalled);
        }

        [TestMethod]
        public void CanExecuteChangedMakesButtonRequeryCommandAndSetsIsEnabledAppropriately()
        {
            var mockCommand = new MockCommand();
            var button = new Button();
            button.IsEnabled = true;
            button.SetValue(CommandProperties.CommandProperty, mockCommand);

            mockCommand.CanExecuteReturnValue = false;
            mockCommand.RaiseCanExecuteChanged();

            Assert.IsFalse(button.IsEnabled);

            mockCommand.CanExecuteReturnValue = true;
            mockCommand.RaiseCanExecuteChanged();

            Assert.IsTrue(button.IsEnabled);
        }

        [TestMethod]
        public void CanExecuteChangedGetsCalledPassingTheCommandParameter()
        {
            var mockCommand = new MockCommand();
            var button = new Button();
            button.IsEnabled = true;
            button.SetValue(CommandProperties.CommandProperty, mockCommand);
            object parameter = new object();
            button.SetValue(CommandProperties.CommandParameterProperty, parameter);

            mockCommand.CanExecuteArgumentParameter = null;
            mockCommand.RaiseCanExecuteChanged();

            Assert.IsNotNull(mockCommand.CanExecuteArgumentParameter);
            Assert.AreSame(parameter, mockCommand.CanExecuteArgumentParameter);
        }

        [TestMethod]
        public void WhenDesubscribedDontCallCanExecute()
        {
            var mockCommand = new MockCommand();
            var button = new Button();
            button.IsEnabled = true;
            button.SetValue(CommandProperties.CommandProperty, mockCommand);
            button.SetValue(CommandProperties.CommandProperty, null);

            mockCommand.CanExecuteCalled = false;
            mockCommand.RaiseCanExecuteChanged();

            Assert.IsFalse(mockCommand.CanExecuteCalled);
        }


        [TestMethod]
        public void BehaviorDoesNotPreventButtonFromBeingGarbageCollected()
        {
            var mockCommand = new MockCommand();
            var button = new Button();

            button.SetValue(CommandProperties.CommandProperty, mockCommand);

            WeakReference buttonReference = new WeakReference(button);

            button = null;
            GC.Collect();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.IsFalse(buttonReference.IsAlive);
        }

        [TestMethod]
        public void InitialHookupSetButtonAsDisabledIfCanExecuteIsFalse()
        {
            var mockCommand = new MockCommand();
            var button = new Button();
            button.IsEnabled = true;

            mockCommand.CanExecuteReturnValue = false;
            button.SetValue(CommandProperties.CommandProperty, mockCommand);

            Assert.IsFalse(button.IsEnabled);
        }


    }

    /* TODO: observe CanExecuteChanged on the command (avoiding memory leaks)
     * Make sure CommandButtonBehaviorProperty is private */

    public class MockClickableButton : Button
    {
        public void InvokeClick()
        {
            OnClick();
        }
    }

    public class MockCommand : ICommand
    {
        public bool ExecuteCalled;
        public object ExecuteArgumentParameter;
        public bool CanExecuteReturnValue;
        public object CanExecuteArgumentParameter;
        public bool CanExecuteCalled = false;

        public bool CanExecute(object parameter)
        {
            CanExecuteArgumentParameter = parameter;
            CanExecuteCalled = true;
            return CanExecuteReturnValue;
        }

        public void Execute(object parameter)
        {
            ExecuteCalled = true;
            ExecuteArgumentParameter = parameter;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
