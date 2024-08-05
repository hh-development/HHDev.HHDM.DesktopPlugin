using HHDev.DataManagement.Client.Wpf.PluginFramework.Interfaces;
using System;
using System.Collections.Generic;
using HHDev.Core.NETFramework.Models;
using HHDev.Core.WPF.Layout;
using HHDev.DataManagement.Client.Wpf.Layout;
using HHDev.DataManagement.Client.Wpf.Views;
using HHDev.DataManagement.Client.Wpf.PluginFramework.CustomizationConfigs;
using HHDev.DataManagement.Client.Core.Models.FlatModels;
using HHDev.HHDM.DesktopPlugin.Setup;
using HHDev.DataManagement.Client.Core.Definitions.HHCustomGridDisplay.Styles;
using HHDev.DataManagement.Client.Core.Definitions.HHCustomGridDisplay;
using HHDev.HHDM.DesktopPlugin.RunSheet;
using HHDev.DataManagement.Client.Wpf.Components.TabControl;
using HHDev.DataManagement.Client.Wpf.Helpers;
using HHDev.HHDM.DesktopPlugin.EventCarData;
using HHDev.HHDM.DesktopPlugin.Setup.SetupGraph;
using HHDev.DataManagement.Client.Core.PluginFramework.Interfaces;
using HHDev.HHDM.DesktopPlugin.FilterableView;

namespace HHDev.HHDM.DesktopPlugin
{
    public class PluginEntry :
        IHhDataManagementPlugin,
        ICustomFlatModelProviderPlugin,
        IRunSheetCustomizationPlugin,
        IEventCarDataCustomizationPlugin,
        ICustomizeUserInterfacePlugin
    {
        #region IHhDataManagementPlugin
        public string Name => "HHDev Plugin";//plugin Name
        public string AutoUpdateKey => "HHDM-HHDev";// key used to get auto update in HH DM
        public Guid PluginId => Guid.Parse("{06B9280A-2343-4A7A-9F2E-DB3C82467DC1}"); // Needs to be a unique Guid
        public Guid[] AccountIds => new Guid[] { Guid.Parse("F2A94AB9-3E13-4FC5-9DBC-E3A88D78E17A") }; // Account Id acessible in HH DM from File => Option
        #endregion

        #region ICustomFlatModelProviderPlugin
        public List<FlatModelInitializationDefinition> FlatModelDefinitions => new List<FlatModelInitializationDefinition>()
        {
            new FlatModelInitializationDefinition(eFlatModelTypes.Setup, "60d5a9b2-122a-42c2-a093-f9add1ec2bb4", typeof(HHDevSetupFlatModel))// Guid of the setup definition(can be found in Admin => Definition)
        };

        #endregion

        #region IRunSheetCustomizationPlugin
        public List<KeyValuePair<Guid, RunSheetCustomizationConfig>> RunSheetCustomizationConfigs { get; } = new List<KeyValuePair<Guid, RunSheetCustomizationConfig>>()
            {
            new KeyValuePair<Guid, RunSheetCustomizationConfig>(Guid.Parse("07111622-0281-4085-8118-d82082f84007"),
                                                        new RunSheetCustomizationConfig(new List<CompositeControlDefinition>(),
                                                                                        RunSheetDefinitionBuilder.GetLayoutDefinition(),
                                                                                        RunSheetDefinitionBuilder.GetPrimitiveDefinitions(),
                                                                                        new List<StyleDefinition>(),
                                                                                        null,null,null,null)),
            

            };

        #endregion

        #region IEventCarDataCustomizationPlugin


        public List<KeyValuePair<Guid, EventCarDataCustomizationConfig>> EventCarDataCustomizationConfigs { get; } = new List<KeyValuePair<Guid, EventCarDataCustomizationConfig>>()
            {
            

               new KeyValuePair<Guid, EventCarDataCustomizationConfig>(
                                    Guid.Parse("4a1632e6-c322-445b-bc08-7c6512403345"),
                                    new EventCarDataCustomizationConfig(GetEventTabsWithDefault(new List<HHCustomizableTab>()
                                    {
                                        new HHCustomizableTab("CustomizeTab", typeof(CustomizeTabView), "setup")
                                    }))),
            };

        private static IEnumerable<HHCustomizableTab> GetEventTabsWithDefault(IEnumerable<HHCustomizableTab> customTabs)
        {
            var retList = new List<HHCustomizableTab>();
            retList.AddRange(CustomTabsHelpers.GetEventDefaultTabs());
            retList.AddRange(customTabs);
            return retList;
        }
        #endregion

        #region ICustomizeUserInterfacePlugin
#pragma warning disable 0067
        public event EventHandler<AddNewDocumentEventArgs> AddNewDocument;
#pragma warning restore 0067
        public List<ViewModelMapping> ViewModelAndViewMappings { get; } = new List<ViewModelMapping>()
        {
            new ViewModelMapping(typeof(SimulationResultsViewModel), typeof(SimulationResultsView)),
            new ViewModelMapping(typeof(PluginRunSheetViewModel), typeof(RunSheetsView)),
            new ViewModelMapping(typeof(ExampleFilterableViewModel), typeof(ExampleFilterableView)),
        };

        public List<Type> TypesForRegistration { get; } = new List<Type>()
        {
            typeof(SimulationResultsViewModel),
            typeof(SimulationResultsView),
            typeof(PluginRunSheetViewModel),
            typeof(ExampleFilterableViewModel),
            typeof(ExampleFilterableView),
        };
        public List<HHRibbonTab> PluginRibbonTabs { get; } = new List<HHRibbonTab>();
        public List<RibbonPageModel> RibbonPages { get; } = new List<RibbonPageModel>();

        private void BuildMainRibbonTab() // add a tab in the ribbon bar and then some button in the new tab
        {
            var tab = new HHDMRibbonPageModel("Plugin Tabs");
            RibbonPages.Add(tab);


            var group = new RibbonPageGroupModel("Setups");
            tab.Groups.Add(group);
            group.Children.Add(new OpenViewRibbonButtonModel("Setup Graphs", "info", typeof(SimulationResultsView), null));
            group.Children.Add(new OpenViewRibbonButtonModel("Setup History", "setup-comparison", typeof(ExampleFilterableView), null));
        }

        #endregion

        #region Constructor

        public PluginEntry()
        {
            BuildMainRibbonTab();
        }

        #endregion       
    }
}
