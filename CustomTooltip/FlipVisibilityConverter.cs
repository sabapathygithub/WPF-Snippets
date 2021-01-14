using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CustomTooltipSample
{
	public class FlipVisibilityConverter: IValueConverter
	{
		static readonly object visible = Visibility.Visible;
		static readonly object collapsed = Visibility.Collapsed;

		object IValueConverter.Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			switch( (Visibility)value )
			{
				case Visibility.Collapsed:
				case Visibility.Hidden:
					return visible;
				case Visibility.Visible:
					return collapsed;
			}
			throw new ArgumentException();
		}

		object IValueConverter.ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotSupportedException();
		}
	}
}
