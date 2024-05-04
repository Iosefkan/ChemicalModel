using Analytics;
using Analytics.Variables;
using ChemModel.Data;
using ChemModel.Data.DbTables;
using ChemModel.Messeges;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ChemModel.ViewModels
{
    public partial class ParamsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Material> materials;
        [ObservableProperty]
        private ObservableCollection<MathModel> mathModels;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SolveCommand))]
        private Material? selectedMaterial;
        [NotifyCanExecuteChangedFor(nameof(SolveCommand))]
        [ObservableProperty]
        private MathModel? selectedModel;
        [NotifyCanExecuteChangedFor(nameof(SolveCommand))]
        [ObservableProperty]
        private double width = 0.25;
        [NotifyCanExecuteChangedFor(nameof(SolveCommand))]
        [ObservableProperty]
        private double length = 8.3;
        [NotifyCanExecuteChangedFor(nameof(SolveCommand))]
        [ObservableProperty]
        private double height = 0.005;
        [NotifyCanExecuteChangedFor(nameof(SolveCommand))]
        [ObservableProperty]
        private double velocity = 1.7;
        [ObservableProperty]
        private double temperature = 195;
        [NotifyCanExecuteChangedFor(nameof(SolveCommand))]
        [ObservableProperty]
        private double step = 0.1;
        [ObservableProperty]
        private ObservableCollection<MaterialEmpiricBind>? coefs;
        [ObservableProperty]
        private ObservableCollection<MaterialPropertyBind>? properties;
        public ParamsViewModel() 
        {
            using var ctx = new Context();
            Materials = new ObservableCollection<Material>(ctx.Materials.ToList());
            MathModels = new ObservableCollection<MathModel>(ctx.MathModels.ToList());
            if (Materials.Any())
            {
                SelectedMaterial = Materials[0];
            }
            if (MathModels.Any())
            {
                SelectedModel = MathModels[0];
            }
        }
        public void MaterialSelected()
        {
            if (SelectedMaterial is null)
                return;
            using var ctx = new Context();
            Properties = new ObservableCollection<MaterialPropertyBind>(ctx.MaterialPropertyBinds.Where(x => x.MaterialId == SelectedMaterial.Id).Include(x => x.Property).Include(x => x.Property.Units).ToList());
            if (SelectedModel is null)
            {
                return;
            }
            var mathEmpirics = ctx.EmpiricCoefficientMaths.Where(x => x.MathModelId == SelectedModel.Id).Select(x => x.PropertyId).ToList();
            Coefs = new ObservableCollection<MaterialEmpiricBind>(ctx.MaterialEmpiricBinds.Where(x => x.MaterialId == SelectedMaterial.Id && mathEmpirics.Contains(x.Id)).Include(x => x.Property).Include(x => x.Property.Units).ToList());
        }
        public void MathModelSelected()
        {
            if (SelectedMaterial is null || SelectedModel is null)
            {
                return;
            }
            using var ctx = new Context();
            var mathEmpirics = ctx.EmpiricCoefficientMaths.Where(x => x.MathModelId == SelectedModel.Id).Select(x => x.PropertyId).ToList();
            Coefs = new ObservableCollection<MaterialEmpiricBind>(ctx.MaterialEmpiricBinds.Where(x => x.MaterialId == SelectedMaterial.Id && mathEmpirics.Contains(x.Id)).Include(x => x.Property).Include(x => x.Property.Units).ToList());
        }
        private bool CanSolve() =>
            SelectedMaterial is not null && SelectedModel is not null && Width > 0 && Length > 0 && Height > 0 && Velocity > 0 && Step > 0;
        [RelayCommand(CanExecute = nameof(CanSolve))]
        private void Solve()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            long mathOperCount = 0;
            Translator translator = new Translator();
            foreach (var coef in Coefs!)
            {
                translator.Add(coef.Property.Chars, coef.Value);
            }
            foreach (var prop in Properties!)
            {
                translator.Add(prop.Property.Chars, prop.Value);
            }
            translator.Add("T", 0);
            double index = Coefs!.First(x => x.Property.Chars == "n").Value;
            double termCoef = Coefs!.First(x => x.Property.Chars == "au").Value;
            var formula = translator.BuildFormula(SelectedModel!.Formula);
            int formulaOpers = CountMathOperations(SelectedModel.Formula);
            Variable tempVar = translator.Get("T");
            double geomCoef = 0.125 * Math.Pow(Height / Width, 2) - 0.625 * (Height / Width) + 1;
            mathOperCount += 7;
            double Qch = ((Height * Width * Velocity) / 2) * geomCoef;
            mathOperCount += 4;
            double gamma = Velocity / Height;
            mathOperCount++;
            double mu0 = Coefs!.First(x => x.Property.Chars == "mu0").Value;
            double qGamma = Height * Width * mu0 * Math.Pow(gamma, index + 1);
            mathOperCount += 5;
            double Tr = Coefs!.First(x => x.Property.Chars == "Tr").Value;
            var bc = Coefs.FirstOrDefault(x => x.Property.Chars! == "b");
            double b = 0;
            if (bc is null)
            {
                Translator trans2 = new Translator();
                foreach (var coef in Coefs!)
                {
                    trans2.Add(coef.Property.Chars, coef.Value);
                }
                foreach (var prop in Properties!)
                {
                    trans2.Add(prop.Property.Chars, prop.Value);
                }
                string form = SelectedModel.Formula.Replace("*(T-Tr))", "");
                form = form.Replace("mu0*e^(-","");
                mathOperCount += CountMathOperations(form);
                try
                {
                    b = (double)trans2.Calculate(form);
                }
                catch
                {
                    MessageBox.Show("Моделирование с данным уравнением невозможно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                b = bc.Value;
            }
            double Ro = Properties!.First(x => x.Property.Chars == "Ro").Value;
            double c = Properties!.First(x => x.Property.Chars == "c").Value;
            double T0 = Properties!.First(x => x.Property.Chars == "T0").Value;
            double qAlpha = Width * termCoef * (Math.Pow(b, -1) - Temperature + Tr);
            mathOperCount += 5;
            int N = (int)(Length / Step);
            mathOperCount++;
            TableData[] data = new TableData[N + 1];
            for (int i = 0; i <= N; i++)
            {
                double z = Step * i;
                double T = Tr + (1 / b) * Math.Log(((b * qGamma + Width * termCoef) /(b*qAlpha)) * (1 - Math.Exp(-(b * qAlpha)/(Ro * c * Qch)*z)) + Math.Exp(b * (T0 - Tr - (qAlpha/(Ro * c * Qch))*z)));
                tempVar.Value = T;
                double vaz = (double)formula.Calculate() * Math.Pow(gamma, index - 1);
                data[i] = new TableData() { Coord = Math.Round(z, 5), Temp = T, Vaz = vaz};
                mathOperCount += 29 + formulaOpers;
            }
            stopwatch.Stop();
            long miliseconds = stopwatch.ElapsedMilliseconds;
            WeakReferenceMessenger.Default.Send(new DataMessage(data));
            WeakReferenceMessenger.Default.Send(new SolveParamsMessage(new SolveParams() { Miliseconds = miliseconds, Operations = mathOperCount }));
            WeakReferenceMessenger.Default.Send(new ResultDataMessage(new ViewModelsData.ResultData() { Temperature = data[data.Length - 1].Temp, Performance = Ro * Qch * 3600, Viscosity = data[data.Length - 1].Vaz }));
            WeakReferenceMessenger.Default.Send(new DataExcelMessage(new ViewModelsData.DataExcel()
            {
                Coefs = Coefs.ToList(),
                Properties = Properties.ToList(),
                Velocity = Velocity,
                Height = Height,
                Length = Length,
                Width = Width,
                Material = SelectedMaterial!, 
                Step = Step,
                TempCr = Temperature,
                MathModel = SelectedModel,
            }));
            MessageBox.Show("Расчет прошел успешно", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private int CountMathOperations(string formula)
        {
            char[] opers = new char[] { '+', '-', '*', '/', '^' };
            return formula.Split(opers).Length - 1;
        }
    }
}
