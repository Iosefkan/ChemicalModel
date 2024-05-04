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
    internal partial class AddUserViewModel : ObservableObject
    {
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        [ObservableProperty]
        private string login = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string password = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string role = "";
        public AddUserViewModel(string addRole)
        {
            Role = addRole;
        }
        private bool CanOk()
        {
            return !string.IsNullOrEmpty(Role) && !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
        }
        [RelayCommand(CanExecute = nameof(CanOk))]
        private void Ok(Window window)
        {
            WeakReferenceMessenger.Default.Send(new NewUserMessage(new NewUser() { Name = Login, Password = HashStatic.GetHash256(Password), Role = Role }));
            window.Close();
        }
    }
}
