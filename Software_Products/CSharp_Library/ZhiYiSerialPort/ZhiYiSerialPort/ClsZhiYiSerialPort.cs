using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using ZhiYiMessageBox;

namespace ZhiYiSerialPort
{
    public class ClsZhiYiSerialPort
    {
        public delegate void GetByteHandler(byte b);
        public static event GetByteHandler GetByteEvent;

        private static SerialPort serial = new SerialPort();
        private static List<byte> buffer = new List<byte>();
        private static Thread thread = null;
        private static bool threadExit = false;

        public static string PortName
        {
            get { return serial.PortName; }
            set { serial.PortName = value; }
        }

        public static int BaudRate
        {
            get { return serial.BaudRate; }
            set { serial.BaudRate = value; }
        }

        public static int DataBits
        {
            get { return serial.DataBits; }
            set { serial.DataBits = value; }
        }

        public static StopBits StopBits
        {
            get { return serial.StopBits; }
            set { serial.StopBits = value; }
        }

        public static Parity Parity
        {
            get { return serial.Parity; }
            set { serial.Parity = value; }
        }

        private static void ProcessData()
        {
            byte b = 0;
            bool getData = false;
            while (!threadExit)
            {
                getData = false;
                lock (buffer)
                {
                    if (buffer.Count > 0)
                    {
                        b = buffer[0];
                        buffer.RemoveAt(0);
                        getData = true;
                    }
                }

                if (getData)
                {
                    GetByteEvent.Invoke(b);
                }
            }
        }

        public static bool Open()
        {
            try
            {
                if (serial.IsOpen) serial.Close();
                serial.Open();


                if(thread != null)
                {
                    threadExit = true;
                    thread.Join();
                    thread.Abort();
                    thread = null;
                }
                thread = new Thread(new ThreadStart(ProcessData));
                thread.IsBackground = true;
                thread.Start();

                serial.DtrEnable = true;
                serial.ErrorReceived += Serial_ErrorReceived;
                serial.DataReceived += Serial_DataReceived;

                return true;
            }
            catch(Exception e)
            {
                ClsMessageBox.Exception(e.Message);
            }

            return false;
        }

        private static void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] tempData = new byte[serial.BytesToRead];
            serial.Read(tempData, 0, tempData.Length);
            lock (buffer)
            {
                buffer.AddRange(tempData);
            }
        }

        private static void Serial_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            ClsMessageBox.Error(e.ToString());
        }

        public static void Close()
        {
            try
            {
                if(thread != null)
                {
                    threadExit = true;
                    thread.Join();
                    thread.Abort();
                    thread = null;
                }
                serial.Close();
            }
            catch(Exception e)
            {
                ClsMessageBox.Exception(e.Message);
            }
        }
    }
}
