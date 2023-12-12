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
using HHDev.DataManagement.Client.Core.Caches;
using HHDev.DataManagement.Client.Core.Definitions.HHCustomGridDisplay.CellDefinitions;
using HHDev.DataManagement.Client.Wpf.Controls;

namespace HHDev.HHDM.DesktopPlugin.RunSheet
{
    public static class RunSheetDefinitionBuilder
    {
        public static Func<RunSheetLayoutDefinitionParameters, CustomGridDefinition> GetLayoutDefinition() => parameters =>
        {
            var runSheetTopBar = new PrimitiveControlReference("CFF1D8A9-2A75-4639-9A74-0456DBF0B773", "SelectedRunSheetModel")
            {
                HorizontalAlignment = eHorizontalAlignment.Stretch,
            };

            var lapTable = new PrimitiveControlReference("E929E560-36E6-449A-BA5A-89643F6C7DF9", "SelectedRunSheetModel")
            {
                HorizontalAlignment = eHorizontalAlignment.Stretch,
                Margin = new HHThickness(0, 5, 0, 0),
                VerticalAlignment = eVerticalAlignment.Stretch,
                Height = double.NaN,
            };


            var engineerNotesTab = new TabItemDefinition()
            {
                Title = "Engineer Notes",
                Content = new PrimitiveControlReference("658BECE5-B7CA-41E4-BA98-932C2F4DCD6E", "SelectedRunSheetModel"),
                HeaderFontSize = 12,
            };

            var issueListTab = new TabItemDefinition()
            {
                Title = "Issue List",
                // IssueList
                Content = new PrimitiveControlReference("99F7D297-24B2-42CD-8318-730E437891A0"),
                HeaderFontSize = 12,
            };
            var driverCommentTabControl = new TabControlDefinition()
            {
                HorizontalAlignment = eHorizontalAlignment.Stretch,
                Margin = new HHThickness(0, 0, 2, 0)
            };
            var engineerCommentTabControl = new TabControlDefinition()
            {
                HorizontalAlignment = eHorizontalAlignment.Stretch,
                Margin = new HHThickness(2, 0, 2, 0)
            };
            driverCommentTabControl.TabItems.Add(new TabItemDefinition()
            {
                Title = "Driver Comments",
                Content = new TextAreaDefinition()
                {
                    IsTitleVisible = false,
                    TextBindingPath = "SelectedRunSheetModel.RunSheetFlatModel.Strings.DriverComments",
                    VerticalAlignment = eVerticalAlignment.Stretch,
                    Margin = new HHThickness(0, 0, 0, 0)
                },
                HeaderFontSize = 12,
            });
            engineerCommentTabControl.TabItems.Add(new TabItemDefinition()
            {
                Title = "Engineer Comments",
                Content = new TextAreaDefinition()
                {
                    IsTitleVisible = false,
                    TextBindingPath = "SelectedRunSheetModel.RunSheetFlatModel.Strings.EngineerComments",
                    VerticalAlignment = eVerticalAlignment.Stretch,
                    Margin = new HHThickness(0, 0, 0, 0)
                },
                HeaderFontSize = 12,
            });

            var engineerNotesIssueListTabControl = new TabControlDefinition()
            {
                HorizontalAlignment = eHorizontalAlignment.Stretch,
                Margin = new HHThickness(2, 0, 0, 0)
            };
            engineerNotesIssueListTabControl.TabItems.Add(engineerNotesTab);
            engineerNotesIssueListTabControl.TabItems.Add(issueListTab);

            var commentsRows = new GridLayoutDefinition(
                          new List<RowDefinition>()
                          {
                              new RowDefinition(1, GridLength.GridUnitType.Star),
                          },
                          new List<ColumnDefinition>()
                          {
                              new ColumnDefinition(1, GridLength.GridUnitType.Star),
                              new ColumnDefinition(1, GridLength.GridUnitType.Star),
                              new ColumnDefinition(1, GridLength.GridUnitType.Star)
                          },
                          new List<GridLayoutItemDefinition>() { 

                // First row of comments
                new GridLayoutItemDefinition(0, 0,
                     driverCommentTabControl),
                new GridLayoutItemDefinition(0, 1,
                     engineerCommentTabControl),
                 new GridLayoutItemDefinition(0, 2,
                     engineerNotesIssueListTabControl),
                     })
            {
                HorizontalAlignment = eHorizontalAlignment.Stretch,
                Margin = new HHThickness(0, 2, 0, 0)
            };

            var mainTabContent = new ScrollViewerLayoutDefinition()
            {
                HorizontalScrollBarVisibility = eScrollBarVisibility.Disabled,
                VerticalScrollBarVisibility = eScrollBarVisibility.Auto,
                LayoutDefinition = new GridLayoutDefinition(new List<RowDefinition>()
                                                            {
                                                                new RowDefinition(GridLength.GridUnitType.Auto),
                                                                new RowDefinition(GridLength.GridUnitType.Auto),
                                                                new RowDefinition(GridLength.GridUnitType.Auto),
                                                                new RowDefinition(GridLength.GridUnitType.Auto),
                                                            },
                                                            new List<ColumnDefinition>()
                                                            {
                                                                new ColumnDefinition(GridLength.GridUnitType.Star)
                                                            },
                                                            new List<GridLayoutItemDefinition>()
                                                            {
                                                                // FuelManagement
                                                                new GridLayoutItemDefinition(0, 0, new PrimitiveControlReference("1A7FE1DB-6ED8-4125-AF0A-2A48927B1B40")
                                                                {
                                                                    HorizontalAlignment = eHorizontalAlignment.Stretch
                                                                }),
                                                                // TyrePressures
                                                                new GridLayoutItemDefinition(1, 0, new PrimitiveControlReference("9319C439-5E55-4EA2-B69B-141B9AF567FE")
                                                                {
                                                                    HorizontalAlignment = eHorizontalAlignment.Stretch,
                                                                    Margin = new HHThickness(0,10,0,0)
                                                                }),
                                                                // Temp
                                                                new GridLayoutItemDefinition(2, 0, new PrimitiveControlReference("C9DF9D5D-0155-4383-A1BE-02494E5CA405")
                                                                {
                                                                    HorizontalAlignment = eHorizontalAlignment.Stretch,
                                                                    Margin = new HHThickness(0,10,0,0)
                                                                })
                                                            })
                {
                    VerticalAlignment = eVerticalAlignment.Top,
                    Margin = new HHThickness(0, 0, 0, 0)
                }
            };

            var runsheetRightPanelTabControl = new TabControlDefinition();

            var mainTab = new TabItemDefinition()
            {
                Title = "Main",
                Content = mainTabContent,
                HeaderFontSize = 12
            };
            var cidTab = new TabItemDefinition()
            {
                Title = "CID",
                Content = new PrimitiveControlReference("986C5E46-E58C-49B1-80D4-F1AE71C94CF9") { VerticalAlignment = eVerticalAlignment.Top, HorizontalAlignment = eHorizontalAlignment.Left },
                HeaderFontSize = 12
            };
            var trackMapTab = new TabItemDefinition()
            {
                Title = "Track Map",
                Content = new PrimitiveControlReference("C485A5B3-5A79-45B6-B0D9-2130EF885C42", "SelectedRunSheetModel"),
                HeaderFontSize = 12
            };
            var attachedFileTab = new TabItemDefinition()
            {
                Title = "Attached Files",
                Content = new PrimitiveControlReference("081CBB52-2D26-4533-9DA8-2627BBCC7542", "SelectedRunSheetModel") { MaxHeight = 500, VerticalAlignment = eVerticalAlignment.Top },
                HeaderFontSize = 12
            };

            var ambientMeasurementTab = new TabItemDefinition()
            {
                Title = "Ambient Measurements",
                Content = new PrimitiveControlReference("D1DC10FF-2B59-48BE-AB8A-B38A3D9F4FCC", "SelectedRunSheetModel") { MaxHeight = 500, VerticalAlignment = eVerticalAlignment.Top },
                HeaderFontSize = 12
            };

            runsheetRightPanelTabControl.TabItems.Add(mainTab);
            runsheetRightPanelTabControl.TabItems.Add(cidTab);
            runsheetRightPanelTabControl.TabItems.Add(trackMapTab);
            runsheetRightPanelTabControl.TabItems.Add(attachedFileTab);
            runsheetRightPanelTabControl.TabItems.Add(ambientMeasurementTab);
            runsheetRightPanelTabControl.Width = 500;

            var leftPart = new GridLayoutDefinition(new List<RowDefinition>()
                                                    {
                                                        new RowDefinition(GridLength.GridUnitType.Auto),
                                                        new RowDefinition(GridLength.GridUnitType.Star),
                                                        new RowDefinition(GridLength.GridUnitType.Auto),
                                                        new RowDefinition(GridLength.GridUnitType.Star)
                                                    },
                                                    new List<ColumnDefinition>()
                                                    {
                                                        new ColumnDefinition(GridLength.GridUnitType.Star)
                                                    },
                                                    new List<GridLayoutItemDefinition>()
                                                    {
                                                        // RunSheet top bar
                                                        new GridLayoutItemDefinition(0, 0, new ScrollViewerLayoutDefinition()
                                                        {
                                                            LayoutDefinition = runSheetTopBar,
                                                            HorizontalScrollBarVisibility = eScrollBarVisibility.Auto,
                                                            VerticalScrollBarVisibility = eScrollBarVisibility.Disabled,
                                                        }),
                                                        new GridLayoutItemDefinition(1, 0, lapTable),
                                                        // Grid Splitter 
                                                        new GridLayoutItemDefinition(2,
                                                                                     0,
                                                                                     new HHGridSplitter()
                                                                                     {
                                                                                         Orientation = eGridSplitterOrientation.Horizontal
                                                                                     }),
                                                        // Comments
                                                        new GridLayoutItemDefinition(3, 0, commentsRows)
                                                    })
            {
                VerticalAlignment = eVerticalAlignment.Stretch,
                MaxHeight = 5000,
            };

            var rightPart = new StackLayoutDefinition(new List<object>()
            {
                runsheetRightPanelTabControl,
                // SetupOverview
                new PrimitiveControlReference("D4D4547E-A238-47AE-B130-4BE6CB3323E0")
                {
                    VerticalAlignment = eVerticalAlignment.Top
                }
            })
            {
                Orientation = eOrientation.Horizontal
            };
            var isSetupInTab = parameters.IsSetupInTab;

            if (isSetupInTab)
            {
                var setupTab = new TabItemDefinition()
                {
                    Title = "Setup",
                    Content = new PrimitiveControlReference("D4D4547E-A238-47AE-B130-4BE6CB3323E0") { VerticalAlignment = eVerticalAlignment.Top, HorizontalAlignment = eHorizontalAlignment.Left },
                    HeaderFontSize = 12,
                };
                runsheetRightPanelTabControl.TabItems.Add(setupTab);
                rightPart = new StackLayoutDefinition(new List<object>()
                {
                    runsheetRightPanelTabControl
                })
                {
                    Orientation = eOrientation.Horizontal
                };
            }
            return new CustomGridDefinition(
                 new GridLayoutDefinition(new List<RowDefinition>()
                {
                     new RowDefinition(GridLength.GridUnitType.Star)
                },
                new List<ColumnDefinition>()
                    {
                        new ColumnDefinition(2, GridLength.GridUnitType.Star),
                        new ColumnDefinition(GridLength.GridUnitType.Auto),
                        new ColumnDefinition(GridLength.GridUnitType.Star)
                    },
                new List<GridLayoutItemDefinition>()
                    {
                     new GridLayoutItemDefinition(0, 0,
                     leftPart ),
                    new GridLayoutItemDefinition(0,1,new HHGridSplitter()
                        {
                            Orientation = eGridSplitterOrientation.Vertical
                        }),
                    new GridLayoutItemDefinition(0, 2, new ScrollViewerLayoutDefinition()
                    {
                        VerticalScrollBarVisibility = eScrollBarVisibility.Disabled,
                        HorizontalScrollBarVisibility = eScrollBarVisibility.Auto,
                        LayoutDefinition = rightPart
                    })
                })
                ,
                "SelectedRunSheetModel.RunSheetFlatModel");
        };
        public static List<PrimitiveDefinition> GetPrimitiveDefinitions()
        {
            return new List<PrimitiveDefinition>()
            {
                new PrimitiveDefinition("D0C6E00A-D8C9-4A4B-9BB2-7AE7E77DE3DB", typeof(RunSheetRightPanel))
            };
        }
    }
}
