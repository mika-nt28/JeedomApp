using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace JeedomApp.Converters
{
    public class ChangeTypeConverter : IValueConverter
    {
        #region Public Properties
        public string OutType { get; set; }
        #endregion Public Properties
        #region Public Methods
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var _outType = OutType;
            if (_outType == null)
                return value;
            switch (OutType)
            {
                case "Double":
                    Double _double;
                    Double.TryParse((string)value, out _double);
                    return _double;
                default:
                    return value;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)

        {
            throw new NotImplementedException();
        }
        #endregion Public Methods
    }
}