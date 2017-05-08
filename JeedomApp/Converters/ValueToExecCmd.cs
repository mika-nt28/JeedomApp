using Jeedom.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace JeedomApp.Converters
{
    public class ValueToExecCmd : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, string language)
        {
           if (targetType.FullName == "System.Double")
            {
                Double _double;
                Double.TryParse((string)value, out _double);
                return _double;
            }
            return (string)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (targetType.FullName == "System.String")
            {
                String _value= value.ToString();
                return _value;
            }
            return (double)value;
        }

        #endregion Public Methods
    }
}