using ChemModel.Data.DbTables;
using ChemModel.Data;
using ChemModel.Messeges;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using ChemModel.Windows;

namespace ChemModel.ViewModels
{
    public partial class UsersTabViewModel : ObservableObject, IRecipient<UserMessage>
    {
        private readonly Dictionary<string, string> roles = new Dictionary<string, string>()
        {
            { "Исследователь", "user"},
            { "Администратор", "admin"},
        };
        [ObservableProperty]
        private ObservableCollection<UserGrid> researchers;
        [ObservableProperty]
        private ObservableCollection<UserGrid> admins;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteResearcherCommand))]
        private UserGrid? selectedResearcher;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteAdminCommand))]
        private UserGrid? selectedAdmin;

        public UsersTabViewModel()
        {
            using Context ctx = new Context();
            Researchers = new ObservableCollection<UserGrid>(ctx.Users.Include(x => x.Role).Where(x => x.Role!.Name == "user").Select(x => new UserGrid() { Id = x.Id, Name = x.Name, Password = x.Password }).ToList());
            Admins = new ObservableCollection<UserGrid>(ctx.Users.Include(x => x.Role).Where(x => x.Role!.Name == "admin").Select(x => new UserGrid() { Id = x.Id, Name = x.Name, Password = x.Password }).ToList());
            if (Researchers.Any())
            {
                SelectedResearcher = Researchers[0];
            }
            if (Admins.Any())
            {
                SelectedAdmin = Admins[0];
            }
            WeakReferenceMessenger.Default.Register<UserMessage>(this);
        }
        [RelayCommand]
        private void AddResearcher()
        {
            AddUser add = new AddUser("Исследователь");
            add.ShowDialog();
        }
        [RelayCommand(CanExecute = nameof(CanDelRes))]
        private void DeleteResearcher()
        {
            if (!Researchers.Any())
                return;
            var res = MessageBox.Show("Вы уверены, что хотите удалить данного исследователя", "Подтверждение", MessageBoxButton.YesNo);
            if (res != MessageBoxResult.Yes)
                return;
            using Context ctx = new Context();
            var user = ctx.Users.Find(SelectedResearcher!.Id);
            if (user is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ctx.Users.Remove(user);
            ctx.SaveChanges();
            Researchers.Remove(SelectedResearcher);
            SelectedResearcher = null;
        }
        private bool CanDelRes => SelectedResearcher is not null;
        [RelayCommand]
        private void AddAdmin()
        {
            AddUser add = new AddUser("Администратор");
            add.ShowDialog();
        }
        [RelayCommand(CanExecute = nameof(CanDelAdm))]
        private void DeleteAdmin()
        {
            if (!Admins.Any())
                return;
            var res = MessageBox.Show("Вы уверены, что хотите удалить данного администратора", "Подтверждение", MessageBoxButton.YesNo);
            if (res != MessageBoxResult.Yes)
                return;
            using Context ctx = new Context();
            var user = ctx.Users.Find(SelectedAdmin!.Id);
            if (user is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ctx.Users.Remove(user);
            ctx.SaveChanges();
            Admins.Remove(SelectedAdmin);
            SelectedAdmin = null;
        }

        public void Receive(UserMessage message)
        {
            NewUser user = message.Value;
            string role = user.Role;
            string? dbRole = roles.GetValueOrDefault(role);
            if (dbRole is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            using Context ctx = new Context();
            Role? getRole = ctx.Roles.First(x => x.Name == dbRole);
            if (getRole is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            User addUser = new User() { Name = user.Name, Password = user.Password, Role = getRole };
            ctx.Users.Add(addUser);
            ctx.SaveChanges();
            if (dbRole == "admin")
            {
                Admins.Add(new UserGrid() { Id = addUser.Id, Name = addUser.Name, Password = addUser.Password });
            }
            if (dbRole == "user")
            {
                Researchers.Add(new UserGrid() { Id = addUser.Id, Name = addUser.Name, Password = addUser.Password });
            }
        }

        private bool CanDelAdm => SelectedAdmin is not null;
    }
}
