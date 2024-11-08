using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using zkemkeeper;


namespace BiometricTest1
{
    public partial class Form1 : Form
    {
        // Declare an instance of the CZKEM class
        public zkemkeeper.CZKEM zkem = new zkemkeeper.CZKEM();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Any initialization logic can go here
        }

        // Event handler for the 'Connect' button click
        private void btnConnect_Click(object sender, EventArgs e)
        {
            int deviceIndex = 0; // Device index (usually 1 for a single device)
            string deviceIp = "192.168.15.101"; // IP address of the device
            int devicePort = 4370; // Port (default 4370)

            // Clear previous device info in the TextBox before displaying new information
            deviceInfo.Clear();
            deviceInfo.AppendText("Connecting...\r\n");
            deviceInfo.AppendText("IP Address : " + deviceIp + "\r\n");
            deviceInfo.AppendText("Port            : " + devicePort + "\r\n");
            deviceInfo.Refresh(); // Force refresh after updating the text

            // Try to connect to the device
            if (zkem.Connect_Net(deviceIp, devicePort))
            {
                if (zkem.RegEvent(1, 65535))
                {
                    deviceInfo.Clear();
                    deviceInfo.AppendText("Connected to device!\r\n");
                    deviceInfo.AppendText(GetDeviceInfo(deviceIndex));
                }
            }
            else
            {
                int errorCode = 0;
                zkem.GetLastError(ref errorCode);  // Get the last error code

                deviceInfo.Clear();
                deviceInfo.AppendText("Failed to connect to the device.\r\n");
                deviceInfo.AppendText("Error Code: " + errorCode.ToString() + "\r\n");
            }
            
        }

        // Method to get device information
        private string GetDeviceInfo(int deviceIndex)
        {
            StringBuilder deviceInfo = new StringBuilder();

            // Example to retrieve some basic device information
            int ret = 0;
            string deviceSerial = "";

            // Get the device serial number (you can replace this with other info if needed)
            if (zkem.GetDeviceInfo(deviceIndex, 1, ref ret)) // 1: Device Serial Number
            {
                deviceSerial = ret.ToString();
            }

            // You can extend this to get other details like model number, firmware version, etc.
            deviceInfo.AppendLine("Device Serial: " + deviceSerial);

            // For example: to get the model number (model ID is 3 in GetDeviceInfo)
            string model = "";
            if (zkem.GetDeviceInfo(deviceIndex, 3, ref ret)) // 3: Model Number
            {
                model = ret.ToString();
            }
            deviceInfo.AppendLine("Model: " + model);

            // Retrieve other info based on what the device supports
            // For example, Firmware version (ID = 2 in GetDeviceInfo)
            string firmwareVersion = "";
            if (zkem.GetDeviceInfo(deviceIndex, 2, ref ret)) // 2: Firmware Version
            {
                firmwareVersion = ret.ToString();
            }
            deviceInfo.AppendLine("Firmware Version: " + firmwareVersion);

            // Example: Device IP (ID = 9 in GetDeviceInfo)
            string deviceIp = "";
            if (zkem.GetDeviceInfo(deviceIndex, 9, ref ret)) // 9: Device IP
            {
                deviceIp = ret.ToString();
            }
            deviceInfo.AppendLine("Device IP: " + deviceIp);

            // Return all the gathered device info as a string
            return deviceInfo.ToString();
        }

    }
}