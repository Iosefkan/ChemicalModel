using ChemModel.Data.DbTables;
using ChemModel.Messeges;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChemModel.ViewModels
{
    public partial class AddEmpiricViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<EmpiricCoefficient> _allEmpirics;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private EmpiricCoefficient? selectedEmpiric;
        public AddEmpiricViewModel(List<EmpiricCoefficient> empirics)
        {
            _allEmpirics = empirics;
        }
        [RelayCommand(CanExecute = nameof(CanOk))]
        private void Ok(Window window)
        {
            WeakReferenceMessenger.Default.Send(new EmpiricMessage(new EmpiricCoefficientMathModel()
            {
                Property = SelectedEmpiric!,
                PropertyId = SelectedEmpiric!.Id,
            }));
            window.Close();
        }

        private bool CanOk() => SelectedEmpiric is not null;

    }
}
