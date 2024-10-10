using HHDev.DataManagement.Client.Core;
using HHDev.DataManagement.Client.Core.ViewModels;
using HHDev.DataManagement.Client.Wpf;
using HHDev.DataManagement.Client.Wpf.PluginFramework.CustomizationConfigs;
using HHDev.DataManagement.Client.Wpf.Services;
using HHDev.DataManagement.Client.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHDev.HHDM.DesktopPlugin.RunSheet
{
    internal class PluginRunSheetViewModel : WpfRunSheetsViewModel
    {
        public PluginRunSheetViewModel(PluginCustomizationContainer<RunSheetCustomizationConfig> runSheetCustomization, 
                                       PluginCustomizationContainer<LapCustomizationConfig> lapCustomization,
                                       IWpfClientContainer container)
            : base(runSheetCustomization, 
                   lapCustomization, 
                   container)
        {
        }
        protected override EditRunSheetViewModel GetEditRunSheetViewModel(EditRunSheetViewModelInitializationObject initObject)
        {
            return new PluginEditRunsheetViewModel(initObject);
        }
    }
}
