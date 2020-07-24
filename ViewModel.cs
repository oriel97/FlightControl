using FlightSimulator.views;
using System;
using System.ComponentModel;

namespace FlightSimulator
{
    /**
     * the view model part. 
     * the controller between the model and the view parts.
     */
    public class ViewModel : INotifyPropertyChanged
    {
        private IModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        private double throttle;
        bool connectionPressed = false;
        private double aileronForBind;
        private double throttleForBind;
        private double elevatorForBind;
        private double rudderForBind;

        // VM_Connection properties.
        public bool VM_Connection
        {
            get { return connectionPressed; }
            set
            {
                bool Connection = value;
                this.connectionPressed = Connection;
            }
        }
        // VM_Location properties.
        public string VM_Location
        {
            get { return model.Location; }
        }
        // VM_throttle properties.
        public double VM_throttle
        {
            get { return throttleForBind; }
            set
            {
                throttle = value;
                // change the value of throttle in the model.
                this.model.changeThrottle(throttle);
            }
        }
        // VM_Aileron properties.
        private double aileron;
        public double VM_Aileron
        {
            get { return aileronForBind; }
            set
            {
                aileron = value;
                // change the value of aileron in the model.
                this.model.changeAileron(aileron);
            }
        }

        internal IModel getIM()
        {
            return this.model;
        }
        // VM_elevator properties.
        private double elevator;
        public double VM_elevator
        {
            get { return elevatorForBind; }
            set
            {
                elevator = value;
                // change the value of elevator in the model.
                this.model.changeElevator(elevator);
            }
        }
        // VM_rudder properties.
        private double rudder;
        public double VM_rudder
        {
            get { return rudderForBind; }
            set
            {
                rudder = value;
                // change the value of rudder in the model.
                this.model.changeRudder(rudder);
            }
        }
        public double VM_Roll { get { return model.Roll; } }
        public double VM_Pitch { get { return model.Pitch; } }
        public double VM_GroundSpeed { get { return model.GroundSpeed; } }
        public double VM_AirSpeed { get { return model.AirSpeed; } }
        public double VM_VerticalSpeed { get { return model.VerticalSpeed; } }
        public double VM_Heading { get { return model.Heading; } }
        public double VM_Altimeter { get { return model.Altimeter; } }
        public double VM_Altitude { get { return model.Altitude; } }
        public double VM_Latitude { get { return model.Latitude; } }
        public double VM_Longtitude { get { return model.Longtitude; } }
        public string VM_ErrorString { get { return model.ErrorString; } }

        /**
         * connect - connect to the server and make a new TCP connection. 
         */
        public void connect(string ip, int port)
        {
            ITelnetClient ITelnet = new MyTelnetClient();
            this.model.SetTelnetClient(ITelnet);
            model.connect(ip, port);
            this.model.setStop();
        }
        // start the program.
        public void start()
        {
            model.start();
        }
        // disconnect
        public void disconnect()
        {
            model.disconnect();
        }
        public void ErrorString(string error)
        {
            model.ErrorString = error;
        }

        public void changeRudder(double rudder)
        {
            rudderForBind = rudder;
            model.changeRudder(rudder);
        }

        public void changeElevator(double elevator)
        {
            elevatorForBind = elevator;
            model.changeElevator(elevator);
        }

        public void changeThrottle(double throttle)
        {
            throttleForBind = throttle;
            model.changeThrottle(throttle);
        }

        public void changeAileron(double aileron)
        {
            aileronForBind = aileron;
            model.changeAileron(aileron);
        }
        // the connstructor.
        // connect all the prooperties to the properies here.
        public ViewModel(IModel model1)
        {
            this.model = model1;
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}