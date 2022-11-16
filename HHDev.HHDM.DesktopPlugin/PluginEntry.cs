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

namespace AndrettiFE.HHDM.DesktopPlugin
{
    public class PluginEntry :
        IHhDataManagementPlugin,
        ICustomFlatModelProviderPlugin
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
            new FlatModelInitializationDefinition(eFlatModelTypes.Setup, "60d5a9b2-122a-42c2-a093-f9add1ec2bb4", typeof(HHDevRunSheetFlatModel))// Guid of the setup definition(can be found in Admin => Definition)
        };

        #endregion

    }
}
