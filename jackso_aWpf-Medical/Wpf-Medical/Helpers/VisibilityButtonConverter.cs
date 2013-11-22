using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Wpf_Medical.Helpers
{
    /// <summary>
    /// Classe pour convertir la visibility en booleen
    /// </summary>
    class VisibilityButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isVisible = (bool)value;
            if (IsVisibilityInverted(parameter))
                isVisible = !isVisible;
            return (isVisible ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isVisible = ((Visibility)value == Visibility.Visible);
            if (IsVisibilityInverted(parameter))
                isVisible = !isVisible;
            return isVisible;
        }

        private static Visibility GetVisibilityMode(object parameter)
        {
            Visibility mode = Visibility.Visible;
            if (parameter != null)
            {
                if (parameter is Visibility)
                {
                    mode = (Visibility)parameter;
                }
                else
                {
                    try
                    {
                        mode = (Visibility)Enum.Parse(typeof(Visibility), parameter.ToString(), true);
                    }
                    catch (FormatException e)
                    {
                        throw new FormatException("Invalid Visibility specified as the ConverterParameter.  Use Visible or Collapsed.", e);
                    }
                }
            }
            return mode;
        }

        private static bool IsVisibilityInverted(object parameter)
        {
            return (GetVisibilityMode(parameter) == Visibility.Collapsed);
        }
    }
}
