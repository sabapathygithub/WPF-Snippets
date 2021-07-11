using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace CustomControls.WPF.Controls
{
	/// <summary>Customized tool window used in About and Release Notes View.</summary>
	public class CustomToolWindow : Window
	{
		static CustomToolWindow()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomToolWindow), new FrameworkPropertyMetadata(typeof(CustomToolWindow)));
		}

		public CustomToolWindow()
		{
			this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
		}

		private void OnCloseWindow(object sender, ExecutedRoutedEventArgs e)
		{
			SystemCommands.CloseWindow(this);
		}

		public FrameworkElement Selector
		{
			get { return (FrameworkElement)GetValue(SelectorProperty); }
			set { SetValue(SelectorProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Selector.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectorProperty =
			DependencyProperty.Register("Selector", typeof(FrameworkElement), typeof(CustomToolWindow), new PropertyMetadata(null));

		public bool HideAdditiveFlowLog
		{
			get { return (bool)GetValue(HideAdditiveFlowLogProperty); }
			set { SetValue(HideAdditiveFlowLogProperty, value); }
		}

		// Using a DependencyProperty as the backing store for HideAdditiveFlowLog.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HideAdditiveFlowLogProperty =
			DependencyProperty.Register("HideAdditiveFlowLog", typeof(bool), typeof(CustomToolWindow), new PropertyMetadata(false));



		public bool ShowCustomTitle
		{
			get { return (bool)GetValue(ShowCustomTitleProperty); }
			set { SetValue(ShowCustomTitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ShowCustomTitle.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ShowCustomTitleProperty =
			DependencyProperty.Register("ShowCustomTitle", typeof(bool), typeof(CustomToolWindow), new PropertyMetadata(false));



		public FrameworkElement CustomTitleContent
		{
			get { return (FrameworkElement)GetValue(CustomTitleContentProperty); }
			set { SetValue(CustomTitleContentProperty, value); }
		}

		// Using a DependencyProperty as the backing store for CustomTitleContent.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CustomTitleContentProperty =
			DependencyProperty.Register("CustomTitleContent", typeof(FrameworkElement), typeof(CustomToolWindow), new PropertyMetadata(null));




		public override void OnApplyTemplate()
		{
			Grid windowTitleLayout = GetTemplateChild("WindowTitleLayout") as Grid;
			if (windowTitleLayout != null)
			{
				windowTitleLayout.MouseLeftButtonDown += WindowTitleLayout_MouseLeftButtonDown;
			}
			ContentControl selectorPlaceHolder = GetTemplateChild("ComboboxPlaceHolder") as ContentControl;
			if (selectorPlaceHolder != null)
			{
				selectorPlaceHolder.Content = Selector;
			}
			StackPanel logoPanel = GetTemplateChild("LogoPlaceHolder") as StackPanel;
			if (logoPanel != null)
			{
				logoPanel.Visibility = !ShowCustomTitle ? Visibility.Visible : Visibility.Collapsed;
			}
			ContentControl customLogo = GetTemplateChild("CustomLogoPlaceHolder") as ContentControl;
			if (customLogo != null)
			{
				customLogo.Visibility = ShowCustomTitle ? Visibility.Visible : Visibility.Collapsed;
				customLogo.Content = CustomTitleContent;
			}
			base.OnApplyTemplate();
		}

		private void WindowTitleLayout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}
	}
}
