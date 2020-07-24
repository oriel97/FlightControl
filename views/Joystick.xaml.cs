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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimulator.views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Joystick : UserControl
    {
        public bool MousePressed;
        public double firstX;
        public double firstY;
        public double vmXAileron;
        public double vmYElevator;
        public Point firstPoint;
        public Point VMPiont;
        private ViewModel vm;
        public Joystick()
        {
            InitializeComponent();
        }

        public void SetVM(ViewModel vm1)
        {
            this.vm = vm1;
        }
        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //check that the mouse pressed 
            if (!MousePressed)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    //get the position of the knob
                    firstPoint.X = e.GetPosition(this).X;
                    firstPoint.Y = e.GetPosition(this).Y;
                    Knob.CaptureMouse();
                    MousePressed = true;
                }
            }
        }
        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (!MousePressed) return;
            double x = e.GetPosition(this).X - firstPoint.X;
            double y = e.GetPosition(this).Y - firstPoint.Y;
            if (Math.Sqrt(x * x + y * y) < 125)
            {
                knobPosition.X = x;
                knobPosition.Y = y;
                if (vm.VM_Connection)
                {
                    //set the new values to the server
                    vm.changeRudder(x / 125);
                    vm.changeElevator(-y / 125);
                }
            }
        }
        private void Knob_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MousePressed = false;
            Storyboard story = (Storyboard)Knob.FindResource("CenterKnob");
            story.Begin();
            Knob.ReleaseMouseCapture();
            if (vm.VM_Connection)
            {
                vm.changeRudder(0);
                vm.changeElevator(0);
            }
        }

        private void centerKnob_Completed(object sender, EventArgs e)
        {
            Storyboard story = (Storyboard)Knob.FindResource("CenterKnob");
            story.Stop();
            knobPosition.X = 0;
            knobPosition.Y = 0;
        }
    }
}
