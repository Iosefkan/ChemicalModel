using ChemModel.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChemModel.Windows.AddFormulaWindow
{
    /// <summary>
    /// Interaction logic for AddFormulaWindow.xaml
    /// </summary>
    public partial class AddFormulaWindow : Window
    {
        public AddFormulaWindow()
        {
            InitializeComponent();
            varGrid.AutoGeneratingColumn += AutoGeneratingColumn;
            empGrid.AutoGeneratingColumn += AutoGeneratingColumn;
            DataContext = new ViewModels.AddFormulaViewModel();
        }
        void AutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
            }
        }
    }
}
