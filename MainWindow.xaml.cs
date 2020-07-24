using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.views;
using System.Diagnostics;

namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IModel server;
        public ViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            server = new Model();
            vm = new ViewModel(server);
            DashBoard1.SetVM(vm);
            Connectetion1.SetVM(vm);
            wheel1.SetVM(vm);
            DataContext = vm;

        }

    }

}