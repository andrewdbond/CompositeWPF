//===============================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation
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
using System.Globalization;
using System.Windows.Data;
using StockTraderRI.Infrastructure.Properties;

namespace StockTraderRI.Infrastructure.Converters
{
    public class TwoDecimalPlaceConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int iNumDigits;

            if (value.GetType() != typeof(decimal))
            {
                throw new ArgumentException(Resources.ValueNotDecimalException, "value");
            }

            try
            {
                iNumDigits = int.Parse(parameter.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new ArgumentException(Resources.CannotConvertParameterToIntegerException, "parameter");
            }

            return Math.Round((decimal)value, iNumDigits);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
