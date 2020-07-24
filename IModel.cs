using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator
{
    public interface IModel : INotifyPropertyChanged
    {
        void connect(string ip, int port);
        void start();
        void disconnect();

        double Roll { get; set; }
        double Pitch { get; set; }
        double GroundSpeed { get; set; }
        double AirSpeed { get; set; }
        double Heading { get; set; }
        double VerticalSpeed { get; set; }
        double Altitude { get; set; }
        double Altimeter { get; set; }
        double Latitude { get; set; }
        double Longtitude { get; set; }
        double Aileron { get; set; }
        double Throttle { get; set; }
        double Rudder { get; set; }
        double Elevator { get; set; }
        string Location { get; set; }
        string ErrorString { get; set; }

        void SetTelnetClient(ITelnetClient tcp);
        void changeThrottle(double command);

        void setStop();
        void changeAileron(double command);

        void changeElevator(double command);

        void changeRudder(double command);


    }
}