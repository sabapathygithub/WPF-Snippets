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

namespace CustomTooltipSample.CustomControls
{
	/// <summary>
	/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
	///
	/// Step 1a) Using this custom control in a XAML file that exists in the current project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is 
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:CustomTooltipSample.CustomControls"
	///
	///
	/// Step 1b) Using this custom control in a XAML file that exists in a different project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is 
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:CustomTooltipSample.CustomControls;assembly=CustomTooltipSample.CustomControls"
	///
	/// You will also need to add a project reference from the project where the XAML file lives
	/// to this project and Rebuild to avoid compilation errors:
	///
	///     Right click on the target project in the Solution Explorer and
	///     "Add Reference"->"Projects"->[Browse to and select this project]
	///
	///
	/// Step 2)
	/// Go ahead and use your control in the XAML file.
	///
	///     <MyNamespace:CustomTooltip/>
	///
	/// </summary>
	public class CustomTooltip: ToolTip
	{
		static CustomTooltip()
		{
			DefaultStyleKeyProperty.OverrideMetadata( typeof( CustomTooltip ), new FrameworkPropertyMetadata( typeof( CustomTooltip ) ) );
		}


		public string Title
		{
			get { return (string)GetValue( TitleProperty ); }
			set { SetValue( TitleProperty, value ); }
		}

		// Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register( "Title", typeof( string ), typeof( CustomTooltip ), new PropertyMetadata( string.Empty ) );



		public string Body
		{
			get { return (string)GetValue( BodyProperty ); }
			set { SetValue( BodyProperty, value ); }
		}

		// Using a DependencyProperty as the backing store for Body.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty BodyProperty =
			DependencyProperty.Register( "Body", typeof( string ), typeof( CustomTooltip ), new PropertyMetadata( string.Empty, ( obj, args ) =>
			{
				if( obj is CustomTooltip customTooltip && customTooltip.bodyText != null )
				{
					string newTitle = Convert.ToString( args.NewValue );
					var inlines = customTooltip.GenerateTextInlines( newTitle );
					customTooltip.bodyText.Inlines.Clear();
					foreach( var item in inlines )
					{
						customTooltip.bodyText.Inlines.Add( item );
					}
				}
			} ) );


		private ICollection<Inline> GenerateTextInlines( string text )
		{
			List<Inline> inlines = new List<Inline>();
			if( !string.IsNullOrEmpty( text ) )
			{
				//This code splits based on new line character and adds it to the TextBlock inline.
				var stringLines = text.Split( new string[] { "\\n" }, StringSplitOptions.RemoveEmptyEntries ).ToList();
				var lastItem = stringLines.Last();
				foreach( var item in stringLines )
				{
					inlines.Add( new Run( item?.Trim() ) );
					//Condition to check to avoid line break for last item.
					if( !item.Equals( lastItem ) )
					{
						inlines.Add( new LineBreak() );
					}
				}
			}
			return inlines;
		}

		public Visibility TipVisibility
		{
			get { return (Visibility)GetValue( TipVisibilityProperty ); }
			set { SetValue( TipVisibilityProperty, value ); }
		}

		// Using a DependencyProperty as the backing store for TipVisibility.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TipVisibilityProperty =
			DependencyProperty.Register( "TipVisibility", typeof( Visibility ), typeof( CustomTooltip ), new PropertyMetadata( Visibility.Collapsed ) );




		public object Tip
		{
			get { return (object)GetValue( TipProperty ); }
			set { SetValue( TipProperty, value ); }
		}

		// Using a DependencyProperty as the backing store for Tip.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TipProperty =
			DependencyProperty.Register( "Tip", typeof( object ), typeof( CustomTooltip ), new PropertyMetadata( null, ( obj, args ) =>
			{
				if( obj is CustomTooltip customTooltip && customTooltip.tipText != null )
				{
					customTooltip.TipVisibility = args.NewValue != null ? Visibility.Visible : Visibility.Collapsed;
					switch( args.NewValue )
					{
						case string strValue:
							customTooltip.tipText.Visibility = Visibility.Visible;
							var inlines = customTooltip.GenerateTextInlines( strValue );
							customTooltip.tipText.Inlines.Clear();
							foreach( var item in inlines )
							{
								customTooltip.tipText.Inlines.Add( item );
							}							
							break;
						default:
							//If the Tip is other than string type. This line will collapses the TipText TextBlock and enables ContentPresenter
							customTooltip.tipText.Visibility = Visibility.Collapsed;
							break;
					}
				}
			} ) );

		private TextBlock tipText;
		private TextBlock bodyText;
		private ContentPresenter tipContent;
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			tipText = GetTemplateChild( "Part_TipText" ) as TextBlock;
			bodyText = GetTemplateChild( "Part_BodyText" ) as TextBlock;
			tipContent = GetTemplateChild( "Part_TipContent" ) as ContentPresenter;
			TipVisibility = ( Tip != null ) ? Visibility.Visible: Visibility.Collapsed ;
			if(Tip is string text)
			{
				var inlines = GenerateTextInlines( text );
				tipText.Inlines.Clear();
				foreach( var inline in inlines )
				{
					tipText.Inlines.Add( inline );
				}		
				tipText.Visibility = Visibility.Visible;
			}
			else if(Tip != null)
			{
				tipText.Visibility = Visibility.Collapsed;
				tipContent.Content = Tip;
			}

			if(Body is string body)
			{
				var inlines = GenerateTextInlines( body );
				bodyText.Inlines.Clear();
				foreach( var inline in inlines )
				{
					bodyText.Inlines.Add( inline );
				}
			}

		}
	}
}
