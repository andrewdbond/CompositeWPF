// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Interactivity;
using Microsoft.Practices.Prism.Composition.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Controls;

namespace Prism.Interactivity.Tests
{
    [TestClass]
    public class InvokeCommandActionFixture
    {
        [TestMethod]
        public void WhenCommandPropertyIsSet_ThenHooksUpCommandBehavior()
        {
            var someControl = new TextBox();
            var commandAction = new InvokeCommandAction();
            var command = new MockCommand();
            commandAction.Attach(someControl);
            commandAction.Command = command;

            Assert.IsFalse(command.ExecuteCalled);

            commandAction.InvokeAction(null);

            Assert.IsTrue(command.ExecuteCalled);
            Assert.AreSame(command, commandAction.GetValue(InvokeCommandAction.CommandProperty));
        }

        [TestMethod]
        public void WhenAttachedAfterCommandPropertyIsSetAndInvoked_ThenInvokesCommand()
        {
            var someControl = new TextBox();
            var commandAction = new InvokeCommandAction();
            var command = new MockCommand();
            commandAction.Command = command;
            commandAction.Attach(someControl);

            Assert.IsFalse(command.ExecuteCalled);

            commandAction.InvokeAction(null);

            Assert.IsTrue(command.ExecuteCalled);
            Assert.AreSame(command, commandAction.GetValue(InvokeCommandAction.CommandProperty));
        }

        [TestMethod]
        public void WhenChangingProperty_ThenUpdatesCommand()
        {
            var someControl = new TextBox();
            var oldCommand = new MockCommand();
            var newCommand = new MockCommand();
            var commandAction = new InvokeCommandAction();
            commandAction.Attach(someControl);
            commandAction.Command = oldCommand;
            commandAction.Command = newCommand;
            commandAction.InvokeAction(null);

            Assert.IsTrue(newCommand.ExecuteCalled);
            Assert.IsFalse(oldCommand.ExecuteCalled);
        }

        [TestMethod]
        public void WhenInvokedWithCommandParameter_ThenPassesCommandParaeterToExecute()
        {
            var someControl = new TextBox();
            var command = new MockCommand();
            var parameter = new object();
            var commandAction = new InvokeCommandAction();
            commandAction.Attach(someControl);
            commandAction.Command = command;
            commandAction.CommandParameter = parameter;

            Assert.IsNull(command.ExecuteParameter);

            commandAction.InvokeAction(null);

            Assert.IsTrue(command.ExecuteCalled);
            Assert.IsNotNull(command.ExecuteParameter);
            Assert.AreSame(parameter, command.ExecuteParameter);
        }

        [TestMethod]
        public void WhenCommandParameterChanged_ThenUpdatesIsEnabledState()
        {
            var someControl = new TextBox();
            var command = new MockCommand();
            var parameter = new object();
            var commandAction = new InvokeCommandAction();
            commandAction.Attach(someControl);
            commandAction.Command = command;

            Assert.IsNull(command.CanExecuteParameter);
            Assert.IsTrue(someControl.IsEnabled);

            command.CanExecuteReturnValue = false;
            commandAction.CommandParameter = parameter;

            Assert.IsNotNull(command.CanExecuteParameter);
            Assert.AreSame(parameter, command.CanExecuteParameter);
            Assert.IsFalse(someControl.IsEnabled);
        }

        [TestMethod]
        public void WhenCanExecuteChanged_ThenUpdatesIsEnabledState()
        {
            var someControl = new TextBox();
            var command = new MockCommand();
            var parameter = new object();
            var commandAction = new InvokeCommandAction();
            commandAction.Attach(someControl);
            commandAction.Command = command;
            commandAction.CommandParameter = parameter;

            Assert.IsTrue(someControl.IsEnabled);

            command.CanExecuteReturnValue = false;
            command.RaiseCanExecuteChanged();

            Assert.IsNotNull(command.CanExecuteParameter);
            Assert.AreSame(parameter, command.CanExecuteParameter);
            Assert.IsFalse(someControl.IsEnabled);
        }

        [TestMethod]
        public void WhenDetatched_ThenSetsCommandAndCommandParameterToNull()
        {
            var someControl = new TextBox();
            var command = new MockCommand();
            var parameter = new object();
            var commandAction = new InvokeCommandAction();
            commandAction.Attach(someControl);
            commandAction.Command = command;
            commandAction.CommandParameter = parameter;

            Assert.IsNotNull(commandAction.Command);
            Assert.IsNotNull(commandAction.CommandParameter);

            commandAction.Detach();

            Assert.IsNull(commandAction.Command);
            Assert.IsNull(commandAction.CommandParameter);
        }

        [TestMethod]
        public void WhenCommandIsSetAndThenBehaviorIsAttached_ThenCommandsCanExecuteIsCalledOnce()
        {
            var someControl = new TextBox();
            var command = new MockCommand();
            var commandAction = new InvokeCommandAction();
            commandAction.Command = command;
            commandAction.Attach(someControl);

            Assert.AreEqual(1, command.CanExecuteTimesCalled);
        }

        [TestMethod]
        public void WhenCommandAndCommandParameterAreSetPriorToBehaviorBeingAttached_ThenCommandIsExecutedCorrectlyOnInvoke()
        {
            var someControl = new TextBox();
            var command = new MockCommand();
            var parameter = new object();
            var commandAction = new InvokeCommandAction();
            commandAction.Command = command;
            commandAction.CommandParameter = parameter;
            commandAction.Attach(someControl);

            commandAction.InvokeAction(null);
           
            Assert.IsTrue(command.ExecuteCalled);
        }

        [TestMethod]
        public void WhenCommandParameterNotSet_ThenEventArgsPassed()
        {
            var eventArgs = new TestEventArgs(null);
            var someControl = new TextBox();
            var command = new MockCommand();
            var parameter = new object();
            var commandAction = new InvokeCommandAction();
            commandAction.Command = command;
            commandAction.Attach(someControl);

            commandAction.InvokeAction(eventArgs);

            Assert.IsInstanceOfType(command.ExecuteParameter, typeof(TestEventArgs));
        }

        [TestMethod]
        public void WhenCommandParameterNotSetAndEventArgsParameterPathSet_ThenPathedValuePassed()
        {
            var eventArgs = new TestEventArgs("testname");
            var someControl = new TextBox();
            var command = new MockCommand();
            var parameter = new object();
            var commandAction = new InvokeCommandAction();
            commandAction.Command = command;
            commandAction.TriggerParameterPath = "Thing1.Thing2.Name";
            commandAction.Attach(someControl);

            commandAction.InvokeAction(eventArgs);

            Assert.AreEqual("testname", command.ExecuteParameter);
        }
    }

    class TestEventArgs : EventArgs
    {
        public TestEventArgs(string name)
        {
            this.Thing1 = new Thing1 { Thing2 = new Thing2 { Name = name } };
        }

        public Thing1 Thing1 { get; set; }
    }

    class Thing1
    {
        public Thing2 Thing2 { get; set; }
    }

    class Thing2
    {
        public string Name { get; set; }
    }
}