using Analytics;
using Analytics.Formulae;
using ChemModel.Data;
using ChemModel.Data.DbTables;
using ChemModel.Messeges;
using ChemModel.Windows;
using ChemModel.Windows.AddFormulaWindow;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChemModel.ViewModels
{
    public partial class MathModelsViewModel : ObservableObject, IRecipient<MathModelMessage>
    {
        [ObservableProperty]
        private ObservableCollection<MathModelEmpiricBind>? coefs;
        [ObservableProperty]
        private ObservableCollection<MathModel> mathModels;
        [ObservableProperty]
        private MathModel? selectedModel;
        Context ctx = new Context();
        public MathModelsViewModel()
        {
            MathModels = new ObservableCollection<MathModel>(ctx.MathModels.ToList());
            if (MathModels.Any())
            {
                SelectedModel = MathModels[0];
                Coefs = new ObservableCollection<MathModelEmpiricBind>(ctx.MathModelEmpiricBinds.Where(x => x.MathModelId == SelectedModel.Id).Include(x => x.Property).Include(x => x.Property.Units).ToList());
            }
            WeakReferenceMessenger.Default.Register<MathModelMessage>(this);
        }
        private bool CanDeleteMathModel()
        {
            return SelectedModel is not null;
        }
        [RelayCommand(CanExecute = nameof(CanDeleteMathModel))]
        private void DeleteMathModel()
        {
            if (!MathModels.Any())
                return;
            ctx.MathModels.Remove(ctx.MathModels.Find(SelectedModel!.Id)!);
            MathModels.Remove(SelectedModel!);
            ctx.SaveChanges();
            SelectedModel = null;
            Coefs = null;
        }
        [RelayCommand]
        private void AddMathModel()
        {
            new AddFormulaWindow().ShowDialog();
        }
        public void CoefsChanged()
        {
            if (SelectedModel is null)
                return;
            Coefs = new ObservableCollection<MathModelEmpiricBind>(ctx.MathModelEmpiricBinds.Where(x => x.MathModelId == SelectedModel.Id).Include(x => x.Property).Include(x => x.Property.Units).ToList());
        }
        [RelayCommand]
        private void SaveChanges()
        {
            if (Coefs is null || !Coefs.Any())
            {
                return;
            }
            ctx.MathModelEmpiricBinds.UpdateRange(Coefs);
            ctx.SaveChanges();
            MessageBox.Show("Сохранение прошло успешно", "Сохранение успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void Receive(MathModelMessage message)
        {
            MathModels.Add(message.Value);
        }
    }
}