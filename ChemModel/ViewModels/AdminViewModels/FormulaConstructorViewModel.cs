using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analytics;
using Exversion;
using Exversion.Analytics;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Windows;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using CommunityToolkit.Mvvm.Messaging;
using ChemModel.Messeges;

namespace ChemModel.ViewModels
{
    public partial class FormulaConstructorViewModel : ObservableObject
    {
        private List<string> mathArgs;
        [ObservableProperty]
        private string funcArgs = "";
        [ObservableProperty]
        private string formula = "";
        [ObservableProperty]
        private string formulaTex = "";
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [ObservableProperty]
        private bool canSaveInd = false;

        public FormulaConstructorViewModel(List<string> args)
        {
            this.mathArgs = args;
            var builder = new StringBuilder();
            builder.Append("F(");
            for (int i = 0; i < args.Count - 1; i++)
            {
                builder.Append(args[i] + ",");
            }
            builder.Append(args[args.Count - 1] + ")=");
            FuncArgs = builder.ToString();
        }
        [RelayCommand]
        private void Draw()
        {
            if (string.IsNullOrEmpty(Formula))
            {
                MessageBox.Show("Введите формулу", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            AnalyticsTeXConverter converter = new AnalyticsTeXConverter();
            string side;
            try
            {
                side = converter.Convert(Formula);
                CanSaveInd = true;
                if (side.Length == 0)
                {
                    throw new Exception();
                }
            }
            catch
            {
                MessageBox.Show("Данная формула не является возможной", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                CanSaveInd = false;
                return;
            }
            for (int i = 0; i < mathArgs.Count; i++)
            {
                var curArg = mathArgs[i];
                var allInd = Formula.AllIndexesOf(curArg);
                if (allInd.Count == 0)
                {
                    CanSaveInd = false;
                    MessageBox.Show("Необходимо, чтобы все заданные аргументы использовались в формуле", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                bool isIn = false;
                foreach (int ind in allInd)
                {
                    if ((ind - 1 < 0 || !char.IsLetter(Formula[ind - 1])) && (ind + curArg.Length >= Formula.Length || !char.IsLetter(Formula[ind + curArg.Length])))
                    {
                        isIn = true;
                    }
                }
                if (!isIn)
                {
                    CanSaveInd = false;
                    MessageBox.Show("Необходимо, чтобы все заданные аргументы использовались в формуле", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                } 
            }
            FormulaTex = FuncArgs + side;
            CanSaveInd = true;
        }
        [RelayCommand(CanExecute = nameof(CanSave))]
        private void Save(Window window)
        {
            Draw();
            if (!CanSaveInd) { return; }
            WeakReferenceMessenger.Default.Send(new FormulasMessage(new FormulaConstructResult() { Formula = Formula, TexFormula = FormulaTex}));
            window.Close();
        }
        private bool CanSave() => CanSaveInd;
    }
}
