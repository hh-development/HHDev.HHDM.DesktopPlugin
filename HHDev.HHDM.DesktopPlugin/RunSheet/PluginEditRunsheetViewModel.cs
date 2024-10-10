using HHDev.Core.NETStandard.DynamicWrappers;
using HHDev.Core.NETStandard.Extensions;
using HHDev.DataManagement.Client.Core.Caches;
using HHDev.DataManagement.Client.Core.Components.ChangeTracking;
using HHDev.DataManagement.Client.Core.Helpers;
using HHDev.DataManagement.Client.Core.Models.FlatModels;
using HHDev.DataManagement.Client.Core.ViewModels;
using HHDev.DataManagement.Client.Wpf.ViewModels;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHDev.HHDM.DesktopPlugin.RunSheet
{
    [AddINotifyPropertyChangedInterface]
    public class RunSheetWrapper : IChangeTrackingEditViewModel
    {
        public RunSheetWrapper(IRunSheetFlatModel flatModel, IChangeTrackingModel changeTrackingModel, Guid ownerId)
        {
            FlatModel = flatModel;
            OwnerId = ownerId;
            ChangeTrackingModels = TypeWrapperCreator<IChangeTrackingModel>.GetTypeWrapper(FlatModel.DefinitionId);
            ChangeTrackingModels.AddProperty(nameof(RunSheetFlatModel), changeTrackingModel);
        }

        public IFlatModel FlatModel { get; }
        public ITypeWrapper<bool> ChangeFlags { get; }
        public Guid OwnerId { get; }
        public ITypeWrapper<IChangeTrackingModel> ChangeTrackingModels { get; }

        public string FlatModelId => FlatModel.Id;

        public void HandleCloseViewModel()
        {
            
        }
    }
    public class PluginEditRunsheetViewModel : WpfEditRunSheetViewModel
    {
        public IColumnItemDisplayDefinitionFlatModel EditColumnItemDefinitionModelRunSheetPluginCID { get; set; }
        public HHObservableCollection<RunSheetWrapper> RunSheetPluginCIDs { get; } = new HHObservableCollection<RunSheetWrapper>();

        public PluginEditRunsheetViewModel(EditRunSheetViewModelInitializationObject initObject) : base(initObject)
        {
            var runsheetPluginCIDDef = ContextCache.SelectedManagementCache
                                             .ColumnItemDisplayDefinitionCache.GetDefinitionByName("Plugin CID", RunSheetFlatModel.DefinitionId);
            if (runsheetPluginCIDDef != null)
            {
                EditColumnItemDefinitionModelRunSheetPluginCID = runsheetPluginCIDDef;
            }
            if (RunSheetFlatModel != null)
            {
                RunSheetPluginCIDs.Add(new RunSheetWrapper(RunSheetFlatModel, ChangeTrackingModels.GetPropertyValue(nameof(RunSheetFlatModel)), RunSheetsViewModel.Id));

            }
        }
    }
}