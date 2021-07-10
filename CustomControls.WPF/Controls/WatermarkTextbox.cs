using System.Windows;
using System.Windows.Controls;

namespace CustomControls.WPF.Controls
{
    /// <summary>
    /// Allow the user to display watermark over the textbox when there is not text.
    /// </summary>
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "Part_Watermark", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
    public class WatermarkTextbox : Control
    {
        private TextBox _textBox;
        private TextBlock _watermarkTextBlock;
        private Button _clearButton;

        static WatermarkTextbox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkTextbox), new FrameworkPropertyMetadata(typeof(WatermarkTextbox)));
        }


        /// <summary>
        /// Gets or sets the watermark text.
        /// </summary>
        public string WatermarkText
        {
            get { return (string)GetValue(WatermarkTextProperty); }
            set { SetValue(WatermarkTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WatermarkText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.Register("WatermarkText", typeof(string), typeof(WatermarkTextbox), new PropertyMetadata(null));



        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(WatermarkTextbox), new PropertyMetadata(null));

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _textBox = GetTemplateChild("PART_TextBox") as TextBox;
            _textBox.TextChanged += TextBox_TextChanged;
            _watermarkTextBlock = GetTemplateChild("Part_Watermark") as TextBlock;
            _clearButton = GetTemplateChild("PART_ClearButton") as Button;
            _clearButton.Click += ClearButton_Click;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = _textBox.Text;
            if (Text.Length > 0)
            {
                _clearButton.Visibility = Visibility.Visible;
                _watermarkTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                _clearButton.Visibility = Visibility.Collapsed;
                _watermarkTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Text = string.Empty;
            _clearButton.Visibility = Visibility.Collapsed;
            _watermarkTextBlock.Visibility = Visibility.Visible;
        }
    }
}
