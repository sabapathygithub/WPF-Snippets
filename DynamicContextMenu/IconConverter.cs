using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ContextMenuTest
{
	public class IconConverter: IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if( value == null )
			{
				return Binding.DoNothing;
			}

			string imageUrl = value.ToString();

			if( string.IsNullOrEmpty( imageUrl ) )
			{
				return Binding.DoNothing;
			}
			var bmp = new BitmapImage( new Uri( imageUrl, UriKind.RelativeOrAbsolute ) ) { DecodePixelHeight = 16, DecodePixelWidth = 16 };

			return new Image { Width = 16, Height = 16, Source = bmp, UseLayoutRounding = true, SnapsToDevicePixels = true };
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
