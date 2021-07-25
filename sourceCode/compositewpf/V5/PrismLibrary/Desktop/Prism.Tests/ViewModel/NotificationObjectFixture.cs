// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Practices.Prism.TestSupport;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.Composition.Tests.ViewModel
{
    [TestClass]
    public class NotificationObjectFixture
    {
        [TestMethod]
        public void WhenNotifyingOnAnInstanceProperty_ThenAnEventIsFired()
        {
            var testObject = new TestNotificationObject();
            var changeTracker = new PropertyChangeTracker(testObject);

            testObject.InstanceProperty = "newValue";

            Assert.IsTrue(changeTracker.ChangedProperties.Contains("InstanceProperty"));
        }

        [TestMethod]
        public void NotificationObjectShouldBeSerializable()
        {
            var serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var stream = new System.IO.MemoryStream();
            bool invoked = false;

            var testObject = new TestNotificationObject();
            testObject.PropertyChanged += (o, e) => { invoked = true; };

            serializer.Serialize(stream, testObject);

            stream.Seek(0, System.IO.SeekOrigin.Begin);

            var reconstitutedObject = serializer.Deserialize(stream) as TestNotificationObject;

            Assert.IsNotNull(reconstitutedObject);
        }

        [TestMethod]
        public void NotificationObjectShouldBeXmlSerializable()
        {
            var serializer = new XmlSerializer(typeof(TestNotificationObject));

            var writeStream = new System.IO.StringWriter();

            var testObject = new TestNotificationObject();
            testObject.PropertyChanged += MockHandler;

            serializer.Serialize(writeStream, testObject);


            var readStream = new System.IO.StringReader(writeStream.ToString());
            var reconstitutedObject = serializer.Deserialize(readStream) as TestNotificationObject;

            Assert.IsNotNull(reconstitutedObject);
        }

        private void MockHandler(object o, EventArgs e)
        {
            // does nothing intentionally
        }
    
        [Serializable]
        public class TestNotificationObject : NotificationObject
        {
            private string instanceValue;

            public string InstanceProperty
            {
                get { return instanceValue; }
                set
                {
                    instanceValue = value;
                    RaisePropertyChanged(() => InstanceProperty);
                }
            }
        }
    }
}
