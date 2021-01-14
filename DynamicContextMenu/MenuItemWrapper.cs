using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ContextMenuTest
{
	public class MenuItemWrapper: INotifyPropertyChanged
	{
		private string textColor;

		public string TextColor
		{
			get { return textColor; }
			set
			{
				textColor = value;
				OnPropertyChanged();
			}
		}

		private string backgroundColor;

		public string BackgroundColor
		{
			get { return backgroundColor; }
			set
			{
				backgroundColor = value;
				OnPropertyChanged();
			}
		}

		private string imageUri;

		public string ImageUri
		{
			get { return imageUri; }
			set
			{
				imageUri = value;
				OnPropertyChanged();
			}
		}

		private string label;
		public string Label
		{
			get => label;
			set
			{
				label = value;
				OnPropertyChanged();
			}
		}

		private List<MenuItemWrapper> children = new List<MenuItemWrapper>();
		public List<MenuItemWrapper> Children
		{
			get => children; set
			{
				children = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName]string propName = "")
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propName ) );
		}
	}
}
