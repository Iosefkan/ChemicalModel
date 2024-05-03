using Analytics.Variables;
using ChemModel.Data.DbTables;
using ChemModel.Messeges;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChemModel.ViewModels
{
    public partial class AddVarViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<Property> vars;
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        [ObservableProperty]
        private Property? selectedVar;
        public AddVarViewModel(List<Property> vars)
        {
            this.vars = vars;
        }

        [RelayCommand(CanExecute = nameof(CanOk))]
        private void Ok(Window window)
        {
            WeakReferenceMessenger.Default.Send(new VarMessage(new VarCoefficient()
            {
                Property = SelectedVar!,
                PropertyId = SelectedVar!.Id,
            }));
            window.Close();
        }

        private bool CanOk() => SelectedVar is not null;
    }
}
