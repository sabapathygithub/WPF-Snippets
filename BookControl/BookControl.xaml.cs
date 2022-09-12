using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookControlApp
{
    /// <summary>
    /// Interaction logic for BookControl.xaml
    /// </summary>
    public partial class BookControl : UserControl
    {
        private ObservableCollection<PageContent> allContents = new ObservableCollection<PageContent>();
        public BookControl()
        {
            InitializeComponent();
            navigationCtrl.MoveBackward += OnPageMoveForwardBackward;
            navigationCtrl.MoveForward += OnPageMoveForwardBackward;
        }

        private void OnPageMoveForwardBackward(object obj = null)
        {
            for (int i = 0; i < ContentGrid.ColumnDefinitions.Count; i++)
            {
                double columnWidth = (i == navigationCtrl.LeftPageIndex || i == navigationCtrl.RightPageIndex) ? 1.0d : 0.0d;
                ContentGrid.ColumnDefinitions[i].Width = new GridLength(columnWidth, GridUnitType.Star);                
            }
        }

        public void AddContent()
        {
            var pageContent = GetPageContent(allContents.Count);
            pageContent.Closed += PageContent_Closed;
            allContents.Add(pageContent);
            ContentGrid.Children.Add(pageContent);
            ContentGrid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0d, GridUnitType.Star)
            });
            Grid.SetColumn(pageContent, allContents.Count - 1);
            navigationCtrl.AddBookContent(new NavigationControlItem { Index = allContents.Count - 1, Name = $"Name{allContents.Count - 1}" });
            OnPageMoveForwardBackward();
        }

        private void PageContent_Closed(object obj)
        {
            if(obj is PageContent pageContent)
            {
                int removedIndex = allContents.IndexOf(pageContent);
                allContents.RemoveAt(removedIndex);
                ContentGrid.Children.RemoveAt(removedIndex);
                ContentGrid.ColumnDefinitions.RemoveAt(removedIndex);
                navigationCtrl.RemoveBookContent(removedIndex);
                OnPageMoveForwardBackward();
            }
        }

        private PageContent GetPageContent(int index)
        {
            return new PageContent
            {
                Content = new Border
                {
                    Child = new TextBlock
                    {
                        Text = $"Page {index + 1}",
                        FontSize = 32.0d,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    },                  
                    BorderBrush = Brushes.DeepSkyBlue,
                    BorderThickness = new Thickness(1.0d, 1.0d, 1.0, 0.0d)
                },
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
        }
    }
}
