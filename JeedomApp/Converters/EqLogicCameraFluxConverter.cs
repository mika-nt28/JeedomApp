using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace JeedomApp.Converters
{
    internal class EqLogicCameraFluxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var url = value as string;
            if (url != null)
                return Jeedom.RequestViewModel.config.Uri + url;
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}