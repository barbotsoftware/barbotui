using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using BarBot.UWP.UserControls;

namespace BarBot.UWP.UserControls
{
    public class ToUpperConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value as String).ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
