using System;
using System.Collections.Generic;
using System.Linq;
using HHDev.DataManagement.Client.Core.Models.FlatModels;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Reflection;

namespace HHDev.HHDM.DesktopPlugin.Setup
{
    public class HHDevSetupFlatModel : SetupFlatModel
    {
        public HHDevSetupFlatModel(SetupFlatModelInitializationObject initializationObject, TypeWrapperInitializationContainerWithParts typeWrapperInitializationContainer) : base(initializationObject, typeWrapperInitializationContainer)
        {
            var flatModelMathFunctionCaller = new FlatModelMathFunctionCaller(this);
            flatModelMathFunctionCaller.RegisterParameterAndFunction( // HH DM function with null checks. UpdateRakeCalculation will be called when one of the input parameters changed and if all are non-null
                new string[] { "RideHeightFL", "RideHeightFR", "RideHeightRL", "RideHeightRR", "Chassis" },//input parameters
                UpdateRakeCalculation, //Function called
                new string[] { nameof(RakeAngle) });// parameter updated

            this.PropertyChanged += HHDevRunSheetFlatModel_PropertyChanged;
            this.Maths.PropertyChanged += Maths_PropertyChanged;
            InitializeSimulationValues();
        }

        public List<string> _parametersToUpdateSimulation = new List<string>()
            {
                "CamberFL",
                "CamberFR"
            };
    private void InitializeSimulationValues()
        {


            var array1 = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var array2 = new List<double>();
            for (int i = 0; i < array1.Length; i++)
            {
                array2.Add(Math.Pow(array1[i], 2));
            }

            _simulationResults = new SimulationArraysContainer(array1, array2.ToArray());
        }
        private void Maths_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) //called when a Math parameter from the setup definition changed
        {
            if (e.PropertyName == "NameOfSetupMathsParameter")
            {
                //call function : Important to add null checks
            }
            if (!_parametersToUpdateSimulation.Contains(e.PropertyName))
            {
                return;
            }
            InitializeSimulationValues();//call it your simulation calculation
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


        #region Graphs
        public event EventHandler<EventArgs> SimulationResultsUpdated;
        public SimulationArraysContainer SimulationResults => _simulationResults;
        private SimulationArraysContainer _simulationResults;

        private PropertyInfo[] _simulationParameters = typeof(SimulationArraysContainer).GetProperties();

        public class SimulationArraysContainer
        {
            public SimulationArraysContainer(double[] testArray1,
                                             double[] testArray2)
            {
                TestArray1 = testArray1;
                TestArray2 = testArray2;
            }

            [Display(Name = "TestArray1", GroupName = "TestArray1 [unit]")]
            public double[] TestArray1 { get; set; }
            [Display(Name = "TestArray2", GroupName = "TestArray1 [unit]")]
            public double[] TestArray2 { get; set; }
        }
        protected void OnSimulationResultsUpdated()
        {
            SimulationResultsUpdated?.Invoke(this, new EventArgs());
        }

        public double[] GetSimulationResultByName(string parameter)
        {
            if (_simulationParameters.Any(x => x.GetCustomAttribute<DisplayAttribute>().Name == parameter) &&
                _simulationResults != null)
            {
                var propertyInfo = _simulationParameters.First(x => x.GetCustomAttribute<DisplayAttribute>().Name == parameter);
                if (typeof(double[]).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    return (double[])propertyInfo.GetValue(_simulationResults);
                }
            }
            return null;
        }
        #endregion
    }

}
