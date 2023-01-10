using HHDev.DataManagement.Client.Core.Definitions.HHCustomGridDisplay;
using HHDev.DataManagement.Client.Core;
using HHDev.DataManagement.Client.Wpf.PluginFramework.CustomizationConfigs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHDev.DataManagement.Client.Core.Definitions.HHCustomGridDisplay.Definitions;
using HHDev.DataManagement.Client.Core.Definitions.HHCustomGridDisplay.GridLayout;
using HHDev.DataManagement.Client.Core.Definitions.HHCustomGridDisplay.Properties;

namespace HHDev.HHDM.DesktopPlugin.RunSheet
{
    public static class RunSheetDefinitionBuilder
    {
        public static CustomGridDefinition GetLayoutDefinition()
        {
            return new CustomGridDefinition(
                 new StackLayoutDefinition(new List<object>()
                     {
                        new StackLayoutDefinition(new List<object>()
                        {
                            // RunSheet tob bar
                            new PrimitiveControlReference("CFF1D8A9-2A75-4639-9A74-0456DBF0B773", "SelectedRunSheetModel") {HorizontalAlignment = eHorizontalAlignment.Left},
                            // Lap Table
                            new PrimitiveControlReference("E929E560-36E6-449A-BA5A-89643F6C7DF9", "SelectedRunSheetModel"){HorizontalAlignment = eHorizontalAlignment.Left, Margin=new HHThickness(0,5,0,0) },
                            
                            // First row of comments
                            new GridLayoutDefinition(new List<RowDefinition>()
                            {
                                new RowDefinition(216),
                            }, new List<ColumnDefinition>()
                            {
                                new ColumnDefinition(GridLength.GridUnitType.Auto),
                                new ColumnDefinition(GridLength.GridUnitType.Auto),
                            }, new List<GridLayoutItemDefinition>()
                            {
                                new GridLayoutItemDefinition(0, 0, new TextAreaDefinition()
                                {
                                    Title = "Driver Comments",
                                    TextBindingPath = "RunSheetFlatModel.Strings.DriverComments",
                                    VerticalAlignment = eVerticalAlignment.Stretch,
                                    Width=486
                                }),
                                new GridLayoutItemDefinition(0, 1, new TextAreaDefinition()
                                {
                                    Title = "Engineer Comments",
                                    TextBindingPath = "RunSheetFlatModel.Strings.EngineerComments",
                                    VerticalAlignment = eVerticalAlignment.Stretch,
                                    Width=486,
                                   Margin=new HHThickness(6,0,0,0),
                                }),
                            }) { HorizontalAlignment = eHorizontalAlignment.Left, Margin=new HHThickness(0,5,0,0)},
                            
                            // Second row of comments
                            new GridLayoutDefinition(new List<RowDefinition>()
                            {
                                new RowDefinition(205),
                            }, new List<ColumnDefinition>()
                            {
                                new ColumnDefinition(GridLength.GridUnitType.Auto),
                                new ColumnDefinition(GridLength.GridUnitType.Auto),
                                new ColumnDefinition(371),
                            }, new List<GridLayoutItemDefinition>()
                            {
                                new GridLayoutItemDefinition(0, 0, new TextAreaDefinition()
                                {
                                     Title = "Changes Next Run",
                                    TextBindingPath = "RunSheetFlatModel.Strings.ChangesForNextRun",
                                    VerticalAlignment = eVerticalAlignment.Stretch,
                                    Width=342
                                }),
                                new GridLayoutItemDefinition(0, 1, new PrimitiveControlReference("D1DC10FF-2B59-48BE-AB8A-B38A3D9F4FCC", "SelectedRunSheetModel"){Margin=new HHThickness(6,0,0,0) }),
                                new GridLayoutItemDefinition(0, 2, new PrimitiveControlReference("081CBB52-2D26-4533-9DA8-2627BBCC7542", "SelectedRunSheetModel"){Margin=new HHThickness(6,0,0,0) }),
                            }) { HorizontalAlignment = eHorizontalAlignment.Left, Margin=new HHThickness(0,5,0,0)}
                        }, eOrientation.Vertical) {VerticalAlignment = eVerticalAlignment.Top},

                        // RunSheetRightPanel
                            new PrimitiveControlReference("D0C6E00A-D8C9-4A4B-9BB2-7AE7E77DE3DB", "SelectedRunSheetModel") {VerticalAlignment = eVerticalAlignment.Top},
                        // SetupOverview
                        new PrimitiveControlReference("D4D4547E-A238-47AE-B130-4BE6CB3323E0") {VerticalAlignment = eVerticalAlignment.Top}
                    }, eOrientation.Horizontal)
                 { VerticalAlignment = eVerticalAlignment.Top, HorizontalAlignment = eHorizontalAlignment.Left }, "RunSheetFlatModel");
        }
        public static List<PrimitiveDefinition> GetPrimitiveDefinitions()
        {
            return new List<PrimitiveDefinition>()
            {
                new PrimitiveDefinition("D0C6E00A-D8C9-4A4B-9BB2-7AE7E77DE3DB", typeof(RunSheetRightPanel))
            };
        }
    }
}
