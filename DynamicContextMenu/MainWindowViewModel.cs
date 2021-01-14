using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ContextMenuTest
{
	public class MainWindowViewModel: INotifyPropertyChanged
	{
		private ObservableCollection<MenuItemWrapper> searchableItems;
		public ObservableCollection<MenuItemWrapper> SearchableItems { get => searchableItems; set { searchableItems = value; OnPropertyChanged( "SearchableItems" ); } }

		public MainWindowViewModel()
		{
			var collection = new ObservableCollection<MenuItemWrapper>();
			collection.Add( new MenuItemWrapper { Label = "Test", ImageUri = "pack://application:,,,/ContextMenuTest;component/Resources/visibility.png" } );
			var menuItem = new MenuItemWrapper
			{
				Label = "Sub",
				ImageUri = "pack://application:,,,/ContextMenuTest;component/Resources/visibility.png",
				TextColor = "#ff2288",
				BackgroundColor = "#ffffff",
				Children = new List<MenuItemWrapper>
				{
				  new MenuItemWrapper
				  {
					  Label =   "SubTest1", ImageUri ="pack://application:,,,/ContextMenuTest;component/Resources/visibility.png",
					  TextColor = "#ff2288",
					  BackgroundColor = "#f222ff",
				  },
				   new MenuItemWrapper
				   {
					   Label =   "SubTest1", ImageUri = "pack://application:,,,/ContextMenuTest;component/Resources/visibility.png",
					   TextColor = "#ff2288",
					   BackgroundColor = "#fff444",
				   }
				}
			};
			collection.Add( menuItem );
			searchableItems = collection;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged( string propName )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propName ) );
		}
	}
}
