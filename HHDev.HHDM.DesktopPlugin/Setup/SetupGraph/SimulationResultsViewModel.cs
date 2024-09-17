using HHDev.Core.NETStandard.Definitions;
using HHDev.Core.NETStandard.Extensions;
using HHDev.Core.NETStandard.Graphing;
using HHDev.Core.NETStandard.Helpers;
using HHDev.Core.NETStandard.Logging;
using HHDev.Core.NETStandard.Src;
using HHDev.DataManagement.Client.Core;
using HHDev.DataManagement.Client.Core.Definitions.HHColumnItemDisplay;
using HHDev.DataManagement.Client.Core.Models.FlatModels;
using HHDev.DataManagement.Client.Core.ViewModels;
using HHDev.DataManagement.Client.Wpf.Services;
using HHDev.DataManagement.Client.Wpf.ViewModels;
using HHDev.DataManagement.Client.Wpf.ViewModels.Management.ColumnItemDefinitionEditor;
using HHDev.DataManagement.Core;
using HHDev.DataManagement.Core.Entities;
using NLog;
using OxyPlot;
using OxyPlot.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using static HHDev.HHDM.DesktopPlugin.Setup.HHDevSetupFlatModel;

namespace HHDev.HHDM.DesktopPlugin.Setup.SetupGraph
{
    public class SimulationResultsViewModel : WpfSetupComparisonViewModel
    {
        protected static ExtendedLogger _logger = (ExtendedLogger)LogManager.GetCurrentClassLogger(typeof(ExtendedLogger));
        public ICommand RefreshGraphCommand { get; set; }

        private bool _isLoaded = false;

        public HHOxyPlotModel PlotModel1 { get; set; }
        public HHOxyPlotModel PlotModel2 { get; set; }
        public HHOxyPlotModel PlotModel3 { get; set; }
        public HHOxyPlotModel PlotModel4 { get; set; }
        public HHOxyPlotModel PlotModel5 { get; set; }
        public HHOxyPlotModel PlotModel6 { get; set; }

        private IEnumerable<DisplayAttribute> _allProperties = typeof(SimulationArraysContainer).GetProperties().Select(x => x.GetCustomAttribute<DisplayAttribute>());
        private IEnumerable<string> _simulationParameters = typeof(SimulationArraysContainer).GetProperties().Select(x => x.GetCustomAttribute<DisplayAttribute>().Name);
        public IEnumerable<string> SimulationParameters => _simulationParameters;

        private double? _selectedSpeed;
        public double? SelectedSpeed
        {
            get => _selectedSpeed;
            set
            {
                if (_selectedSpeed == value)
                {
                    return;
                }
                _selectedSpeed = value;
                RefreshGraphs();
            }
        }
        

        private string _selectedXaxis1;
        private string _selectedYaxis1;
        private string _selectedXaxis2;
        private string _selectedYaxis2;
        public string SelectedXaxis1
        {
            get => _selectedXaxis1;
            set
            {
                if (_selectedXaxis1 == value)
                {
                    return;
                }
                _selectedXaxis1 = value;
                RefreshUserDefinedGraphs();
            }
        }
        public string SelectedYaxis1
        {
            get => _selectedYaxis1;
            set
            {
                if (_selectedYaxis1 == value)
                {
                    return;
                }
                _selectedYaxis1 = value;
                RefreshUserDefinedGraphs();
            }
        }
        public string SelectedXaxis2
        {
            get => _selectedXaxis2;
            set
            {
                if (_selectedXaxis2 == value)
                {
                    return;
                }
                _selectedXaxis2 = value;
                RefreshUserDefinedGraphs();
            }
        }
        public string SelectedYaxis2
        {
            get => _selectedYaxis2;
            set
            {
                if (_selectedYaxis2 == value)
                {
                    return;
                }
                _selectedYaxis2 = value;
                RefreshUserDefinedGraphs();
            }
        }
        public SimulationResultsViewModel(IEntitySelectionService entitySelectionService, 
                                          IClientCoreContainer container)
            : base(null, 
                   entitySelectionService, 
                   container)
        {
            DisplayName = "Simulation Results";
            PlotModel1 = InitializePlotModel("Array 2", "Array 2 [unit]", true, true);
            PlotModel2 = InitializePlotModel("Array 2", "Array 2 [unit]", false, true);
            PlotModel3 = InitializePlotModel("Array 2", "Array 2 [unit]", false, true);
            PlotModel4 = InitializePlotModel("Array 2", "Array 2 [unit]", false, true);
            PlotModel5 = InitializePlotModel("", "", false, false);
            PlotModel6 = InitializePlotModel("", "", false, false);

            RefreshGraphCommand = new DelegateCommand(RefreshGraphs);
            _controlName = "Simulation";
        }

        protected override void AddNewSetup()
        {
            base.AddNewSetup();
            RefreshGraphs();

            if (!_isLoaded)
            {
                if (CIDViewModelManager.ColumnItemDefinitionModel == null)
                {
                    return;
                }
                OnPropertyChanged(nameof(CIDViewModelManager.SelectedView));
                _isLoaded = true;
            }
        }

        

        protected override void AddExtraHandlersToEditViewModel(EditSetupSheetViewModel vm)
        {
            base.AddExtraHandlersToEditViewModel(vm);
            ((HHDevSetupFlatModel)vm.FlatModel).SimulationResultsUpdated += SimulationResultsViewModel_SimulationResultsUpdated;
            RefreshGraphs();
        }

        private void SimulationResultsViewModel_SimulationResultsUpdated(object sender, EventArgs e)
        {
            RefreshGraphs();
        }

        protected override void RemoveExtraHandlersFromEditViewModel(EditSetupSheetViewModel vm)
        {
            base.RemoveExtraHandlersFromEditViewModel(vm);
            ((HHDevSetupFlatModel)vm.FlatModel).SimulationResultsUpdated -= SimulationResultsViewModel_SimulationResultsUpdated;
            RefreshGraphs();
        }
        #region Graphs

        private void RefreshGraphs()
        {
            ClearAllGraphs();

            int index = 0;
            foreach (var rawSetup in _setups)
            {
                if (!(rawSetup is HHDevSetupFlatModel setup) ||
                    setup.SimulationResults?.TestArray1 == null)
                {
                    continue;
                }

                if (setup.SimulationResults.TestArray2 != null)
                {
                    AddSeriesToPlotModel(PlotModel1, setup.SimulationResults.TestArray1, setup.SimulationResults.TestArray2, index, setup.Name);
                }
                if (setup.SimulationResults.TestArray2 != null)
                {
                    AddSeriesToPlotModel(PlotModel2, setup.SimulationResults.TestArray1, setup.SimulationResults.TestArray2, index, setup.Name);
                }
                if (setup.SimulationResults.TestArray2 != null)
                {
                    AddSeriesToPlotModel(PlotModel3, setup.SimulationResults.TestArray1, setup.SimulationResults.TestArray2, index, setup.Name);
                }
                if (setup.SimulationResults.TestArray2 != null)
                {
                    AddSeriesToPlotModel(PlotModel4, setup.SimulationResults.TestArray1, setup.SimulationResults.TestArray2, index, setup.Name);
                }

                index++;
            }
            
            
            PlotModel1.RefreshChart();
            PlotModel2.RefreshChart();
            PlotModel3.RefreshChart();
            PlotModel4.RefreshChart();
            RefreshUserDefinedGraphs();
        }

       

        private void RefreshUserDefinedGraphs()
        {
            RefreshUserDefinedAxes();
            ClearUserDefinedGraphs();
            int index = 0;
            foreach (var rawSetup in _setups)
            {
                if (!(rawSetup is HHDevSetupFlatModel setup))
                {
                    continue;
                }

                var X1 = setup.GetSimulationResultByName(SelectedXaxis1);
                var Y1 = setup.GetSimulationResultByName(SelectedYaxis1);
                var X2 = setup.GetSimulationResultByName(SelectedXaxis2);
                var Y2 = setup.GetSimulationResultByName(SelectedYaxis2);

                if (X1 != null && Y1 != null)
                {
                    AddSeriesToPlotModel(PlotModel5, X1, Y1, index, setup.Name);
                }
                if (X2 != null && Y2 != null)
                {
                    AddSeriesToPlotModel(PlotModel6, X2, Y2, index, setup.Name);
                }
                index++;
            }
            PlotModel5.RefreshChart();
            PlotModel6.RefreshChart();
        }

        private void RefreshUserDefinedAxes()
        {
            PlotModel5.Title = null;
            PlotModel5.XAxisTitle = GetAxisLabelForProperty(_selectedXaxis1);
            PlotModel5.YAxisTitle = GetAxisLabelForProperty(_selectedYaxis1);

            PlotModel6.Title = null;
            PlotModel6.XAxisTitle = GetAxisLabelForProperty(_selectedXaxis2);
            PlotModel6.YAxisTitle = GetAxisLabelForProperty(_selectedYaxis2);
        }

        
        private string GetAxisLabelForProperty(string propertyName)
        {
            return _allProperties.FirstOrDefault(x => x.Name == propertyName)?.GroupName;
        }

        private void AddSeriesToPlotModel(HHOxyPlotModel plotModel, double[] speed, double[] yData, int index, string seriesName)
        {
            var series = new HHOxyLineSeries()
            {
                Color = GetColorByIndex(index),
                Title = seriesName,
                MarkerType = MarkerType.None,
                LineStyle = LineStyle.Solid
            };

            plotModel.AddSeries(series);

            var maxIndex = Math.Min(speed.Length, yData.Length);

            for (int i = 0; i < maxIndex; i++)
            {
                if (speed[i] == 0)
                {
                    continue;
                }
                series.Points.Add(new DataPoint(speed[i], yData[i]));
            }
        }
        private LineAnnotation CreateAnnotation(double xCoord, string text, HHOxyPlotModel plotModel)
        {
            
            var lineAnnotation = new LineAnnotation()
            {
                Type = LineAnnotationType.Vertical,
                X = xCoord,
                Color = GetColorByIndex(0),
                Text = null,
                LineStyle = LineStyle.Solid,
                StrokeThickness = 2,
                TextOrientation = AnnotationTextOrientation.Horizontal,
                TextVerticalAlignment = OxyPlot.VerticalAlignment.Top,
                TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Left,
                FontSize = 14
            };

            return lineAnnotation;
        }
        private void ClearAllGraphs()
        {
            ClearPlotModelSeries(PlotModel1);
            ClearPlotModelSeries(PlotModel2);
            ClearPlotModelSeries(PlotModel3);
            ClearPlotModelSeries(PlotModel4);
            PlotModel1.ClearAnnotations();
            PlotModel2.ClearAnnotations();
            PlotModel3.ClearAnnotations();
            PlotModel4.ClearAnnotations();
        }

        private void ClearUserDefinedGraphs()
        {
            ClearPlotModelSeries(PlotModel5);
            ClearPlotModelSeries(PlotModel6);
        }

        private void ClearPlotModelSeries(HHOxyPlotModel plotModel)
        {
            plotModel.ClearSeries();
        }

        private HHOxyPlotModel InitializePlotModel(string displayName, string yCoordName, bool legendVisible, bool trackPoint)
        {
            var plotModel = new HHOxyPlotModel()
            {
                //ChartTitle = displayName,
                //XAxisTitle = "",
                YAxisTitle = yCoordName,
                IsLegendVisible = legendVisible,
                XAxisTitle = null,
                MinorGridVisible=true                
            };
            if (trackPoint)
            {
#pragma warning disable 00618
                plotModel.TrackerChanged += PlotModel_TrackerChanged;
#pragma warning restore 00618
            }
            return plotModel;
        }

        private void PlotModel_TrackerChanged(object sender, TrackerEventArgs e)
        {
            if (e.HitResult == null)
            {
                return;
            }
            SelectedSpeed = e.HitResult.DataPoint.X;
        }

        private OxyColor GetColorByIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return OxyColors.Blue;
                case 1:
                    return OxyColors.Red;
                case 2:
                    return OxyColors.Green;
                case 3:
                    return OxyColors.Yellow;
                case 4:
                    return OxyColors.Orange;
                case 5:
                    return OxyColors.Magenta;
                case 6:
                    return OxyColors.Cyan;
                default:
                    return OxyColors.Black;
            }
        }

        #endregion

        #region SaveAndLoad
        public override void SaveToXML(XmlElement parentXMLElement)
        {
            base.SaveToXML(parentXMLElement);
            var simNode = parentXMLElement.OwnerDocument.CreateElement("Simulation");
            parentXMLElement.AppendChild(simNode);
            XMLHelperFunctions.WriteToXML("X1",SelectedXaxis1, simNode);
            XMLHelperFunctions.WriteToXML("Y1", SelectedYaxis1, simNode);
            XMLHelperFunctions.WriteToXML("X2", SelectedXaxis2, simNode);
            XMLHelperFunctions.WriteToXML("Y2", SelectedYaxis2, simNode);
        }
        public override bool LoadFromXML(XmlElement parentXMLElement)
        {
            var baseLoad = base.LoadFromXML(parentXMLElement);

            var simNode = parentXMLElement.SelectSingleNode("Simulation");

            if (simNode != null)
            {
                var x1Node = simNode.SelectSingleNode("X1");
                if (x1Node != null)
                {
                    _selectedXaxis1 = x1Node.InnerText;
                }
                var y1Node = simNode.SelectSingleNode("Y1");
                if (y1Node != null)
                {
                    _selectedYaxis1 = y1Node.InnerText;
                }
                var x2Node = simNode.SelectSingleNode("X2");
                if (x2Node != null)
                {
                    _selectedXaxis2 = x2Node.InnerText;
                }
                var y2Node = simNode.SelectSingleNode("Y2");
                if (y2Node != null)
                {
                    _selectedYaxis2 = y2Node.InnerText;
                }

                RefreshUserDefinedGraphs();
            }

            return baseLoad;
        }
        #endregion
    }
}
