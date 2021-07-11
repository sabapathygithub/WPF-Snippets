using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomControls.WPF.Controls
{
	/// <summary>Customized Window Control</summary>
	public class CustomWindow : Window
	{
		Button minimizeButton = null;

		static CustomWindow()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(typeof(CustomWindow)));
		}

		/// <summary>Gets or sets a value; which indicates whether to hide/show the minimize button.</summary>
		public bool HideMinimizeButton
		{
			get { return (bool)GetValue(HideMinimizeButtonProperty); }
			set { SetValue(HideMinimizeButtonProperty, value); }
		}

		// Using a DependencyProperty as the backing store for HideMinimizeButton.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HideMinimizeButtonProperty =
			DependencyProperty.Register("HideMinimizeButton", typeof(bool), typeof(CustomWindow), new PropertyMetadata(false, (obj, args) =>
			{
				if (obj is CustomWindow customWindow && customWindow.minimizeButton != null)
				{
					customWindow.minimizeButton.Visibility = ((bool)args.NewValue ? Visibility.Collapsed : Visibility.Visible);
				}
			}));

		public CustomWindow()
		{
			this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
			this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
			this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestroreWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, OnShowSystemMenu));
		}

		private void OnMaximizeWindow(object sender, ExecutedRoutedEventArgs e)
		{
			SystemCommands.MaximizeWindow(this);
		}

		private void OnRestroreWindow(object sender, ExecutedRoutedEventArgs e)
		{
			SystemCommands.RestoreWindow(this);
		}

		private void OnCloseWindow(object sender, ExecutedRoutedEventArgs e)
		{
			SystemCommands.CloseWindow(this);
		}

		private void OnMinimizeWindow(object sender, ExecutedRoutedEventArgs e)
		{
			SystemCommands.MinimizeWindow(this);
		}

		private void OnShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
		{
			var position = Mouse.GetPosition(this);
			var x = (this.WindowState == WindowState.Maximized) ? position.X : position.X + this.Left;
			var y = (this.WindowState == WindowState.Maximized) ? position.Y : position.Y + this.Top;
			SystemCommands.ShowSystemMenu(this, new Point(x, y));
		}

		public override void OnApplyTemplate()
		{
			Grid windowTitleLayout = GetTemplateChild("WindowTitleLayout") as Grid;
			if (windowTitleLayout != null)
			{
				windowTitleLayout.MouseLeftButtonDown += WindowTitleLayout_MouseLeftButtonDown;
			}
			minimizeButton = GetTemplateChild("Part_MinimizeButton") as Button;
			if (minimizeButton != null)
			{
				minimizeButton.Visibility = HideMinimizeButton ? Visibility.Collapsed : Visibility.Visible;
			}
			base.OnApplyTemplate();
		}

		private void WindowTitleLayout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}
	}
}
