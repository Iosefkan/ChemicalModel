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
using System.Windows.Media.Media3D;

namespace ChemModel.ViewModels
{
    public partial class MathModelsViewModel : ObservableObject, IRecipient<MathModelMessage>, IRecipient<UserMessage>
    {
        private User? user;
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
        }
        public void Receive(MathModelMessage message)
        {
            MathModels.Add(message.Value);
            if (user is not null)
            {
                ctx.UserAddMathModels.Add(new UserAddMathModel()
                {
                    MathModel = message.Value,
                    MathModelId = message.Value.Id,
                    User = user,
                    UserId = user.Id,
                });
                ctx.SaveChanges();
            }
        }

        public void Receive(UserMessage message)
        {
            user = message.Value;
        }
    }
}