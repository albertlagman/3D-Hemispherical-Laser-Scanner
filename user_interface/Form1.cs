using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Collections.Concurrent;
//For launching external programs
using System.Diagnostics;

//EMGU CV imports
using Emgu.CV;
using Emgu.CV.CvEnum; 
using Emgu.CV.Structure;
using Emgu.CV.UI;     

namespace user_interface
{
    public partial class Form : System.Windows.Forms.Form
    {
        string[] serialPortNames = SerialPort.GetPortNames();           // get port names
        int zAxisDeg = 0;          // indicate the position the machine must rotate to
        int xAxisDeg = 0;          // indicate the position the machine must rotate to
        int zPos = 0;           // tick position of z-axis
        int xPos = 0;           // tick position of x-axis
        int receivedZPos = 0;       // current position of z-axis in ticks
        int receivedXPos = 0;       // current position of x-axis in ticks
        Mat imgCurrent;

        byte[] TxBytes = new Byte[2];         // array of 2 bytes
        byte[] RxBytes = new Byte[4];
        ConcurrentQueue<byte> RxQueue = new ConcurrentQueue<byte>();
        StreamWriter fileStream;        // create data file
        Capture capWebcam;
        bool isRecording = true;            // turn on recording to data file
 
        //For running Processing
        ProcessStartInfo start = new ProcessStartInfo();
        int exitCode;

        public Form()
        {
            InitializeComponent();
            comboSerialPort.Items.AddRange(serialPortNames);            // list accessible serial ports

            // Enter in the command line arguments, everything you would enter after the executable name itself
            start.Arguments = "--sketch=\"C:\\Users\\albert\\Documents\\UBC\\MECH423\\Final Project\\user_interface\\bin\\x64\\Debug\\sketch_3D_Setup\" --run";
            // Enter the executable to run, including the complete path
            start.FileName = "C:\\Program Files\\processing-3.0b5\\processing-java.exe";
            // Do you want to show a console window?
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (connectButton.Text == "Connect")
            {
                // connect to serialPort1
                if (comboSerialPort.Text != null)    // if there's something in the dropdown box
                {
                    serialPort1.PortName = comboSerialPort.SelectedItem.ToString();
                    serialPort1.Open();
                    connectButton.Text = "Disconnect";
                    statusBox.Text = "Connected";
                    statusBox.BackColor = Color.Lime;
                }
            }
            else    // Disconnect
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();            // close serial port
                }
                connectButton.Text = "Connect";
                statusBox.Text = "Disconnected";
                statusBox.BackColor = Color.Red;
            }
        }

        private void loadButton_Click(object sender, EventArgs e)           // this commands Custom Angular Control
        {
            // check to make sure parameters make sense and serial port is open
            fileStream = new StreamWriter("sketch_3D_Setup\\data.txt", false);
            if (zAxisTextBox.Text != "" && (x0degRadioButton.Checked == true ||
                x45degRadioButton.Checked == true || x90degRadioButton.Checked == true) && 
                serialPort1.IsOpen)
            {
                // change status
                statusBox.Text = "Scanning";
                statusBox.BackColor = Color.Yellow;
                
                // obtain parameters
                zPos = convertAngle2zTick(Convert.ToInt32(zAxisTextBox.Text));
                if (x0degRadioButton.Checked == true)
                {
                    xPos = 0;
                }
                else if (x45degRadioButton.Checked == true)
                {
                    xPos = 1;
                }
                else
                {
                    xPos = 2;
                }


                // send command
                for(int i = 0; i < zPos + 1; i++)
                {
                    sendTickMessage(xPos);
                    receiveMessage();
                    receivedXtextBox.Text = receivedXPos.ToString();
                    receivedZtextBox.Text = receivedZPos.ToString();
                    receivedXtextBox.Refresh();
                    receivedZtextBox.Refresh();
                    processSingleFrameAndUpdateGUI();                   // take a picture and write to file
                }

                // send reset message
                sendResetMessage();
                receiveMessage();
                receivedXtextBox.Text = receivedXPos.ToString();
                receivedZtextBox.Text = receivedZPos.ToString();
                receivedXtextBox.Refresh();
                receivedZtextBox.Refresh();

                // change status
                statusBox.Text = "Done Scanning";
                statusBox.BackColor = Color.Lime;
            }
            fileStream.Close();
            runProcessing();

        }

        private void scan360degButton_Click(object sender, EventArgs e)
        {
            fileStream = new StreamWriter("sketch_3D_Setup\\data.txt", false);

            if (serialPort1.IsOpen)      // check serial port is open
            {
                // change status
                statusBox.Text = "Scanning";
                statusBox.BackColor = Color.Yellow;

                xPos = 0;
                zPos = 400;

                // send command
                for (int i = 0; i < zPos + 1; i++)
                {
                    sendTickMessage(xPos);
                    receiveMessage();
                    receivedXtextBox.Text = receivedXPos.ToString();
                    receivedZtextBox.Text = receivedZPos.ToString();
                    receivedXtextBox.Refresh();
                    receivedZtextBox.Refresh();
                    processSingleFrameAndUpdateGUI();                   // take a picture and write to file
                }

                // send reset message
                sendResetMessage();
                receiveMessage();
                receivedXtextBox.Text = receivedXPos.ToString();
                receivedZtextBox.Text = receivedZPos.ToString();
                receivedXtextBox.Refresh();
                receivedZtextBox.Refresh();

                // change status
                statusBox.Text = "Done Scanning";
                statusBox.BackColor = Color.Lime;
            }

            fileStream.Close();
            runProcessing();
        }

        private void scanHemiButton_Click(object sender, EventArgs e)
        {
            fileStream = new StreamWriter("sketch_3D_Setup\\data.txt", false);

            if (serialPort1.IsOpen)      // check serial port is open
            {
                // change status
                statusBox.Text = "Scanning";
                statusBox.BackColor = Color.Yellow;

                xPos = 0;
                zPos = 400;
                while (xPos < 3)
                {
                    // send command
                    for (int i = 0; i < zPos + 1; i++)
                    {
                        sendTickMessage(xPos);
                        receiveMessage();
                        receivedXtextBox.Text = receivedXPos.ToString();
                        receivedZtextBox.Text = receivedZPos.ToString();
                        receivedXtextBox.Refresh();
                        receivedZtextBox.Refresh();
                        processSingleFrameAndUpdateGUI();                   // take a picture and write to file
                    }

                    // send reset message
                    sendResetMessage();
                    receiveMessage();
                    receivedXtextBox.Text = receivedXPos.ToString();
                    receivedZtextBox.Text = receivedZPos.ToString();
                    receivedXtextBox.Refresh();
                    receivedZtextBox.Refresh();

                    xPos = xPos + 1;    // move to next tilt
                }
                // change status
                statusBox.Text = "Done Scanning";
                statusBox.BackColor = Color.Lime;
            }

            fileStream.Close();
            runProcessing();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {

            if (serialPort1.IsOpen)      // check serial port is open
            {
                sendResetMessage();
                serialPort1.Close();
                statusBox.Text = "Disconnected";
                statusBox.BackColor = Color.Red;
            }
            fileStream.Close();
        }

        private int convertAngle2zTick(int zDeg)         // this function converts an inputted angle to a stepper motor tick z-position
        {
            int tickPos = (int) Math.Ceiling((float)zDeg/0.9);
            return tickPos;
        }

        private float convertZTick2Angle(int tick)         // this function converts an inputted stepper motor tick z-position to an angle
        {
            // fix this stuff
            float angle = (float) (tick * 0.9);
            return angle;
        }

        private void sendTickMessage(int xPos)          // this function sends a data packet indicating a tick request to the serial port
        {
            int txByte = 255;                   // start byte
            TxBytes[0] = Convert.ToByte(txByte);

            // xPos is 0, 1, or 2
            txByte = xPos;
            TxBytes[1] = Convert.ToByte(txByte);

            // Send TxBytes -----------------------------------------------------------
            if (serialPort1.IsOpen)                         // if serial port is connected
            {
                serialPort1.Write(TxBytes, 0, 1);           // write TxBytes[0]
                serialPort1.Write(TxBytes, 1, 1);           // write TxBytes[1] 
            }
        }

        private void sendResetMessage()          // this function sends a data packet indicating a reset request to the serial port
        {
            int txByte = 255;                   // start byte
            TxBytes[0] = Convert.ToByte(txByte);

            // xPos is 0, 1, or 2
            txByte = 128;                  // reset byte
            TxBytes[1] = Convert.ToByte(txByte);

            // Send TxBytes -----------------------------------------------------------
            if (serialPort1.IsOpen)                         // if serial port is connected
            {
                serialPort1.Write(TxBytes, 0, 1);           // write TxBytes[0]
                serialPort1.Write(TxBytes, 1, 1);           // write TxBytes[1] 
            }
        }

        private void receiveMessage()       // this function waits for a data packet from the serial port 
        {
            // this function modifies receivedZPos and receivedXPos to indicate the current tick positions of z and x

            byte result;
            int receivedState = 0;              // indicate the state of receiving (which byte it ise receiving)

            if (serialPort1.IsOpen)
            {

                while (RxQueue.Count < 4) { };     // wait for queue to have more than 4 bytes

                while (RxQueue.Count <= 4 && RxQueue.Count > 0)         // remove results from queue  
                {

                    while (!RxQueue.TryDequeue(out result)) { };       // wait until new result is dequeued (x value)

                    // extract RxBytes
                    if (result == 255)
                    {
                        receivedState = 1;
                        RxBytes[0] = result;
                    }
                    else if (receivedState == 1)
                    {
                        RxBytes[1] = result;
                        receivedState++;
                    }
                    else if (receivedState == 2)
                    {
                        RxBytes[2] = result;
                        receivedState++;
                    }
                    else if (receivedState == 3)
                    {
                        RxBytes[3] = result;
                        receivedState = 0;

                        // process RxBytes - calculate receivedXPos
                        if ((int)(RxBytes[3] & 12) == 12)
                        {
                            receivedXPos = 2;
                        }
                        else if ((int)(RxBytes[3] & 12) == 4)
                        {
                            receivedXPos = 1;
                        }
                        else if ((int)(RxBytes[3] & 12) == 0)
                        {
                            receivedXPos = 0;
                        }

                        // process RxBytes - calculate receivedZPos
                        receivedZPos = 0;
                        if ((int)(RxBytes[3] & 1) == 1)            // check to see if RxBytes[2] is 255
                        {
                            receivedZPos = receivedZPos + 255;
                        }
                        else
                        {
                            receivedZPos = receivedZPos + (int)(RxBytes[2]);
                        }
                        if ((int)(RxBytes[3] & 2) == 2)            // check to see if RxBytes[1] is 255
                        {
                            receivedZPos = receivedZPos + 65281;
                        }
                        else
                        {
                            receivedZPos = receivedZPos + (int)(RxBytes[1] << 8);
                        }
                    }
                }
            }
        }

        private void write2File(string text)
        {

        }


        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.Close();
 //           fileStream.Close();
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int newByte;
            int bytesToRead = 0;                // determine are there bytes to read

            if (serialPort1.IsOpen)
            {
                bytesToRead = serialPort1.BytesToRead;          // number of bytes in received buffer
            }
            while (bytesToRead > 0 && serialPort1.IsOpen)
            {
                newByte = serialPort1.ReadByte();               // read input data
                bytesToRead = serialPort1.BytesToRead;
                RxQueue.Enqueue(Convert.ToByte(newByte));
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                capWebcam = new Capture(0);         // start camera
                capWebcam.SetCaptureProperty(CapProp.FrameHeight, 480);         // height resolution
                capWebcam.SetCaptureProperty(CapProp.FrameWidth, 640);          // width resolution
                capWebcam.SetCaptureProperty(CapProp.Fps, 30);                  // frame rate
            }
            catch (Exception ex)
            {
                MessageBox.Show("unable to read from webcam, error: " + Environment.NewLine + Environment.NewLine +
                                ex.Message + Environment.NewLine + Environment.NewLine +
                                "exiting program");
                Environment.Exit(0);
                return;
            }
  //          Application.Idle += captureCameraThread;       // add process image function to the application's list of tasks
        }

        void processSingleFrameAndUpdateGUI()
        {
            if (isRecording == true)
            {
                fileStream.Write(";");                      // write to data.txt indicating start of frame

                // write curent x position to data.txt
                if (receivedXPos == 0)
                {
                    fileStream.Write("0,");
                }
                else if (receivedXPos == 1)
                {
                    fileStream.Write("45,");
                }
                else
                {
                    fileStream.Write("90,");
                }

                fileStream.Write(convertZTick2Angle(receivedZPos).ToString());
            }

            Mat imgOriginal;                        // image variable (stores 1 frame)

            imgOriginal = capWebcam.QueryFrame();   // get 2nd frame
            imgOriginal = capWebcam.QueryFrame();

            //Mat imgFiltered = new Mat(imgOriginal.Size, DepthType.Cv8U, 1);     

            //imgFiltered.SetTo(new ScalarArray(new MCvScalar(0)));

            Image<Gray, Byte> filtered = new Image<Gray, byte>(640, 480);           // image which stores all red
            Image<Gray, Byte> pointCloud = new Image<Gray, byte>(640, 480);         // point cloud

            byte[,,] data = imgOriginal.ToImage<Bgr, Byte>().Data; 

            // stores all red pixels as white pixels---------------------------------------
            for (int i = 0; i < imgOriginal.Height; i++)
            {
                for (int j = 0; j < imgOriginal.Width; j++)
                {
                    if (data[i, j, 2] > 50)                                 // if red value is above 50, add to filtered data
                    {
                        filtered.Data[i, j, 0] = 255;
                    }
                }
            }

            // convert white pixels to point cloud ----------------------------
            for (int i = 0; i < filtered.Height; i++)
            {
                for (int j = 0; j < filtered.Width; j++)
                {
                    int k = 0;
                    while (filtered.Data[i, j, 0] == 255 && i < filtered.Height && i % 5 == 0)
                    {  // stay in this loop IF: there is a white pixel in image 1 
                       //AND if it's not run off the edge of the image 
                       //AND only if it's on every 5th row
                        k++;  // increase "k" by 1. This counts the number of white pixels in a consecutive row
                        if (j >= filtered.Width - 1) // if pixel is at the end of a row, restart at new row
                        {
                            j = 0;
                            i++;
                            break;
                        }
                        else
                        {
                            j++;
                        }
                    }
                    if (k > 0)    // if there is a white pixel in the previous row
                    {
                        int centredRowNumber = j - k / 2;           // middle of several pixels
                        int xCoord;
                        int yCoord;
                        if (centredRowNumber < 0)                   // fixes crash somehow
                        {
                            yCoord = i - 1;
                            xCoord = filtered.Width - 1 - k / 2;
                            pointCloud.Data[yCoord, xCoord, 0] = 255;
                        }
                        else
                        {
                            yCoord = i;
                            xCoord = j - (k / 2);
                            pointCloud.Data[yCoord, xCoord, 0] = 255;
                        }
                        if (isRecording == true)
                        {
                            fileStream.Write("," + xCoord.ToString() + "," + yCoord.ToString());
                        }
                    }
                }
            }


            ibOriginal.Image = imgOriginal;
            ibFiltered.Image = filtered.ToUMat();
            ibOriginal.Refresh();
            ibFiltered.Refresh();
        }

        private void btnFrameAdvance_Click(object sender, EventArgs e)
        {
            processSingleFrameAndUpdateGUI();
        }
        private void runProcessing()
        {
            using (Process proc = Process.Start(start))
            {
                proc.WaitForExit();

                // Retrieve the app's exit code
                exitCode = proc.ExitCode;
            }
        }
    }
}
