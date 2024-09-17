using HHDev.Core.NETStandard.Definitions;
using HHDev.DataManagement.Client.Core.Definitions.HHCustomGridDisplay;
using HHDev.DataManagement.Client.Core.Definitions.HHCustomGridDisplay.GridLayout;
using HHDev.DataManagement.Client.Core.Definitions.HHCustomGridDisplay.Properties;
using HHDev.DataManagement.Client.Wpf.Controls;
using HHDev.DataManagement.Client.Wpf.Helpers;
using HHDev.DataManagement.Client.Wpf.PluginFramework.CustomizationConfigs;
using HHDev.DataManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HHDev.HHDM.DesktopPlugin.RunSheet
{
    public static class RunSheetDefinitionBuilder
    {
        private static StyleOptions GetTabItemStyleOptions()
        {
            var options = new StyleOptions
            {
                FontSize =
                {
                    StyleOptionsItemMode = eStyleOptionsItemMode.Constant,
                    ConstantTyped = 12
                },
                FontWeight =
                {
                    StyleOptionsItemMode = eStyleOptionsItemMode.Constant,
                    ConstantTyped = FontWeights.SemiBold.ToString()
                },
                ForegroundColour =
                {
                    StyleOptionsItemMode = eStyleOptionsItemMode.Constant,
                    ConstantTyped = ThemeHelpers.GetThemeColor("Foreground").ToString()
                }
            };

            return options;
        }

        public static Func<RunSheetLayoutDefinitionParameters, CustomGridDefinition> GetLayoutDefinition() => parameters =>
        {
            var runSheetTopBar = new PrimitiveControlReferenceDefinition("CFF1D8A9-2A75-4639-9A74-0456DBF0B773", "SelectedRunSheetModel")
            {
                HorizontalAlignment = eHorizontalAlignment.Stretch,
            };

            var lapTable = new PrimitiveControlReferenceDefinition("E929E560-36E6-449A-BA5A-89643F6C7DF9", "SelectedRunSheetModel")
            {
                HorizontalAlignment = eHorizontalAlignment.Stretch,
                Margin = new HHThickness(0, 5, 0, 0),
                VerticalAlignment = eVerticalAlignment.Stretch,
                Height = double.NaN,
            };


            var engineerNotesTab = new TabItemLayoutItem()
            {
                Title = "Engineer Notes",
                Content = new PrimitiveControlReferenceDefinition("658BECE5-B7CA-41E4-BA98-932C2F4DCD6E", "SelectedRunSheetModel"),
                StyleOptions = GetTabItemStyleOptions(),
            };

            var issueListTab = new TabItemLayoutItem()
            {
                Title = "Issue List",
                // IssueList
                Content = new PrimitiveControlReferenceDefinition("99F7D297-24B2-42CD-8318-730E437891A0"),
                StyleOptions = GetTabItemStyleOptions(),
            };
            var driverCommentTabControl = new TabControlLayoutItem()
            {
                HorizontalAlignment = eHorizontalAlignment.Stretch,
                Margin = new HHThickness(0, 0, 2, 0)
            };
            var engineerCommentTabControl = new TabControlLayoutItem()
            {
                HorizontalAlignment = eHorizontalAlignment.Stretch,
                Margin = new HHThickness(2, 0, 2, 0)
            };
            driverCommentTabControl.TabItems.Add(new TabItemLayoutItem()
            {
                Title = "Driver Comments",
                Content = new TextAreaControlDefinition()
                {
                    IsTitleVisible = false,
                    TextBindingPath = "SelectedRunSheetModel.RunSheetFlatModel.Strings.DriverComments",
                    VerticalAlignment = eVerticalAlignment.Stretch,
                    Margin = new HHThickness(0, 0, 0, 0)
                },
                StyleOptions = GetTabItemStyleOptions(),
            });
            engineerCommentTabControl.TabItems.Add(new TabItemLayoutItem()
            {
                Title = "Engineer Comments",
                Content = new TextAreaControlDefinition()
                {
                    IsTitleVisible = false,
                    TextBindingPath = "SelectedRunSheetModel.RunSheetFlatModel.Strings.EngineerComments",
                    VerticalAlignment = eVerticalAlignment.Stretch,
                    Margin = new HHThickness(0, 0, 0, 0)
                },
                StyleOptions = GetTabItemStyleOptions(),
            });

            var engineerNotesIssueListTabControl = new TabControlLayoutItem()
            {
                HorizontalAlignment = eHorizontalAlignment.Stretch,
                Margin = new HHThickness(2, 0, 0, 0)
            };
            engineerNotesIssueListTabControl.TabItems.Add(engineerNotesTab);
            engineerNotesIssueListTabControl.TabItems.Add(issueListTab);

            var commentsRows = new GridLayoutItem(
                          new List<HHRowDefinition>()
                          {
                              new HHRowDefinition(1, eGridUnitType.Star),
                          },
                          new List<HHColumnDefinition>()
                          {
                              new HHColumnDefinition(1, eGridUnitType.Star),
                              new HHColumnDefinition(1, eGridUnitType.Star),
                              new HHColumnDefinition(1, eGridUnitType.Star)
                          },
                          new List<GridItemDefinition>() { 

                // First row of comments
                new GridItemDefinition(0, 0,
                     driverCommentTabControl),
                new GridItemDefinition(0, 1,
                     engineerCommentTabControl),
                 new GridItemDefinition(0, 2,
                     engineerNotesIssueListTabControl),
                     })
            {
                HorizontalAlignment = eHorizontalAlignment.Stretch,
                Margin = new HHThickness(0, 2, 0, 0)
            };

            var mainTabContent = new ScrollViewerLayoutItem()
            {
                HorizontalScrollBarVisibility = eScrollBarVisibility.Disabled,
                VerticalScrollBarVisibility = eScrollBarVisibility.Auto,
                Content = new GridLayoutItem(new List<HHRowDefinition>()
                                                            {
                                                                new HHRowDefinition(eGridUnitType.Auto),
                                                                new HHRowDefinition(eGridUnitType.Auto),
                                                                new HHRowDefinition(eGridUnitType.Auto),
                                                                new HHRowDefinition(eGridUnitType.Auto),
                                                            },
                                                            new List<HHColumnDefinition>()
                                                            {
                                                                new HHColumnDefinition(eGridUnitType.Star)
                                                            },
                                                            new List<GridItemDefinition>()
                                                            {
                                                                // FuelManagement
                                                                new GridItemDefinition(0, 0, new PrimitiveControlReferenceDefinition("1A7FE1DB-6ED8-4125-AF0A-2A48927B1B40")
                                                                {
                                                                    HorizontalAlignment = eHorizontalAlignment.Stretch
                                                                }),
                                                                // TyrePressures
                                                                new GridItemDefinition(1, 0, new PrimitiveControlReferenceDefinition("9319C439-5E55-4EA2-B69B-141B9AF567FE")
                                                                {
                                                                    HorizontalAlignment = eHorizontalAlignment.Stretch,
                                                                    Margin = new HHThickness(0,10,0,0)
                                                                }),
                                                                // Temp
                                                                new GridItemDefinition(2, 0, new PrimitiveControlReferenceDefinition("C9DF9D5D-0155-4383-A1BE-02494E5CA405")
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

            var runsheetRightPanelTabControl = new TabControlLayoutItem();

            var mainTab = new TabItemLayoutItem()
            {
                Title = "Main",
                Content = mainTabContent,
                StyleOptions = GetTabItemStyleOptions(),
            };
            var cidTab = new TabItemLayoutItem()
            {
                Title = "CID",
                Content = new PrimitiveControlReferenceDefinition("986C5E46-E58C-49B1-80D4-F1AE71C94CF9") { VerticalAlignment = eVerticalAlignment.Top, HorizontalAlignment = eHorizontalAlignment.Left },
                StyleOptions = GetTabItemStyleOptions(),
            };
            var trackMapTab = new TabItemLayoutItem()
            {
                Title = "Track Map",
                Content = new PrimitiveControlReferenceDefinition("C485A5B3-5A79-45B6-B0D9-2130EF885C42", "SelectedRunSheetModel"),
                StyleOptions = GetTabItemStyleOptions(),
            };
            var attachedFileTab = new TabItemLayoutItem()
            {
                Title = "Attached Files",
                Content = new PrimitiveControlReferenceDefinition("081CBB52-2D26-4533-9DA8-2627BBCC7542", "SelectedRunSheetModel") { MaxHeight = 500, VerticalAlignment = eVerticalAlignment.Top },
                StyleOptions = GetTabItemStyleOptions(),
            };

            var ambientMeasurementTab = new TabItemLayoutItem()
            {
                Title = "Ambient Measurements",
                Content = new PrimitiveControlReferenceDefinition("D1DC10FF-2B59-48BE-AB8A-B38A3D9F4FCC", "SelectedRunSheetModel") { MaxHeight = 500, VerticalAlignment = eVerticalAlignment.Top },
                StyleOptions = GetTabItemStyleOptions(),
            };

            runsheetRightPanelTabControl.TabItems.Add(mainTab);
            runsheetRightPanelTabControl.TabItems.Add(cidTab);
            runsheetRightPanelTabControl.TabItems.Add(trackMapTab);
            runsheetRightPanelTabControl.TabItems.Add(attachedFileTab);
            runsheetRightPanelTabControl.TabItems.Add(ambientMeasurementTab);
            runsheetRightPanelTabControl.Width = 500;

            var leftPart = new GridLayoutItem(new List<HHRowDefinition>()
                                                    {
                                                        new HHRowDefinition(eGridUnitType.Auto),
                                                        new HHRowDefinition(eGridUnitType.Star),
                                                        new HHRowDefinition(eGridUnitType.Auto),
                                                        new HHRowDefinition(eGridUnitType.Star)
                                                    },
                                                    new List<HHColumnDefinition>()
                                                    {
                                                        new HHColumnDefinition(eGridUnitType.Star)
                                                    },
                                                    new List<GridItemDefinition>()
                                                    {
                                                        // RunSheet top bar
                                                        new GridItemDefinition(0, 0, new ScrollViewerLayoutItem()
                                                        {
                                                            Content = runSheetTopBar,
                                                            HorizontalScrollBarVisibility = eScrollBarVisibility.Auto,
                                                            VerticalScrollBarVisibility = eScrollBarVisibility.Disabled,
                                                        }),
                                                        new GridItemDefinition(1, 0, lapTable),
                                                        // Grid Splitter 
                                                        new GridItemDefinition(2,
                                                                                     0,
                                                                                     new HHGridSplitterDefinition()
                                                                                     {
                                                                                         Orientation = eGridSplitterOrientation.Horizontal
                                                                                     }),
                                                        // Comments
                                                        new GridItemDefinition(3, 0, commentsRows)
                                                    })
            {
                VerticalAlignment = eVerticalAlignment.Stretch,
                MaxHeight = 5000,
            };

            var rightPart = new StackLayoutItem(eOrientation.Vertical, new List<object>()
            {
                runsheetRightPanelTabControl,
                // SetupOverview
                new PrimitiveControlReferenceDefinition("D4D4547E-A238-47AE-B130-4BE6CB3323E0")
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
                var setupTab = new TabItemLayoutItem()
                {
                    Title = "Setup",
                    Content = new PrimitiveControlReferenceDefinition("D4D4547E-A238-47AE-B130-4BE6CB3323E0") { VerticalAlignment = eVerticalAlignment.Top, HorizontalAlignment = eHorizontalAlignment.Left },
                    StyleOptions = GetTabItemStyleOptions(),
                };
                runsheetRightPanelTabControl.TabItems.Add(setupTab);
                rightPart = new StackLayoutItem(eOrientation.Vertical, new List<object>()
                {
                    runsheetRightPanelTabControl
                })
                {
                    Orientation = eOrientation.Horizontal
                };
            }
            return new CustomGridDefinition(
                 new GridLayoutItem(new List<HHRowDefinition>()
                {
                     new HHRowDefinition(eGridUnitType.Star)
                },
                new List<HHColumnDefinition>()
                    {
                        new HHColumnDefinition(2, eGridUnitType.Star),
                        new HHColumnDefinition(eGridUnitType.Auto),
                        new HHColumnDefinition(eGridUnitType.Star)
                    },
                new List<GridItemDefinition>()
                    {
                     new GridItemDefinition(0, 0,
                     leftPart ),
                    new GridItemDefinition(0,1,new HHGridSplitterDefinition()
                        {
                            Orientation = eGridSplitterOrientation.Vertical
                        }),
                    new GridItemDefinition(0, 2, new ScrollViewerLayoutItem()
                    {
                        VerticalScrollBarVisibility = eScrollBarVisibility.Disabled,
                        HorizontalScrollBarVisibility = eScrollBarVisibility.Auto,
                        Content = rightPart
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
