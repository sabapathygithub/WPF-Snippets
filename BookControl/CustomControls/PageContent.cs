using System;
using System.Windows;
using System.Windows.Controls;

namespace BookControlApp
{

    public class PageContent : Control
    {
        public event Action<object> Closed;
        static PageContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageContent), new FrameworkPropertyMetadata(typeof(PageContent)));
        }



        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(PageContent), new PropertyMetadata(null));

        Button closeButton;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            closeButton = GetTemplateChild("Part_CloseButton") as Button;
            closeButton.Click += CloseButton_Click;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Closed?.Invoke(this);
        }
    }
}
