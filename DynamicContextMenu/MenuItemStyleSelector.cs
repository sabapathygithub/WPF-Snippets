using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ContextMenuTest
{
	public class MenuItemStyleSelector: StyleSelector
	{
		public Style DynamicMenuStyle { get; set; }

		public Style StaticMenuStyle { get; set; }

		public override Style SelectStyle( object item, DependencyObject container )
		{
			if( item is MenuItemWrapper )
			{
				return this.DynamicMenuStyle;
			}
			else if( item is MenuItem )
			{
				return this.StaticMenuStyle;
			}
			return base.SelectStyle( item, container );
		}
	}
}
