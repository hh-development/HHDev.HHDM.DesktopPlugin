using HHDev.Core.NETStandard.Enums;
using HHDev.Core.NETStandard.Src;
using HHDev.DataManagement.Client.Core;
using HHDev.DataManagement.Client.Core.Caches;
using HHDev.DataManagement.Client.Core.Models.FlatModels;
using HHDev.DataManagement.Client.Core.ViewModels;
using HHDev.DataManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HHDev.HHDM.DesktopPlugin.ContextSelectionView
{
    public class ExampleContextSelectionViewModel : BaseEventCarCacheViewModel
    {
        public IRunSheetCache RunSheetCache { get; private set; }
        public IRunSheetFlatModel SelectedRunSheetModel { get; set; }
        public ICommand DoYourFunctionCommand { get; set; }
        protected override string GetDisplayNameForNewContext()
        {
            return "Context Selection";
        }

        public ExampleContextSelectionViewModel(IClientCoreContainer container)
            : base(new ContextCacheConfigOptionsV2(eContextDisplayMode.All, new FilterOptionsV2(eEventFilterLevelV2.Subscribed, eCarFilterLevelV2.Subscribed)), 
                   "Event.Runs", 
                   container)
        {
            DoYourFunctionCommand = new DelegateCommand(DoYourFunction);
        }

        private void DoYourFunction()
        {
            //add code to be run when clicking button in view
        }

        protected override void BeforeEventCarCacheSetToNull()
        {
            base.BeforeEventCarCacheSetToNull();
            RunSheetCache = null;
        }

        protected override void HandleNewEventCarCacheArrived()
        {
            base.HandleNewEventCarCacheArrived();

            if (ContextSelection.SelectedSession == null)
            {
                return;
            }

            RunSheetCache = EventCarCache.GetRunSheetCache(ContextSelection.SelectedSession.Id);

            if (RunSheetCache != null)
            {
                SelectedRunSheetModel = RunSheetCache.RunSheets.FirstOrDefault();
            }
        }

        protected override void HandleSessionContextChange()
        {
            base.HandleSessionContextChange();

            if (EventCarCache == null)
            {
                return;
            }

            RunSheetCache = EventCarCache.GetRunSheetCache(ContextSelection.SelectedSession.Id);

            if (RunSheetCache != null)
            {
                SelectedRunSheetModel = RunSheetCache.RunSheets.FirstOrDefault();
            }
        }
    }
}
