using ChemModel.Messeges;
using ChemModel.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ChemModel.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ChemModel.Data.DbTables;
using Microsoft.EntityFrameworkCore;

namespace ChemModel.ViewModels
{
    public partial class AddFormulaViewModel : ObservableObject, IRecipient<FormulasMessage>, IRecipient<EmpiricMessage>, IRecipient<VarMessage>
    {
        private List<EmpiricCoefficient> allEmpiric;
        private List<Property> allProps;
        [ObservableProperty]
        private ObservableCollection<MathModelEmpiricBind> empiricData = new ObservableCollection<MathModelEmpiricBind>();
        [ObservableProperty]
        private ObservableCollection<VarCoefficient> varData = new ObservableCollection<VarCoefficient>();
        [NotifyCanExecuteChangedFor(nameof(DeleteEmpiricCommand))]
        [ObservableProperty]
        private MathModelEmpiricBind? selectedEmpiric = null;
        [NotifyCanExecuteChangedFor(nameof(DeleteVarCommand))]
        [ObservableProperty]
        private VarCoefficient? selectedVar = null;
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [ObservableProperty]
        private string name = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string texFormula = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string formula = "";
        [NotifyCanExecuteChangedFor(nameof(AddEmpiricCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteEmpiricCommand))]
        [NotifyCanExecuteChangedFor(nameof(AddVarCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteVarCommand))]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [ObservableProperty]
        private bool canChange = true;
        Context ctx = new Context();
        [NotifyCanExecuteChangedFor(nameof(ClearFormulaCommand))]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [ObservableProperty]
        private bool canClear = false;
        public AddFormulaViewModel()
        {
            WeakReferenceMessenger.Default.Register<FormulasMessage>(this);
            WeakReferenceMessenger.Default.Register<EmpiricMessage>(this);
            WeakReferenceMessenger.Default.Register<VarMessage>(this);
            allEmpiric = ctx.EmpiricCoefficients.Include(x => x.Units).ToList();
            allProps = ctx.Properties.Include(x => x.Units).ToList();
        }

        [RelayCommand]
        private void ConstructFormula()
        {
            if (EmpiricData.Count <= 0 && VarData.Count <= 0)
            {
                MessageBox.Show("Введите параметры модели", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            List<string> all = new();
            foreach (var item in EmpiricData)
            {
                all.Add(item.Property.Chars);
            }
            foreach (var item in VarData)
            {
                all.Add(item.Property.Chars);
            }
            all.Add("T");
            new FormulaConstructorWindow(all).ShowDialog();
        }
        [RelayCommand(CanExecute = nameof(CanChangeParams))]
        private void AddEmpiric()
        {
            new AddEmpiricWindow(allEmpiric).ShowDialog();
        }
        [RelayCommand(CanExecute = nameof(CanChangeParams))]
        private void AddVar()
        {
            new AddVarWindow(allProps).ShowDialog();
        }
        [RelayCommand(CanExecute = nameof(CanDelEmpiric))]
        private void DeleteEmpiric()
        {
            if (!allEmpiric.Any())
            {
                MessageBox.Show("В базе данных больше нет параметров для добавления", "Добавление запрещено", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (EmpiricData.Count == 0)
            {
                return;
            }
            allEmpiric.Add(SelectedEmpiric.Property);
            EmpiricData.Remove(SelectedEmpiric);
            SelectedEmpiric = null;
        }
        [RelayCommand(CanExecute = nameof(CanDelVar))]
        private void DeleteVar()
        {
            if (!allProps.Any())
            {
                MessageBox.Show("В базе данных больше нет параметров для добавления", "Добавление запрещено", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (VarData.Count == 0)
            {
                return;
            }
            allProps.Add(SelectedVar.Property);
            VarData.Remove(SelectedVar);
            SelectedVar = null;
        }
        private bool CanDelEmpiric()
        {
            return CanChange && SelectedEmpiric is not null;
        }
        private bool CanDelVar()
        {
            return CanChange && SelectedVar is not null;
        }
        private bool CanChangeParams() => CanChange;
        private bool CanClearFormula() => CanClear;
        [RelayCommand(CanExecute = nameof(CanClearFormula))]
        private void ClearFormula()
        {
            CanClear = false;
            CanChange = true;
            Formula = "";
            TexFormula = "";
        }
        private bool CanSave()
        {
           return (!CanChange && CanClear && !string.IsNullOrEmpty(TexFormula) && !string.IsNullOrEmpty(Formula) && !string.IsNullOrEmpty(Name));
        }
        [RelayCommand(CanExecute = nameof(CanSave))]
        private void Save(Window window)
        {
            var mathModel = new MathModel() { Name = Name, Formula = Formula, TexFormula = TexFormula };
            foreach (var empiric in EmpiricData)
            {
                empiric.MathModel = mathModel;
            }
            foreach (var var in VarData)
            {
                var.MathModel = mathModel;
            }
            mathModel.EmpiricCoefficients = EmpiricData.ToList();
            mathModel.VarCoefficients = VarData.ToList();
            foreach (var model in ctx.MathModels.ToList())
            {
                if (model.Name == mathModel.Name || model.Formula == mathModel.Formula)
                {
                    MessageBox.Show("Нельзя добавить уже существующую формулу", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            ctx.MathModels.Add(mathModel);
            ctx.VarCoefficients.AddRange(mathModel.VarCoefficients);
            ctx.MathModelEmpiricBinds.AddRange(mathModel.EmpiricCoefficients);
            ctx.SaveChanges();
            WeakReferenceMessenger.Default.Send(new MathModelMessage(mathModel));
            window.Close();
        }
        public void Receive(FormulasMessage message)
        {
            TexFormula = message.Value.TexFormula!;
            Formula = message.Value.Formula!;
            CanChange = false;
            CanClear = true;
        }

        public void Receive(EmpiricMessage message)
        {
            EmpiricData.Add(message.Value);
            allEmpiric.Remove(message.Value.Property);
        }

        public void Receive(VarMessage message)
        {
            VarData.Add(message.Value);
            allProps.Remove(message.Value.Property);
        }

    }
}
