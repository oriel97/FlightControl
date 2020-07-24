using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulator
{
    /**
     * the model class. 
     * the part of the model who controll all the value and communicate with the servr.
     */
    class Model : IModel
    {
        private static Mutex mutex = new Mutex();
        public event PropertyChangedEventHandler PropertyChanged;
        private ITelnetClient telnetClient;
        public Boolean stop = false;
        private double roll;
        private double verticalSpeed;
        private double pitch;
        private double groundSpeed;
        private double airSpeed;
        private double heading;
        private double altitude;
        private double altimeter;
        private double latitude;
        private double longtitude;
        private string location;
        private double aileron;
        private double rudder;
        private double elevator;
        private double throttle;
        private string errorString = "";
        Queue<string> erroeStack = new Queue<string>();
        // set the stop to be false to make the loop that read 
        // from server to work.
        public void setStop ()
        {
            this.stop = false;
        }
        /**
         * evry error that there is i will present in a board 
         * that will show all the error there is.
         * it work with max of 5 element in the board(with a queue that controll it.
         */
        public string ErrorString
        {
            get { return this.errorString; }
            set
            {
                // check if the error is already in the queue
                if (erroeStack.Contains(value))
                {
                    errorString = "";
                    foreach (string s in erroeStack)
                    {
                        errorString = errorString + "\n" + s;
                    }
                    // notify that there is a change
                    NotifyPropertyChanged("ErrorString");
                }
                else
                {
                    // if the queue is not full(less than 5 element).
                    int size = erroeStack.Count;
                    if (size <= 5)
                    {
                        erroeStack.Enqueue(value);
                        errorString = "";
                        foreach (string s in erroeStack)
                        {
                            errorString = errorString + "\n" + s;
                        }
                        // notify that there is a change
                        NotifyPropertyChanged("ErrorString");
                    }
                    // if the queue is full dequeue element and enqueue a new one.
                    else
                    {
                        erroeStack.Dequeue();
                        erroeStack.Enqueue(value);
                        errorString = "";
                        foreach (string s in erroeStack)
                        {
                            errorString = errorString + "\n" + s;
                        }
                        // notify that there is a change
                        NotifyPropertyChanged("ErrorString");
                    }
                }
            }
        }

        // the location property - controll where the pin is.
        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                // notify that there is a change
                NotifyPropertyChanged("Location");
            }
        }
        // the Longtitude property
        public double Longtitude
        {
            get { return longtitude; }
            set
            {
                longtitude = value;
                // notify that there is a change
                NotifyPropertyChanged("Longitude");
            }
        }
        // the Longtitude property
        public double Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                // notify that there is a change
                NotifyPropertyChanged("Latitude");
            }
        }

        // the Roll property
        public double Roll
        {
            get { return roll; }
            set
            {
                roll = value;
                // notify that there is a change
                NotifyPropertyChanged("Roll");
            }
        }
        // the verticalSpeed property
        public double VerticalSpeed
        {
            get { return verticalSpeed; }
            set
            {
                verticalSpeed = value;
                // notify that there is a change
                NotifyPropertyChanged("VerticalSpeed");
            }
        }
        // the pitch property
        public double Pitch
        {
            get { return pitch; }
            set
            {
                pitch = value;
                // notify that there is a change
                NotifyPropertyChanged("Pitch");
            }
        }
        // the Heading property
        public double Heading
        {
            get { return heading; }
            set
            {
                heading = value;
                // notify that there is a change
                NotifyPropertyChanged("Heading");
            }
        }
        // the groundSpeed property
        public double GroundSpeed
        {
            get { return groundSpeed; }
            set
            {
                groundSpeed = value;
                // notify that there is a change
                NotifyPropertyChanged("GroundSpeed");
            }
        }
        // the altitude property
        public double Altitude
        {
            get { return altitude; }
            set
            {
                altitude = value;
                // notify that there is a change
                NotifyPropertyChanged("Altitude");
            }
        }
        // the altimeter property
        public double Altimeter
        {
            get { return altimeter; }
            set
            {
                altimeter = value;
                // notify that there is a change
                NotifyPropertyChanged("Altimeter");
            }
        }
        // the airSpeed property
        public double AirSpeed
        {
            get { return airSpeed; }
            set
            {
                airSpeed = value;
                // notify that there is a change
                NotifyPropertyChanged("AirSpeed");
            }
        }
        // the aileron property
        public double Aileron
        {
            get { return aileron; }
            set
            {
                aileron = value;
                // notify that there is a change
                NotifyPropertyChanged("Aileron");
            }
        }
        // the throttle property
        public double Throttle
        {
            get { return throttle; }
            set
            {
                throttle = value;
                // notify that there is a change
                NotifyPropertyChanged("Throttle");
            }
        }
        // the Elevator property
        public double Elevator
        {
            get { return elevator; }
            set
            {
                elevator = value;
                // notify that there is a change
                NotifyPropertyChanged("Elevator");
            }
        }
        // the Longtitude property
        public double Rudder
        {
            get { return rudder; }
            set
            {
                rudder = value;
                // notify that there is a change
                NotifyPropertyChanged("Rudder");
            }
        }
        // SetTelnetClient func - connect the tcp to the model.
        public void SetTelnetClient (ITelnetClient tcp)
        {
            this.telnetClient = tcp;
        }
     
        // connect to the server
        public void connect(string ip, int port)
        {
            if (telnetClient.getTcp() == 1)
            {
                // make the connection.
                this.telnetClient.connect(ip, port);
            }
            else
            {
                ErrorString = " Error at connection - the server is closed!";
            }
        }
        // disconnect from the server.
        public void disconnect()
        {
            // before disconnect put the pin in ben gurion airport.
            latitude = 32.005;
            longtitude = 34.887;
            Location = latitude + "," + longtitude;
            // stop the loop
            stop = true;
            // disconnect.
            this.telnetClient.disconnect();
        }

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        }
        //changeThrottle - change the Throttle value after the user moved the sliders.
        public void changeThrottle(double command)
        {
            mutex.WaitOne();
            this.telnetClient.write("set /controls/engines/current-engine/throttle " + command + "\r\n");
            string temp = this.telnetClient.read();
            mutex.ReleaseMutex();
        }
        //changeThrottle - change the Aileron value after the user moved the sliders.
        public void changeAileron(double command)
        {
            mutex.WaitOne();
            this.telnetClient.write("set /controls/flight/aileron " + command + "\r\n");
            string temp = this.telnetClient.read();
            mutex.ReleaseMutex();
        }
        //changeThrottle - change the Elevator value after the user moved the joystick.
        public void changeElevator(double command)
        {
            mutex.WaitOne();
            this.telnetClient.write("set /controls/flight/elevator " + command + "\r\n");
            string temp = this.telnetClient.read();
            mutex.ReleaseMutex();
        }
        //changeThrottle - change the Rudder value after the user moved the joystick.
        public void changeRudder(double command)
        {
            mutex.WaitOne();
            this.telnetClient.write("set /controls/flight/rudder " + command + "\r\n");
            string temp = this.telnetClient.read();
            mutex.ReleaseMutex();
        }
        /**
         *setEverything- after reading all the values of the properties 
         * (pitch,roll...) set them in there properties and then the notify that each
         * property has been changed and by that change the locate of the pin and the
         * properties of the plan.
         */
        void setEverything(String command)
        {
            string[] lines = command.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );
            // try to converte the value from the server to double
            // maybe the values are Err.
            try
            {
                this.Latitude = Convert.ToDouble(lines[8]);
            }
            catch (Exception e)
            {
                ErrorString = "Error at Latitude";
            }
            try
            {
                this.Longtitude = Convert.ToDouble(lines[9]);
            }

            catch (Exception e)
            {
                ErrorString = "Error at Longtitude";
            }
            try
            {
                this.Aileron = Convert.ToDouble(lines[10]);
            }
            catch (Exception e)
            {
                ErrorString = "Error at Aileron";
            }
            try
            {
                this.Throttle = Convert.ToDouble(lines[11]);
            }
            catch (Exception e)
            {
                ErrorString = "Error at Throttle";
            }
            try
            {
                this.Elevator = Convert.ToDouble(lines[12]);
            }
            catch (Exception e)
            {
                ErrorString = "Error at Elevator";
            }
            try
            {
                this.Rudder = Convert.ToDouble(lines[13]);
            }
            catch (Exception e)
            {
                ErrorString = "Error at Rudder";
            }
            // if the location of the plan is out of the world dont set the values.
            if ((longtitude <= 180 && longtitude >= -180) && (latitude <= 90 && latitude >= -90))
            {
                try
                {
                    this.Roll = Convert.ToDouble(lines[0]);
                }
                catch (Exception e)
                {
                    ErrorString = "Error at Roll";
                }
                try
                {
                    this.Pitch = Convert.ToDouble(lines[1]);
                }
                catch (Exception e)
                {
                    ErrorString = "Error at Pitch";
                }
                try
                {
                    this.VerticalSpeed = Convert.ToDouble(lines[2]);
                }
                catch (Exception e)
                {
                    ErrorString = "Error at VerticalSpeed";
                }
                try
                {
                    this.GroundSpeed = Convert.ToDouble(lines[3]);
                }
                catch (Exception e)
                {
                    ErrorString = "Error at GroundSpeed";
                }
                try
                {
                    this.AirSpeed = Convert.ToDouble(lines[4]);
                }
                catch (Exception e)
                {
                    ErrorString = "Error at AirSpeed";
                }
                try
                {
                    this.Heading = Convert.ToDouble(lines[5]);
                }
                catch (Exception e)
                {
                    ErrorString = "Error at Heading";
                }
                try
                {
                    this.Altitude = Convert.ToDouble(lines[6]);
                }
                catch (Exception e)
                {
                    ErrorString = "Error at Altitude";
                }
                try
                {
                    this.Altimeter = Convert.ToDouble(lines[7]);
                }
                catch (Exception e)
                {
                    ErrorString = "Error at Altimeter";
                }
            }
            // the same for the other sensors properties
        }
        /**
         * start func - where everything is happen!
         * reading from the server, set theam in there properties.
         * if the server is crushed or disconnected put the plan in Ben Gurion Air Port
         * and notify that.
         */
        public void start()
        {
            
            new Thread(delegate () {
                while (!stop)
                {
                    try
                    {
                        mutex.WaitOne();
                        String data = "";
                        // reading from the server.
                        telnetClient.write("get /instrumentation/attitude-indicator/internal-roll-deg\r\n " +
                                           "get /instrumentation/attitude-indicator/internal-pitch-deg\r\n " +
                                           "get /instrumentation/gps/indicated-vertical-speed\r\n" +
                                           "get /instrumentation/gps/indicated-ground-speed-kt\r\n" +
                                           "get /instrumentation/airspeed-indicator/indicated-speed-kt\r\n" +
                                           "get /instrumentation/heading-indicator/indicated-heading-deg\r\n" +
                                           "get /instrumentation/gps/indicated-altitude-ft\r\n" +
                                           "get /instrumentation/altimeter/indicated-altitude-ft\r\n" +
                                           "get /position/latitude-deg\r\n" +
                                           "get /position/longitude-deg\r\n" +
                                           "get /controls/flight/aileron\r\n" +
                                           "get /controls/engines/current-engine/throttle\r\n" +
                                           "get /controls/flight/elevator\r\n" +
                                           "get /controls/flight/rudder\r\n");
                        // reading 4 time in a secoend.
                        Thread.Sleep(250);
                        data = telnetClient.read();
                        mutex.ReleaseMutex();
                        setEverything(data);
                        Debug.WriteLine(longtitude + " , " + Latitude);
                        // check if the plan is in earth
                        if ((longtitude <= 180 && longtitude >= -180) && (latitude <= 90 && latitude >= -90))
                        {
                            Location = latitude + "," + longtitude;

                        }
                        else
                        {
                            ErrorString = "go back!! this is the end of the world!";
                        }

                        // read the data in 4Hz
                    }
                    // notify there is timeout
                    catch (IOException e)
                    {
                        ErrorString = "timeout! lets wait some more!";
                    }
                    // if the server crushed or disconnected.
                    catch (Exception e)
                    {
                        ErrorString = "cannot use the server!";
                        mutex.WaitOne();
                        latitude = 32.005;
                        longtitude = 34.887;
                        Location = latitude + "," + longtitude;
                        mutex.ReleaseMutex();
                    }
                }
            }).Start();

        }
    }
}
