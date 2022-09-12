using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BookControlApp
{

    public class BookPageDisplay : ListBox
    {
        double itemWidth_ = double.NaN;
        double itemHeight_ = double.NaN;

        static BookPageDisplay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BookPageDisplay), new FrameworkPropertyMetadata(typeof(BookPageDisplay)));
        }

        public BookPageDisplay()
        {
            this.SelectionChanged += BookPageDisplay_SelectionChanged;
        }



        public Brush SelectionBorderBrush
        {
            get { return (Brush)GetValue(SelectionBorderBrushProperty); }
            set { SetValue(SelectionBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionBorderBrushProperty =
            DependencyProperty.Register("SelectionBorderBrush", typeof(Brush), typeof(BookPageDisplay), new PropertyMetadata(null));


        private void BookPageDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!double.IsNaN(itemWidth_) && this.SelectedItems.Count > 0)
            {
                ArrangeElements();
            }
        }

        private void ArrangeElements()
        {
            if (Items.Count == SelectedItems.Count)
            {
                containerGrid_.HorizontalAlignment = HorizontalAlignment.Center;
                containerGrid_.Margin = new Thickness(0.0d);
            }
            else if(SelectedItems.Count > 1)
            {
                double itemContainerWidth = containerGrid_.ActualWidth;
                containerGrid_.HorizontalAlignment = HorizontalAlignment.Left;
                int[] selectedIndices = new int[this.SelectedItems.Count];
                for (int i = 0; i < this.SelectedItems.Count; i++)
                {
                    selectedIndices[i] = this.Items.IndexOf(this.SelectedItems[i]);
                }

                double selectedItemWidth = SelectedItems.Count * itemWidth_;
                double remainingItemWidth = (itemContainerWidth - selectedItemWidth) / (Items.Count - SelectedItems.Count);

                double availableLeftSideWidth = (ActualWidth - selectedItemWidth) / 2;
                int availableItemsOnLeft = Items.Count - (Items.Count - selectedIndices.Min());
                double margin = availableLeftSideWidth - (availableItemsOnLeft >= 0 ?(availableItemsOnLeft * remainingItemWidth) : -(selectedItemWidth / 2));
                if (margin > 0)
                {
                    containerGrid_.Margin = new Thickness(margin, 0.0d, 0.0d, 0.0d);
                }
                else if(margin < 0)
                {
                    // Increase the width of the control.
                    this.Width = ActualWidth + (remainingItemWidth * 3);
                    margin = margin + (remainingItemWidth * 1.5);
                    containerGrid_.Margin = new Thickness(margin, 0.0d, 0.0d, 0.0d);
                }
            }
        }

        Border selectionBorder_;
        Grid containerGrid_;
        ItemsPresenter itemsContainer_;
        Grid mainGrid_;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            selectionBorder_ = GetTemplateChild("SelectionBorder") as Border;
            containerGrid_ = GetTemplateChild("ContainerGrid") as Grid;
            itemsContainer_ = GetTemplateChild("ItemsContainer") as ItemsPresenter;            
            mainGrid_ = GetTemplateChild("MainGrid") as Grid;
            containerGrid_.SizeChanged += ContainerGrid_SizeChanged;
        }

        private void ContainerGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.Items.Count > 0 && double.IsNaN(itemWidth_))
            {
                var contentPre = this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem);
                if (contentPre is FrameworkElement element)
                {
                    itemWidth_ = element.ActualWidth;
                    itemHeight_ = element.ActualHeight;
                }
            }
            if (!double.IsNaN(itemWidth_))
            {
                selectionBorder_.Visibility = Visibility.Visible;
                selectionBorder_.Width = itemWidth_ * (Items.Count <= 1 ? 1.0d : 2.0d);
                selectionBorder_.Height = itemHeight_ - 2;
                ArrangeElements();
            }
        }
    }
}
