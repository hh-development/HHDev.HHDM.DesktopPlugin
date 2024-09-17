using HHDev.Core.NETStandard.Definitions;
using HHDev.Core.NETStandard.Extensions;
using HHDev.DataManagement.Client.Core;
using HHDev.DataManagement.Client.Core.Caches;
using HHDev.DataManagement.Client.Core.Components.CIDViewModels;
using HHDev.DataManagement.Client.Core.Components.Filtering;
using HHDev.DataManagement.Client.Core.Definitions.HHColumnItemDisplay;
using HHDev.DataManagement.Client.Core.Definitions.HHColumnItemDisplay.Interfaces;
using HHDev.DataManagement.Client.Core.Models.FlatModels;
using HHDev.DataManagement.Client.Core.ViewModels;
using HHDev.DataManagement.Client.Wpf.Views;
using HHDev.DataManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HHDev.HHDM.DesktopPlugin.FilterableView
{
    public class ExampleFilterableViewModel : BaseFilterableViewModel
    {
        public ExampleFilterableViewModel(IClientCoreContainer container) : base(new BaseFilterableViewModelOptions()
        {
            CustomCarFiltering = false,     // if this is true then a view could be created to filter the cars (e.g. as done in the main graph)
        }, "Event.Setups", container)
        {
            CIDViewModelManager = new CIDViewModelManager<ISetupFlatModel>("Default",
                                                                           eCustomPropertyDefinitionType.Setup,
                                                                           setup => new EditSetupSheetViewModel(setup, Id, null),
                                                                           editViewModel => ((EditSetupSheetViewModel)editViewModel).HandleCloseViewModel(),
                                                                           () => new ControlDefinition()
                                                                           {
                                                                               LineDefinitions = new HHObservableCollection<ControlLineDefinition>()
                                                                               {
                                                                                   new ControlLineDefinition()
                                                                                   {
                                                                                       CellDefinitions = new HHObservableCollection<IColumnItemDisplayCellDefinition>()
                                                                                       {
                                                                                           new ControlCellDefinition(new ParameterTextboxDefinition()
                                                                                           {
                                                                                               LinkedParameter = "Name",
                                                                                               StyleOptions = new StyleOptions()
                                                                                               {
                                                                                                   HighlightChange = false,
                                                                                               },
                                                                                           }),
                                                                                       }
                                                                                   },
                                                                               }
                                                                           },
                                                                           "FlatModel");
        }

        public CIDViewModelManager<ISetupFlatModel> CIDViewModelManager { get; private set; }
        private HHObservableCollection<ISetupFlatModel> _shownSetups = new HHObservableCollection<ISetupFlatModel>();

        protected override void AddCustomFilter()
        {            
            var w = new AddCustomFilterWindow(ContextSelection);
            var res = w.ShowDialog();

            if (res.HasValue && res == true)
            {
                AddCustomFilterDefinition(new CustomFilterItemDefinition(w.CustomFilterEntityType, w.Equation));
            }
        }

        protected override void EditCustomFilterDefinition(CustomFilterItemDefinition definition)
        {
            var w = new AddCustomFilterWindow(ContextSelection);
            w.Title = "Edit Custom Filter";
            w.Equation = definition.Expression;
            w.CustomFilterEntityType = definition.CustomFilterEntityType;
            var res = w.ShowDialog();

            if (res.HasValue && res == true)
            {
                definition.Expression = w.Equation;
                definition.CustomFilterEntityType = w.CustomFilterEntityType;
                ApplyFilter();
            }
        }

        protected override string GetDisplayNameForNewContext() => "Example filterable view";

        protected override void HandleFullDataReset()
        {
            if (_contextSelectionFilter == null)
            {
                return;
            }

            _shownSetups.Clear();

            var setupsToAdd = new List<ISetupFlatModel>();

            foreach (var eventCache in _eventCaches)
            {
                if (_contextSelectionFilter.IsEventSelected(eventCache.EventId))
                {
                    foreach (var eventCarCache in eventCache.EventCarCaches)
                    {
                        if (_contextSelectionFilter.IsCarSelected(eventCarCache.CarId))
                        {
                            setupsToAdd.AddRange(eventCarCache.SetupCache.Setups);
                        }
                    }
                }
            }

            if (setupsToAdd.Count == 0)
            {
                return;
            }

            _shownSetups.AddRange(setupsToAdd);
            CIDViewModelManager.InitializeCID(ContextSelection, _shownSetups, setupsToAdd.First().DefinitionId);
        }
    }
}
