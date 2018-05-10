using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGUI.Model
{
    public class StatusToColorConv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Must convert to a brush!");
            Model.MessageTypeEnum status = (Model.MessageTypeEnum)value;
            if (status == MessageTypeEnum.INFO)
            {
                return Brushes.SpringGreen;
            }
            else if (status == MessageTypeEnum.FAIL)
            {
                return Brushes.Red;
            }
            else if (status == MessageTypeEnum.WARNING)
            {
                return Brushes.Yellow;
            }
            else return Brushes.Transparent;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
