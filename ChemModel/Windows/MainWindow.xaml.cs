using System.Windows;

namespace ChemModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closed += (sender, args) => { Owner.Close(); };
        }
    }
}