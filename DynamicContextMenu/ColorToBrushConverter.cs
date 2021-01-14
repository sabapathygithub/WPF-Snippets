using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ContextMenuTest
{
	public class ColorToBrushConverter: IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if( value == null )
			{
				return Binding.DoNothing;
			}
			string color = value.ToString();
			return (SolidColorBrush)( new BrushConverter().ConvertFrom( color ) );
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
