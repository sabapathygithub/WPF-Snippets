using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CustomControls.WPF.Controls
{
    /// <summary>
    /// Extended TabControl which saves the displayed item so you don't get the performance hit of
    /// unloading and reloading the VisualTree when switching tabs
    /// </summary>
    /// <remarks>
    /// Mainly this custom classes is defined to overcome the following problem. 
    /// When ChromiumWebBrowser is rendered in a tab control which automatically reloaded on each tab item navigation.
    /// Due to this, WebBrowser property in the view model gets null value.
    /// Reference: http://stackoverflow.com/a/9802346
    /// </remarks>
    [TemplatePart(Name = "PART_ItemsHolder", Type = typeof(Panel))]
    public class TabControlExtended : TabControl
    {
        private Panel _itemsContainerPanel;

        /// <summary>
        /// Initializes the extended tab control.
        /// </summary>
        public TabControlExtended()
        {
            // This is necessary so that we get the initial databound selected item
            ItemContainerGenerator.StatusChanged += ItemContainerGeneratorStatusChanged;
        }

        /// <summary>
        /// Get the ItemsHolder and generate any children
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _itemsContainerPanel = GetTemplateChild("PART_ItemsHolder") as Panel;
            UpdateSelectedItem();
        }

        #region Protected Methods
        /// <summary>
        /// When the items change we remove any generated panel children and add any new ones as necessary
        /// </summary>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (_itemsContainerPanel == null)
            {
                return;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    _itemsContainerPanel.Children.Clear();
                    break;

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (var item in e.OldItems)
                        {
                            var cp = FindChildContentPresenter(item);
                            if (cp != null)
                            {
                                _itemsContainerPanel.Children.Remove(cp);
                                cp.Content = null;
                            }
                        }
                    }

                    // Don't do anything with new items because we don't want to
                    // create visuals that aren't being shown

                    UpdateSelectedItem();
                    break;

                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException("Replace not implemented yet");
            }
        }

        /// <summary>
        /// When selection change is raised this will update the selected item.
        /// </summary>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            UpdateSelectedItem();
        }

        /// <summary>
        /// Gets the selected tab item.
        /// </summary>
        /// <returns></returns>
        public TabItem GetSelectedTabItem()
        {
            var selectedItem = SelectedItem;
            if (selectedItem == null)
            {
                return null;
            }

            var item = selectedItem as TabItem ?? ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as TabItem;

            return item;
        }

        /// <summary>
        /// Overrides OnCreateAutomationPeer
        /// </summary>
        /// <returns>AutomationPeer</returns>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new TabControlAutomationPeer(this);
        }
        #endregion

        #region Private Methods
        // If containers are done, generate the selected item
        private void ItemContainerGeneratorStatusChanged(object sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                ItemContainerGenerator.StatusChanged -= ItemContainerGeneratorStatusChanged;
                UpdateSelectedItem();
            }
        }

        private void UpdateSelectedItem()
        {
            if (_itemsContainerPanel == null)
            {
                return;
            }

            // Generate a ContentPresenter if necessary
            var item = GetSelectedTabItem();
            if (item != null)
            {
                CreateChildContentPresenter(item);
            }

            // show the right child
            foreach (ContentPresenter child in _itemsContainerPanel.Children)
            {
                child.Visibility = ((child.Tag as TabItem).IsSelected) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private ContentPresenter CreateChildContentPresenter(object item)
        {
            if (item == null)
            {
                return null;
            }

            var cp = FindChildContentPresenter(item);

            if (cp != null)
            {
                return cp;
            }

            var tabItem = item as TabItem;
            cp = new ContentPresenter
            {
                Content = (tabItem != null) ? tabItem.Content : item,
                ContentTemplate = this.SelectedContentTemplate,
                ContentTemplateSelector = this.SelectedContentTemplateSelector,
                ContentStringFormat = this.SelectedContentStringFormat,
                Visibility = Visibility.Collapsed,
                Tag = tabItem ?? (this.ItemContainerGenerator.ContainerFromItem(item))
            };
            _itemsContainerPanel.Children.Add(cp);
            return cp;
        }

        private ContentPresenter FindChildContentPresenter(object data)
        {
            if (data is TabItem)
            {
                data = (data as TabItem).Content;
            }

            if (data == null)
            {
                return null;
            }

            if (_itemsContainerPanel == null)
            {
                return null;
            }

            foreach (ContentPresenter cp in _itemsContainerPanel.Children)
            {
                if (cp.Content == data)
                {
                    return cp;
                }
            }

            return null;
        }
        #endregion
    }
}
