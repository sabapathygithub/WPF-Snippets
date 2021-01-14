using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContextMenuTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow: Window
	{
		List<string> menuSource;
		bool isDynamicAdded = false;

		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = new MainWindowViewModel();
			menuSource = new List<string>
			{
				"Dynamic 1",
				"Dynamic 2",
				"Dynamic 3"
			};
		}		
	}
}
