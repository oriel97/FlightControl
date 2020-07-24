using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimulator.views
{
    /// <summary>
    /// Interaction logic for wheel.xaml
    /// </summary>
    public partial class wheel : UserControl
    {
        private ViewModel vm;
        public wheel()
        {
            InitializeComponent();
        }

        public void SetVM(ViewModel vm1)
        {
            this.vm = vm1;
            Joystick1.SetVM(vm);
            DataContext = vm;
        }

        private void ab(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (vm.VM_Connection)
            {
                double throttle = throttleSlider1.Value;
                //send the value to the server
                vm.changeThrottle(throttle);
            }
        }

        private void aa(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (vm.VM_Connection)
            {
                double aileron = aileronSlider.Value;
                //send the value to the server
                vm.changeAileron(aileron);
            }
        }
    }
}
