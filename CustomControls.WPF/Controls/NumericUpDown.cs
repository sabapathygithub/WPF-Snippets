using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CustomControls.WPF.Controls
{
    /// <summary>
    ///  Defines the logic for numeric updown.
    /// </summary>
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_IncreaseButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_DecreaseButton", Type = typeof(RepeatButton))]
    public class NumericUpDown : Control
    {
        private TextBox _textBox;
        private RepeatButton _increaseButton;
        private RepeatButton _decreaseButton;
        private readonly RoutedUICommand _increaseValueCommand = new RoutedUICommand("IncreaseValue", "IncreaseValue", typeof(NumericUpDown));
        private readonly RoutedUICommand _decreaseValueCommand = new RoutedUICommand("DecreaseValue", "DecreaseValue", typeof(NumericUpDown));
        private readonly RoutedUICommand _updateValueStringCommand = new RoutedUICommand("UpdateValueString", "UpdateValueString", typeof(NumericUpDown));

        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
        }

        /// <summary>
        /// Initializes the numeric up down.
        /// </summary>
        public NumericUpDown()
        {
            CommandBindings.Add(new CommandBinding(_increaseValueCommand, (a, b) => IncreaseValue()));
            CommandBindings.Add(new CommandBinding(_decreaseValueCommand, (a, b) => DecreaseValue()));
            CommandBindings.Add(new CommandBinding(_updateValueStringCommand, (a, b) =>
            {
                Value = ParseStringToInt(_textBox?.Text);
            }));
        }

        #region Value
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Backing store for Value.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDown), new PropertyMetadata(0, OnValueChanged, CoerceValue));

        private static void OnValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
        }

        private static object CoerceValue(DependencyObject element, object baseValue)
        {
            var control = (NumericUpDown)element;
            var value = (int)baseValue;
            if (control._textBox != null)
                control._textBox.Text = value.ToString(CultureInfo.CurrentCulture);
            return value;
        }
        #endregion


        #region MaxValue
        /// <summary>
        /// Gets or sets maximum value.
        /// </summary>
        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        /// <summary>
        /// Backing store for MaxValue.
        /// </summary>        
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(NumericUpDown), new PropertyMetadata(int.MaxValue, OnMaxValueChanged, CoerceMaxValue));

        private static void OnMaxValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown)element;
            var maxValue = (int)e.NewValue;

            // If maxValue steps over MinValue, shift it
            if (maxValue < control.MinValue)
            {
                control.MinValue = maxValue;
            }

            if (maxValue <= control.Value)
            {
                control.Value = maxValue;
            }
        }

        private static object CoerceMaxValue(DependencyObject element, Object baseValue)
        {
            var maxValue = (int)baseValue;

            if (maxValue == int.MaxValue)
            {
                return DependencyProperty.UnsetValue;
            }

            return maxValue;
        }
        #endregion


        #region MinValue
        /// <summary>
        ///  Gets or sets the minimum value.
        /// </summary>
        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        /// <summary>
        /// Backing store for MinValue
        /// </summary>
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(NumericUpDown), new PropertyMetadata(int.MinValue, OnMinValueChanged, CoerceMinValue));

        private static void OnMinValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown)element;
            var minValue = (int)e.NewValue;

            // If minValue steps over MaxValue, shift it
            if (minValue > control.MaxValue)
            {
                control.MaxValue = minValue;
            }

            //if (minValue >= control.Value)
            //{
            //    //control.Value = minValue;
            //}
        }

        private static object CoerceMinValue(DependencyObject element, Object baseValue)
        {
            var minValue = (int)baseValue;

            if (minValue == int.MinValue)
            {
                return DependencyProperty.UnsetValue;
            }

            return minValue;
        }
        #endregion


        /// <summary>
        /// Overrides the on apply template of base class and attaches the children controls.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _textBox = GetTemplateChild("PART_TextBox") as TextBox;
            if (_textBox != null)
            {
                _textBox.Text = Value.ToString();
                _textBox.InputBindings.Add(new KeyBinding(_increaseValueCommand, new KeyGesture(Key.Up)));
                _textBox.InputBindings.Add(new KeyBinding(_decreaseValueCommand, new KeyGesture(Key.Down)));
                _textBox.InputBindings.Add(new KeyBinding(_updateValueStringCommand, new KeyGesture(Key.Enter)));
                _textBox.LostFocus += TextBox_LostFocus;
            }
            _increaseButton = GetTemplateChild("PART_IncreaseButton") as RepeatButton;
            if (_increaseButton != null)
            {
                _increaseButton.Focusable = false;
                _increaseButton.Command = _increaseValueCommand;
            }
            _decreaseButton = GetTemplateChild("PART_DecreaseButton") as RepeatButton;
            if (_decreaseButton != null)
            {
                _decreaseButton.Focusable = false;
                _decreaseButton.Command = _decreaseValueCommand;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Value = ParseStringToInt(_textBox.Text);
        }

        private int ParseStringToInt(string text)
        {
            int.TryParse(text, out int value);
            return value;
        }

        private void CoerceValueToBounds(ref int value)
        {
            if (value < MinValue)
            {
                value = MinValue;
            }
            else if (value > MaxValue)
            {
                value = MaxValue;
            }
        }

        private void IncreaseValue()
        {
            // Get the value that's currently in the _textBox.Text
            int value = ParseStringToInt(_textBox?.Text);
            value++;
            // Coerce the value to min/max
            CoerceValueToBounds(ref value);

            Value = value;
        }

        private void DecreaseValue()
        {
            // Get the value that's currently in the _textBox.Text
            int value = ParseStringToInt(_textBox?.Text);
            value--;
            // Coerce the value to min/max
            CoerceValueToBounds(ref value);

            Value = value;
        }
    }
}
