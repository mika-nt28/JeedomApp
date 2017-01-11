using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace JeedomApp.Converters
{
    internal class JeedomIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return "N/A";
            var css = value as string;
            css = css.Replace("-", "_");
            JeedomIcons icon;
            Enum.TryParse<JeedomIcons>(css, out icon);
            var i = ((char)icon).ToString();
            return i;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        // Voir https://github.com/jeedom/core/blob/beta/core/css/icon/jeedom/style.css

        public enum JeedomIcons
        {
            jeedom_lumiere_off = '\ue610',
            jeedom_lumiere_on = '\ue611'
        }
    }
}