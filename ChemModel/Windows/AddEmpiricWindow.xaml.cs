using ChemModel.Data.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChemModel.Windows
{
    /// <summary>
    /// Interaction logic for AddEmpiricWindow.xaml
    /// </summary>
    public partial class AddEmpiricWindow : Window
    {
        public AddEmpiricWindow(List<EmpiricCoefficient> empirics)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddEmpiricViewModel(empirics);
        }
        private static readonly Regex _reg = new Regex("[^0-9,-]+");
        private static bool IsTextAllowed(string text)
        {
            return !_reg.IsMatch(text);
        }
        private void TextBox_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
    }
}
