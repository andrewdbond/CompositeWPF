// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Prism.TestSupport;

namespace Microsoft.Practices.Prism.Composition.Tests.ViewModel
{
    [TestClass]
    public class PropertySupportFixture
    {
        [TestMethod]
        public virtual void WhenExtractingNameFromAValidPropertyExpression_ThenPropertyNameReturned()
        {
            var propertyName = PropertySupport.ExtractPropertyName(() => this.InstanceProperty);
            Assert.AreEqual("InstanceProperty", propertyName);
        }

        [TestMethod]
        public void WhenExpressionRepresentsAStaticProperty_ThenExceptionThrown()
        {
            ExceptionAssert.Throws<ArgumentException>(() => PropertySupport.ExtractPropertyName(() => StaticProperty));
        }

        [TestMethod]
        public void WhenExpressionIsNull_ThenAnExceptionIsThrown()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() => PropertySupport.ExtractPropertyName<int>(null));
        }

        [TestMethod]
        public void WhenExpressionRepresentsANonMemberAccessExpression_ThenAnExceptionIsThrown()
        {
            ExceptionAssert.Throws<ArgumentException>(
                () => PropertySupport.ExtractPropertyName(() => this.GetHashCode())
                );

        }

        [TestMethod]
        public void WhenExpressionRepresentsANonPropertyMemberAccessExpression_ThenAnExceptionIsThrown()
        {
            ExceptionAssert.Throws<ArgumentException>(() => PropertySupport.ExtractPropertyName(() => this.InstanceField));
        }

        [TestMethod]
        [Ignore]    // cannot build the expression
        public void WhenExpressionRepresentsAPropertyWithNoGetMethod_ThenAnExceptionIsThrown()
        {
            ExceptionAssert.Throws<ArgumentException>(() =>
                       PropertySupport.ExtractPropertyName(
                               Expression.Lambda<Func<int>>(
                                   Expression.MakeMemberAccess(
                                       null,
                                       typeof(PropertySupportFixture).GetProperty("SetOnlyStaticProperty")))));
        }

        public static int StaticProperty { get; set; }
        public int InstanceProperty { get; set; }
        public int InstanceField;
        public static int SetOnlyStaticProperty { set { } }
    }
}
