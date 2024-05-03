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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ChemModel.ViewModels
{
    public partial class AdminViewModel : ObservableObject
    {
        [ObservableProperty]
        private string mem = "";

        private DispatcherTimer timer;
        public AdminViewModel()
        {
            timer = new();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (e, args) =>
            {
                using Process proc = Process.GetCurrentProcess();
                Mem = Math.Round((double)(proc.PrivateMemorySize64 / (1024 * 1024)), 2) + " МБ";
            };
            timer.Start();
        }
    }
}
