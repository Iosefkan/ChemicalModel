using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using ChemModel.Data;
using System.Diagnostics.Metrics;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using ChemModel.Data.DbTables;
using System.Windows.Controls;
using ChemModel.Windows.AddFormulaWindow;
using Microsoft.EntityFrameworkCore.Query;
using ChemModel.Windows;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ChemModel.ViewModels
{
    public partial class AuthViewModel : ObservableObject
    {
        private PasswordBox pwb;
        [ObservableProperty]
        private string error = "";
        [ObservableProperty]
        private string login = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EnterCommand))]
        private int tryEnterCount = 0;

        public AuthViewModel(PasswordBox pwb)
        {
            this.pwb = pwb;
        }
        [RelayCommand(CanExecute = nameof(CanEnter))]
        private void Enter(Window window)
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(pwb.Password))
            {
                Error = "Введите данные";
                return;
            }
            using var ctx = new Context();
            //var userD = new Role() { Name = "user" };
            //var admin = new Role() { Name = "admin" };
            //ctx.Roles.AddRange([userD, admin]);
            //ctx.Users.Add(new User() { Name = "admin", Password = GetHash256("admin"), Role = admin });
            //var non = new Unit() { Name = "-" };
            //var au = new Unit() { Name = "Вт/(м^2*°С)" };
            //ctx.Units.AddRange([non, au]);
            //var n = new MaterialMathModelProperty()
            //{
            //    Chars = "n",
            //    Name = "Индекс течения материала",
            //    Units = non,
            //};
            //var auu = new MaterialMathModelProperty()
            //{
            //    Chars = "au",
            //    Name = "Коэффициент теплоотдачи от крышки канала к материалу",
            //    Units = au,
            //};
            //ctx.MaterialMathModelProperties.AddRange([n, auu]);
            //ctx.SaveChanges();
            //return;
            var user = ctx.Users.Include(x => x.Role).FirstOrDefault(x => x.Name == Login && x.Password == HashStatic.GetHash256(pwb.Password));
            if (user is null)
            {
                TryEnterCount++;
                Error = "Неверный логин и/или пароль";
                return;
            }
            if (user.Role!.Name == "admin")
            {
                var main = new AdminWindow() { Owner = window };
                window.Hide();
                main.ShowDialog();
            }
            else if (user.Role.Name == "user")
            {
                var main = new ResearcherWindow() { Owner = window };
                window.Hide();
                main.ShowDialog();
            }
        }
        private bool CanEnter() =>
            TryEnterCount <= 10;
        
    }
}
