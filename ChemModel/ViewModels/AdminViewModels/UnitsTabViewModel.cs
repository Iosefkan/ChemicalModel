using ChemModel.Data;
using ChemModel.Data.DbTables;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.ViewModels
{
    public partial class UnitsTabViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Unit> units;
        [NotifyCanExecuteChangedFor(nameof(DeleteUnitCommand))]
        [ObservableProperty]
        private Unit? selectedUnit;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddUnitCommand))]
        private string newUnit = "";
        public UnitsTabViewModel() 
        {
            using Context ctx = new Context();
            Units = new ObservableCollection<Unit>(ctx.Units.ToList());
            if (Units.Any())
            {
                SelectedUnit = Units[0];
            }
        }
        public bool CanAdd() => !string.IsNullOrEmpty(NewUnit);
        [RelayCommand(CanExecute = nameof(CanAdd))]
        public void AddUnit()
        {
            using Context ctx = new Context();
            var unit = new Unit { Name = NewUnit };
            Units.Add(unit);
            ctx.Units.Add(unit);
            ctx.SaveChanges();
            NewUnit = "";
        }
        public bool CanDelete() => SelectedUnit is not null;
        [RelayCommand(CanExecute = nameof(CanDelete))]
        public void DeleteUnit()
        {
            if (Units is null || !Units.Any())
            {
                return;
            }
            using Context ctx = new Context();
            ctx.Units.Remove(SelectedUnit!);
            ctx.SaveChanges();
            Units.Remove(SelectedUnit!);
            SelectedUnit = null;
        }
    }
}
