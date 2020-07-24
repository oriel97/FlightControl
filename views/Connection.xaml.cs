using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimulator.views
{
    /// <summary>
    /// Interaction logic for Connection.xaml
    /// </summary>
    public partial class Connection : UserControl
    {
        private ViewModel vm;
        public Connection()
        {
            InitializeComponent();
        }

        public void SetVM(ViewModel vm)
        {
            this.vm = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string portSt = portText.Text;
            string ipSt = ipText.Text;
            int portNum;
            if (portSt.Equals("") || ipText.Equals(""))
            {
                vm.ErrorString("fill the port and the ip");
                return;//no port or no ip
            }
            if (!vm.VM_Connection)
            {
                //try to connect the server
                try
                {
                    portNum = int.Parse(portSt);
                    vm.connect(ipSt, portNum);
                    vm.VM_Connection = true;
                    vm.start();
                }
                catch(Exception exeption)
                {

                    vm.VM_Connection = false;
                    vm.ErrorString("try to connect again");
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (vm.VM_Connection)
            {
                vm.VM_Connection = false;
                vm.disconnect();
            }
        }
    }
}
