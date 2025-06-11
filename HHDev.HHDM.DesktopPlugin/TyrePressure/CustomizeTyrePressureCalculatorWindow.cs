using HHDev.DataManagement.Client.Core.Models;
using HHDev.DataManagement.Client.Wpf.PluginFramework.Interfaces;
using HHDev.DataManagement.Client.Wpf.Views.Engineering;
using System;

namespace HHDev.HHDM.DesktopPlugin.TyrePressure
{
    internal class CustomizeTyrePressureCalculatorWindow : IPressureCalculatorWindow
    {

        public CustomizeTyrePressureCalculatorWindow(TyrePressureCalculatorWindowInitializationObject initObject)
        {
            
        }

        public bool? ShowDialog()
        {
            throw new NotImplementedException();
        }

        public TyreSetPressureAdjustmentModel GetPressureAdjustment()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<EventArgs> RequestPerformBulkUpdate;

        protected virtual void OnRequestPerformBulkUpdate()
        {
            RequestPerformBulkUpdate?.Invoke(this, EventArgs.Empty);
        }
    }
}
