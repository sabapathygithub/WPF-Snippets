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
    /// Interaction logic for NavigationControl.xaml
    /// </summary>
    public partial class NavigationControl : UserControl
    {

        private int leftPageIndex_;
        private int rightPageIndex_;

        public NavigationControl()
        {
            InitializeComponent();
            BookContents = new ObservableCollection<NavigationControlItem>();
            BookContents.CollectionChanged += BookContents_CollectionChanged;
            BackButton.SetBinding(IsEnabledProperty, new Binding(nameof(EnableBackNavigation)) { Source = this });
            ForwardButton.SetBinding(IsEnabledProperty, new Binding(nameof(EnableForwardNavigation)) { Source = this });
            PageDisplayControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(nameof(BookContents)) { Source = this });
        }


        /// <summary>
        /// Occurs when the forward command is executed.
        /// </summary>
        public event Action<object> MoveForward;

        /// <summary>
        /// Occurs when the back command is executed.
        /// </summary>
        public event Action<object> MoveBackward;

        public ObservableCollection<NavigationControlItem> BookContents
        {
            get { return (ObservableCollection<NavigationControlItem>)GetValue(BookContentsProperty); }
            set { SetValue(BookContentsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BookContents.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BookContentsProperty =
            DependencyProperty.Register("BookContents", typeof(ObservableCollection<NavigationControlItem>), typeof(NavigationControl), new PropertyMetadata(null));



        public bool EnableBackNavigation
        {
            get { return (bool)GetValue(EnableBackNavigationProperty); }
            set { SetValue(EnableBackNavigationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnableBackNavigation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableBackNavigationProperty =
            DependencyProperty.Register("EnableBackNavigation", typeof(bool), typeof(NavigationControl), new PropertyMetadata(false));




        public bool EnableForwardNavigation
        {
            get { return (bool)GetValue(EnableForwardNavigationProperty); }
            set { SetValue(EnableForwardNavigationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnableForwardNavigation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableForwardNavigationProperty =
            DependencyProperty.Register("EnableForwardNavigation", typeof(bool), typeof(NavigationControl), new PropertyMetadata(false));



        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            leftPageIndex_++;
            rightPageIndex_++;
            EnableDisableNavigation();
            MoveForward?.Invoke(this);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            leftPageIndex_--;
            rightPageIndex_--;
            EnableDisableNavigation();
            MoveBackward?.Invoke(this);
        }

        public int LeftPageIndex { get => leftPageIndex_; }

        public int RightPageIndex { get => rightPageIndex_; }


        private void OnNavigateCommandExecuted(NavigationControlItem controlItem)
        {            
            // If the clicked item is currently visible in the book control then it should be skipped for navigation.
            if (controlItem.Index == rightPageIndex_ || controlItem.Index == leftPageIndex_)
            {
                return;
            }
            else if (controlItem.Index < leftPageIndex_)
            {
                leftPageIndex_ = controlItem.Index;
                rightPageIndex_ = controlItem.Index + 1;
                MoveBackward?.Invoke(this);
            }
            else
            {
                leftPageIndex_ = controlItem.Index - 1;
                rightPageIndex_ = controlItem.Index;
                MoveForward?.Invoke(this);
            }
            EnableDisableNavigation();
        }

        private void BookContents_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (BookContents.Count > 1)
            {
                leftPageIndex_ = BookContents.Count - 2;
                rightPageIndex_ = BookContents.Count - 1;
                EnableDisableNavigation();
            }
            else if (BookContents.Count == 1)
            {
                BookContents[0].IsSelected = true;
            }
        }

        private void EnableDisableNavigation()
        {
            if(BookContents.Count == 0)
            {
                return;
            }
            foreach (var item in BookContents.Where(i => i.IsSelected))
            {
                item.IsSelected = false;
            }
            if (leftPageIndex_ >= 0)
            {
                BookContents[leftPageIndex_].IsSelected = true;
            }
            if (rightPageIndex_ < BookContents.Count)
            {
                BookContents[rightPageIndex_].IsSelected = true;
            }

            EnableBackNavigation = leftPageIndex_ > 0;
            EnableForwardNavigation = rightPageIndex_ < BookContents.Count - 1;
        }

        /// <summary>
        /// Clears all the contents.
        /// </summary>
        public void ClearBookContents()
        {
            leftPageIndex_ = 0;
            rightPageIndex_ = -1;
            for (int i = BookContents.Count - 1; i > 0; i--)
            {
                BookContents.RemoveAt(i);
            }
        }

        /// <summary>
        /// Removes item of the index from BookContents 
        /// </summary>
        public void RemoveBookContent(int removedIndex)
        {
            BookContents.RemoveAt(removedIndex);
        }

        public void AddBookContent(NavigationControlItem controlItem)
        {
            BookContents.Add(controlItem);
        }

        private void CenterButton_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button centerButton && centerButton.DataContext is NavigationControlItem controlItem)
            {
                OnNavigateCommandExecuted(controlItem);
            }
        }
    }

    public class NavigationControlItem : BaseViewModel
    {
        public int Index { get; set; }

        public string Name { get; set; } = string.Empty;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected; 
            set
            {
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }
    }
}
