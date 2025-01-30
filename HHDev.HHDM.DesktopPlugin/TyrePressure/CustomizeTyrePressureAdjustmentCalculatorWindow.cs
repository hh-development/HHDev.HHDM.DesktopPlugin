using HHDev.DataManagement.Client.Core.ViewModels.Engineering.TyreSets;
using HHDev.DataManagement.Client.Wpf.Views.Engineering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace HHDev.HHDM.DesktopPlugin.TyrePressure
{
    public class CustomizeTyrePressureAdjustmentCalculatorWindow : TyrePressureAdjustmentCalculatorWindow
    {
        public CustomizeTyrePressureAdjustmentCalculatorWindow(TyrePressureAdjustmentCalculatorWindowInitializationObject initObject)
           : base(initObject)
        {

        }

        protected override double? CalculateNewCold(Pressure targetHot,
                                                   Pressure referenceHot,
                                                   Pressure referenceCold,
                                                   Temperature referenceTyreTemp,
                                                   Temperature referenceAirTemp,
                                                   Temperature referenceTrackTemp,
                                                   Temperature currentTyreTemp,
                                                   Temperature expectedAirTemp,
                                                   Temperature expectedTrackTemp)
        {


            var bleedP = CustomizeTyrePressureCalculation.CalculatePressureAdjustment(targetHot,
                referenceHot,
                referenceCold,
                referenceTyreTemp,
                referenceAirTemp,
                referenceTrackTemp,
                currentTyreTemp,
                expectedAirTemp,
                expectedTrackTemp);
            if (bleedP == null)
            {
                return null;
            }

            return CustomizeTyrePressureCalculation.GetPressureValueFromBarToUnit(_eventCarMasterCache, bleedP.Value);


        }
    }
}