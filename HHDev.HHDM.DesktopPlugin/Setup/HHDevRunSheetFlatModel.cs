using System;
using System.Collections.Generic;
using System.Linq;
using HHDev.DataManagement.Client.Core.Models.FlatModels;
using System.Text;
using System.Threading.Tasks;

namespace HHDev.HHDM.DesktopPlugin.Setup
{
    public class HHDevRunSheetFlatModel : SetupFlatModel
    {
        public HHDevRunSheetFlatModel(SetupFlatModelInitializationObject initializationObject, TypeWrapperInitializationContainerWithParts typeWrapperInitializationContainer) : base(initializationObject, typeWrapperInitializationContainer)
        {
            base.RegisterParameterAndFunction( // HH DM function with null checks. UpdateRakeCalculation will be called when one of the input parameters changed and if all are non-null
                new string[] { "RideHeightFL", "RideHeightFR", "RideHeightRL", "RideHeightRR", "Chassis" },//input parameters
                UpdateRakeCalculation, //Function called
                new string[] { nameof(RakeAngle) });// parameter updated
            this.PropertyChanged += HHDevRunSheetFlatModel_PropertyChanged;
            this.Maths.PropertyChanged += Maths_PropertyChanged;
        }

        private void Maths_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) //called when a Math parameter from the setup definition changed
        {
            if (e.PropertyName == "NameOfSetupMathsParameter")
            {
                //call function : Important to add null checks
            }
        }

        private void HHDevRunSheetFlatModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) // call when one parameter from the setup definition has changed (parameter or part parameter)
        {
            if (e.PropertyName == "NameOfSetupParameter")
            {
                //call function : Important to add null checks
            }
        }

        private void UpdateRakeCalculation()
        {
            var rhFL = Doubles.GetPropertyValue("RideHeightFL");
            var rhFR = Doubles.GetPropertyValue("RideHeightFR");
            var rhRL = Doubles.GetPropertyValue("RideHeightRL");
            var rhRR = Doubles.GetPropertyValue("RideHeightRR");
            var chassis = Parts.GetPropertyValue("Chassis");

            var rhF = (rhFL + rhFR) / 2;
            var rhR = (rhRL + rhRR) / 2;

            var wb = chassis.Doubles.GetPropertyValue("Wheelbase");
            if (!wb.HasValue)
            {
                RakeAngle = null;
                return;
            }
            RakeAngle =Math.Asin((double)((rhR - rhF) / wb))*180/Math.PI;
        }

        public double? RakeAngle { get; set; }
    }
}
