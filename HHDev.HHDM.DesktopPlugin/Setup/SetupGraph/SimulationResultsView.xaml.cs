using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Ribbon;
using HHDev.Core.WPF.Layout;
using HHDev.DataManagement.Client.Wpf.Controls.Ribbon;
using HHDev.DataManagement.Client.Wpf.Views;
using HHDev.DataManagement.Core;
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

namespace HHDev.HHDM.DesktopPlugin.Setup.SetupGraph
{
    /// <summary>
    /// Interaction logic for SimulationResultsView.xaml
    /// </summary>
    public partial class SimulationResultsView : BaseView, ILayoutDocument
    {
        private bool _isSetupCollapsed = false;
        private GridLength _originalGridLength;
        public SimulationResultsView()
        {
            InitializeComponent();
        }

        protected override IEnumerable<object> BuildRibbonPageGroups()
        {
            var itemManagementGroup = new HHDMRibbonPageGroup(eUiElementPermissionLevel.Read);
            itemManagementGroup.Caption = "Items";
            var graphGroup = new HHDMRibbonPageGroup(eUiElementPermissionLevel.None);
            graphGroup.Caption = "Graphs";

            var addSetupButton = new BarButtonItem() { Content = "Add Setup", Focusable = true };
            addSetupButton.SetBinding(BarButtonItem.CommandProperty, new Binding("DataContext.AddNewSetupCommand") { Source = this });

            var refreshGraphButton = new BarButtonItem() { Content = "Refresh Graph", Focusable = true };
            refreshGraphButton.SetBinding(BarButtonItem.CommandProperty, new Binding("DataContext.RefreshGraphCommand") { Source = this });

            itemManagementGroup.Items.Add(addSetupButton);
            graphGroup.Items.Add(refreshGraphButton);

            return new RibbonPageGroup[] { itemManagementGroup, graphGroup};
        }

        private void GridSplitter_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_isSetupCollapsed)
            {
                ScrollViewStackPanel.Visibility = System.Windows.Visibility.Visible;
                SetupColumn.Width = _originalGridLength;
            }
            else
            {
                ScrollViewStackPanel.Visibility = System.Windows.Visibility.Collapsed;
                _originalGridLength = SetupColumn.Width;
                SetupColumn.Width = GridLength.Auto;
            }
            _isSetupCollapsed = !_isSetupCollapsed;
        }
    }
}
