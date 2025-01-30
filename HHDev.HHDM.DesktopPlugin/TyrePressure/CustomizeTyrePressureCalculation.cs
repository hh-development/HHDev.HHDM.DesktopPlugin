using HHDev.Core.NETStandard.Extensions;
using HHDev.DataManagement.Client.Core.Caches;
using HHDev.DataManagement.Client.Core.Models;
using HHDev.DataManagement.Client.Core.Models.FlatModels;
using HHDev.DataManagement.Core;
using System;
using System.Linq;
using UnitsNet;

namespace HHDev.HHDM.DesktopPlugin.TyrePressure
{
    public static class CustomizeTyrePressureCalculation
    {
        public static double? CalculatePressureFromRef
            (Pressure targetP, Pressure refHotP, Pressure refP, Temperature refTyreT, Temperature refAirT, Temperature refTrackT, Temperature currTyreT, Temperature expectAirT, Temperature expectTrackT)
        {
            Temperature hotTireAirTerature = Temperature.FromKelvins(((refHotP.Bars * 100) + CONSTANT1) * (refTyreT.Kelvins) / ((refP.Bars * 100) + CONSTANT1));
            double percentTrack = 100;
            double percentAmbient = 100;
            if (expectTrackT.DegreesCelsius + expectAirT.DegreesCelsius != 0)
            {
                percentTrack = expectTrackT.DegreesCelsius / (expectTrackT.DegreesCelsius + expectAirT.DegreesCelsius) * 100;
                percentAmbient = expectAirT.DegreesCelsius / (expectTrackT.DegreesCelsius + expectAirT.DegreesCelsius) * 100;
            }
            double percentTrack2 = expectTrackT.DegreesCelsius * percentTrack / 100;
            double percentAmbient2 = expectAirT.DegreesCelsius * percentAmbient / 100;
            double trackVsAmbient = percentTrack2 + percentAmbient2;

            double airAndTarmacrefRun = refAirT.DegreesCelsius + refTrackT.DegreesCelsius;
            double percentAmbientrefRun = 100;
            double percentTrackrefRun = 100;
            if (airAndTarmacrefRun != 0)
            {
                percentAmbientrefRun = refAirT.DegreesCelsius / airAndTarmacrefRun * 100;
                percentTrackrefRun = refTrackT.DegreesCelsius / airAndTarmacrefRun * 100;
            }

            double percentAmbientrefRun2 = refAirT.DegreesCelsius * percentAmbientrefRun / 100;
            double percentTrackrefRun2 = refTrackT.DegreesCelsius * percentTrackrefRun / 100;
            double trackVsAmbient2 = percentAmbientrefRun2 + percentTrackrefRun2;

            double targetVsrefRun = trackVsAmbient - trackVsAmbient2;
            Temperature targetVsrefRunCompensated = Temperature.FromDegreesCelsius(targetVsrefRun * CONSTANT2);

            Temperature hotTireAirTeraturecompensated = Temperature.FromDegreesCelsius(hotTireAirTerature.DegreesCelsius + targetVsrefRunCompensated.DegreesCelsius);
            if (hotTireAirTeraturecompensated.Kelvins == 0)
            {
                return null;
            }
            var ColdPressureBar = ((((targetP.Bars * 100) + CONSTANT1) * (currTyreT.Kelvins) / (hotTireAirTeraturecompensated.Kelvins)) - CONSTANT1) / 100;
            return ColdPressureBar;
        }

        public static double? CalculatePressureAdjustment
                            (Pressure targetP, Pressure refHotP, Pressure refP, Temperature refTyreT, Temperature refAirT, Temperature refTrackT, Temperature currTyreT, Temperature expectAirT, Temperature expectTrackT)
        {
            // this function is used for a quick adjustment without measuring the pressure before the bleed. If we wnat later to add a claculation with a measurement of the tyre pressure after over for exemple, we will need to calculate the actual tyre temperature depending of the actual pressure. 
            //Temperature currTyreTcalc;

            //  currTyreTcalc = CalculateActualTemp(currTyreP, refP, refTyreT); // if user enter actual P but no T, we calculate the theortical Temperature.


            var tempRefPAtcurrTyreT = CalculatePressureFromRef(refHotP, refHotP, refP, refTyreT, refAirT, refTrackT, currTyreT, refAirT, refTrackT);//Calcul Ref Pressure @ actual Tyre Temp
            if (tempRefPAtcurrTyreT == null)
            {
                return null;
            }
            var TempNewPAtExpectT = CalculatePressureFromRef(targetP, refHotP, Pressure.FromBars(tempRefPAtcurrTyreT.Value), currTyreT, refAirT, refTrackT, currTyreT, expectAirT, expectTrackT);//Calcul Pressure @ actual Tyre Temp, New Target, New Track & Air T

            return TempNewPAtExpectT.Value - tempRefPAtcurrTyreT.Value;
        }

        public static Temperature CalculateActualTemp(Pressure refHotP, Pressure refP, Temperature refTyreT)
        {
            if (refP.Bars == 0)
            {
                return refTyreT;
            }
            var currTemp = Temperature.FromKelvins(refHotP * refTyreT.Kelvins / refP);
            return currTemp;
        }

        public static void SetGenericValuesForReferencePressure(TyreSetPressureAdjustmentModel referencePressure,
                                                                IEventCarDataFlatModel eventObject)
        {
            referencePressure.ReferenceTemperature = eventObject.Doubles.GetPropertyValue("RefPressureTyreTemp");

            referencePressure.FLValue2 = eventObject.Doubles.GetPropertyValue("RefPressureTargetFL");
            referencePressure.FRValue2 = eventObject.Doubles.GetPropertyValue("RefPressureTargetFR");
            referencePressure.RLValue2 = eventObject.Doubles.GetPropertyValue("RefPressureTargetRL");
            referencePressure.RRValue2 = eventObject.Doubles.GetPropertyValue("RefPressureTargetRR");

            referencePressure.ReferenceTemperature2 = eventObject.Doubles.GetPropertyValue("RefPressureAirTemp");
            referencePressure.ReferenceTemperature3 = eventObject.Doubles.GetPropertyValue("RefPressureTrackTemp");
        }

        public static double? CalculateReferenceForColdHelper(string cornerString,
                                                                IEventCarDataFlatModel eventObject,
                                                                IBasicEntityFlatModel pressureAdjustment)
        {
            var refTarget = eventObject.Doubles.GetInternalPropertyValue($"RefPressureTarget{cornerString}");
            var refHotP = pressureAdjustment.Doubles.GetInternalPropertyValue($"{cornerString}Value2");
            var refColdP = pressureAdjustment.Doubles.GetInternalPropertyValue($"{cornerString}Value");
            var refTyreT = pressureAdjustment.Doubles.GetInternalPropertyValue("ReferenceTemperature");
            var refAirT = pressureAdjustment.Doubles.GetInternalPropertyValue("ReferenceTemperature2");
            var refTrackT = pressureAdjustment.Doubles.GetInternalPropertyValue("ReferenceTemperature3");
            var actualTyreT = eventObject.Doubles.GetInternalPropertyValue("RefPressureTyreTemp");
            var expectAirT = eventObject.Doubles.GetInternalPropertyValue("RefPressureAirTemp");
            var expectTrackT = eventObject.Doubles.GetInternalPropertyValue("RefPressureTrackTemp");

            if (refTarget == null || refHotP == null || refColdP == null || refTyreT == null || refAirT == null || refTrackT == null || actualTyreT == null || expectAirT == null || expectTrackT == null)
            {
                return null;
            }
            return CalculatePressureFromRef(Pressure.FromBars(refTarget.Value),
                                                                                      Pressure.FromBars(refHotP.Value),
                                                                                       Pressure.FromBars(refColdP.Value),
                                                                                       Temperature.FromDegreesCelsius(refTyreT.Value),
                                                                                       Temperature.FromDegreesCelsius(refAirT.Value),
                                                                                       Temperature.FromDegreesCelsius(refTrackT.Value),
                                                                                       Temperature.FromDegreesCelsius(actualTyreT.Value),
                                                                                       Temperature.FromDegreesCelsius(expectAirT.Value),
                                                                                       Temperature.FromDegreesCelsius(expectTrackT.Value));

        }

        public static double? CalculateReferenceForAdjustmentHelper(string cornerString,
                                                                IEventCarDataFlatModel eventObject,
                                                                IBasicEntityFlatModel pressureAdjustment,
                                                                TyreSetPressureAdjustmentModel previousReference)
        {
            if (pressureAdjustment.Doubles.GetPropertyValue("ReferenceTemperature") == 0)
            {
                return null;
            }
            var tempRatio = eventObject.Doubles.GetInternalPropertyValue("RefPressureTyreTemp") / pressureAdjustment.Doubles.GetInternalPropertyValue("ReferenceTemperature");
            if (previousReference.GetPropertyValue($"{cornerString}Value") == null)
            {
                return null;
            }
            var previousRefPressure = GetPressureValueFromBarToUnit(eventObject.EventCarMasterCache, GetPressureWithUnit(eventObject.EventCarMasterCache, (double)previousReference.GetPropertyValue($"{cornerString}Value")).Bars);
            var referenceAdjustment1 = previousRefPressure + (pressureAdjustment.Doubles.GetInternalPropertyValue($"{cornerString}Value") * tempRatio);
            if (referenceAdjustment1 == null)
            {
                return null;
            }
            var referenceAdjustment1Pressure = GetPressureWithUnit(eventObject.EventCarMasterCache, (double)referenceAdjustment1);
            var refTarget = eventObject.Doubles.GetInternalPropertyValue($"RefPressureTarget{cornerString}");
            var refHotP = pressureAdjustment.Doubles.GetInternalPropertyValue($"{cornerString}Value2");
            var refColdP = pressureAdjustment.Doubles.GetInternalPropertyValue($"{cornerString}Value");
            var refAirT = pressureAdjustment.Doubles.GetInternalPropertyValue("ReferenceTemperature2");
            var refTrackT = pressureAdjustment.Doubles.GetInternalPropertyValue("ReferenceTemperature3");
            var actualTyreT = eventObject.Doubles.GetInternalPropertyValue("RefPressureTyreTemp");
            var expectAirT = eventObject.Doubles.GetInternalPropertyValue("RefPressureAirTemp");
            var expectTrackT = eventObject.Doubles.GetInternalPropertyValue("RefPressureTrackTemp");

            if (refTarget == null || refHotP == null || refColdP == null || refAirT == null || refTrackT == null || actualTyreT == null || expectAirT == null || expectTrackT == null)
            {
                return null;
            }



            return CalculatePressureFromRef(Pressure.FromBars(refTarget.Value),
                                                                 Pressure.FromBars(refHotP.Value),
                                                                 referenceAdjustment1Pressure.ToUnit(UnitsNet.Units.PressureUnit.Bar),
                                                                 Temperature.FromDegreesCelsius(actualTyreT.Value),
                                                                 Temperature.FromDegreesCelsius(refAirT.Value),
                                                                 Temperature.FromDegreesCelsius(refTrackT.Value),
                                                                 Temperature.FromDegreesCelsius(actualTyreT.Value),
                                                                 Temperature.FromDegreesCelsius(expectAirT.Value),
                                                                 Temperature.FromDegreesCelsius(expectTrackT.Value));

        }

        // NOTE: that for now this is the same as the CalculateReferenceForColdHelper function.  We could just reuse that calculation if that
        // does end up being the case
        public static double? CalculateReferenceForMeasurementHelper(string cornerString,
                                                                IEventCarDataFlatModel eventObject,
                                                                IBasicEntityFlatModel pressureAdjustment)
        {
            var refTarget = eventObject.Doubles.GetInternalPropertyValue($"RefPressureTarget{cornerString}");
            var refHotP = pressureAdjustment.Doubles.GetInternalPropertyValue($"{cornerString}Value2");
            var refColdP = pressureAdjustment.Doubles.GetInternalPropertyValue($"{cornerString}Value");
            var refTyreT = pressureAdjustment.Doubles.GetInternalPropertyValue("ReferenceTemperature");
            var refAirT = pressureAdjustment.Doubles.GetInternalPropertyValue("ReferenceTemperature2");
            var refTrackT = pressureAdjustment.Doubles.GetInternalPropertyValue("ReferenceTemperature3");
            var actualTyreT = eventObject.Doubles.GetInternalPropertyValue("RefPressureTyreTemp");
            var expectAirT = eventObject.Doubles.GetInternalPropertyValue("RefPressureAirTemp");
            var expectTrackT = eventObject.Doubles.GetInternalPropertyValue("RefPressureTrackTemp");

            if (refTarget == null || refHotP == null || refColdP == null || refTyreT == null || refAirT == null || refTrackT == null || actualTyreT == null || expectAirT == null || expectTrackT == null)
            {
                return null;
            }
            return CalculatePressureFromRef(Pressure.FromBars(refTarget.Value),
                                                                                      Pressure.FromBars(refHotP.Value),
                                                                                       Pressure.FromBars(refColdP.Value),
                                                                                       Temperature.FromDegreesCelsius(refTyreT.Value),
                                                                                       Temperature.FromDegreesCelsius(refAirT.Value),
                                                                                       Temperature.FromDegreesCelsius(refTrackT.Value),
                                                                                       Temperature.FromDegreesCelsius(actualTyreT.Value),
                                                                                       Temperature.FromDegreesCelsius(expectAirT.Value),
                                                                                       Temperature.FromDegreesCelsius(expectTrackT.Value));
        }

        public static double? CalculateReferenceForRunSheetHelper(string cornerString,
                                                                  IEventCarDataFlatModel eventObject,
                                                                  IRunSheetFlatModel runSheet,
                                                                  TyreSetPressureAdjustmentModel previousReference)
        {
            var hotP = runSheet.Doubles.GetInternalPropertyValue($"{cornerString}HotPressure");
            var previousColdRefP = (double?)previousReference.GetPropertyValue($"{cornerString}Value");

            var refTarget = eventObject.Doubles.GetInternalPropertyValue($"RefPressureTarget{cornerString}");
            var refAirT = runSheet.Maths.GetPropertyValue("CalculatedAirTemperature") as double?;
            var refTrackT = runSheet.Maths.GetPropertyValue("CalculatedTrackTemperature") as double?;
            var actualTyreT = eventObject.Doubles.GetPropertyValue("RefPressureTyreTemp");
            var expectAirT = eventObject.Doubles.GetInternalPropertyValue("RefPressureAirTemp");
            var expectTrackT = eventObject.Doubles.GetInternalPropertyValue("RefPressureTrackTemp");

            if (refTarget == null || hotP == null || previousColdRefP == null || refAirT == null || refTrackT == null || actualTyreT == null || expectAirT == null || expectTrackT == null)
            {
                return null;
            }

            var hotPressure = Pressure.FromBars(hotP.Value);

            var referenceTyreTemperature = PressureCalculations.CalculateActualTemp(hotPressure,
                                                                                    GetPressureWithUnit(eventObject.EventCarMasterCache, previousColdRefP.Value).ToUnit(UnitsNet.Units.PressureUnit.Bar),
                                                                                    Temperature.FromDegreesCelsius(eventObject.Doubles.GetInternalPropertyValue("RefPressureTyreTemp").Value));


            return PressureCalculations.CalculatePressureFromRef(Pressure.FromBars(eventObject.Doubles.GetInternalPropertyValue($"RefPressureTarget{cornerString}").Value),
                                                                 hotPressure,
                                                                 GetPressureWithUnit(eventObject.EventCarMasterCache, previousColdRefP.Value),
                                                                 referenceTyreTemperature,
                                                                 Temperature.FromDegreesCelsius(refAirT.Value),
                                                                 Temperature.FromDegreesCelsius(refTrackT.Value),
                                                                 Temperature.FromDegreesCelsius(actualTyreT.Value),
                                                                 Temperature.FromDegreesCelsius(expectAirT.Value),
                                                                 Temperature.FromDegreesCelsius(expectTrackT.Value));
        }
        public static Pressure GetPressureWithUnit(IEventCarMasterCache _eventCarMasterCache, double value)
        {
            var defn = _eventCarMasterCache.ContextSelection.SelectedManagementCache.CustomPropertyDefinitionCache.GetCustomPropertyDefinition(eCustomPropertyDefinitionType.TyreSetPressureAdjustment);
            var unit = defn.CustomPropertyDescriptions.FirstOrDefault(x => x.Name == "FLValue").Unit;
            return Pressure.From(value, Pressure.ParseUnit(unit));
        }

        public static Temperature GetTemperatureWithUnit(IEventCarMasterCache _eventCarMasterCache, double value)
        {
            var defn = _eventCarMasterCache.ContextSelection.SelectedManagementCache.CustomPropertyDefinitionCache.GetCustomPropertyDefinition(eCustomPropertyDefinitionType.TyreSetPressureAdjustment);
            var unit = defn.CustomPropertyDescriptions.FirstOrDefault(x => x.Name == "ReferenceTemperature").Unit;
            return Temperature.From(value, Temperature.ParseUnit(unit));
        }

        public static double GetPressureValueFromBarToUnit(IEventCarMasterCache _eventCarMasterCache, double value)
        {
            var defn = _eventCarMasterCache.ContextSelection.SelectedManagementCache.CustomPropertyDefinitionCache.GetCustomPropertyDefinition(eCustomPropertyDefinitionType.TyreSetPressureAdjustment);
            var unit = defn.CustomPropertyDescriptions.FirstOrDefault(x => x.Name == "FLValue").Unit;
            return Pressure.From(value, Pressure.ParseUnit("bar")).ToUnit(Pressure.ParseUnit(unit)).Value;
        }

        public static double GetTemperatureValueFromCelciusToUnit(IEventCarMasterCache _eventCarMasterCache, double value)
        {
            var defn = _eventCarMasterCache.ContextSelection.SelectedManagementCache.CustomPropertyDefinitionCache.GetCustomPropertyDefinition(eCustomPropertyDefinitionType.TyreSetPressureAdjustment);
            var unit = defn.CustomPropertyDescriptions.FirstOrDefault(x => x.Name == "ReferenceTemperature").Unit;
            return Temperature.From(value, Temperature.ParseUnit("\u00B0C")).ToUnit(Temperature.ParseUnit(unit)).Value;
        }

    }
}
