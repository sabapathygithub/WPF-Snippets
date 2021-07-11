using System.Collections;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CustomControls.WPF.Controls
{
	/// <summary>Allows user to select multiple option.</summary>
	public class MultiSelectionBox : ComboBox
	{
		ListBox selector;
		static MultiSelectionBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectionBox), new FrameworkPropertyMetadata(typeof(MultiSelectionBox)));
		}

		public string SelectionTextSeparator
		{
			get { return (string)GetValue(SelectionTextSeparatorProperty); }
			set { SetValue(SelectionTextSeparatorProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SelectionTextSeparator.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectionTextSeparatorProperty =
			DependencyProperty.Register("SelectionTextSeparator", typeof(string), typeof(MultiSelectionBox), new PropertyMetadata(", "));

		public IList SelectedItems
		{
			get { return (IList)GetValue(SelectedItemsProperty); }
			set { SetValue(SelectedItemsProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectedItemsProperty =
			DependencyProperty.Register("SelectedItems", typeof(IList), typeof(MultiSelectionBox), new PropertyMetadata(null));

		public string SelectionText
		{
			get { return (string)GetValue(SelectionTextProperty); }
			set { SetValue(SelectionTextProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ContentString.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectionTextProperty =
			DependencyProperty.Register("SelectionText", typeof(string), typeof(MultiSelectionBox), new PropertyMetadata(null));

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			selector = GetTemplateChild("ItemsPresenter") as ListBox;
			selector.SelectionChanged += Selector_SelectionChanged;
		}

		private void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectedItems = selector.SelectedItems;
			GenerateSelectionText();
		}

		private void GenerateSelectionText()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (SelectedItems != null)
			{
				for (int i = 0; i < SelectedItems.Count; i++)
				{
					stringBuilder.Append(SelectedItems[i].ToString());
					if (i != SelectedItems.Count - 1)
						stringBuilder.Append(SelectionTextSeparator);
				}
			}
			SelectionText = stringBuilder.ToString();
		}
	}
}
