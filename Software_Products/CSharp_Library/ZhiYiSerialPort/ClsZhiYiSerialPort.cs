using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;

namespace ZhiYiCSharpLibs
{
    public class ZhiYiSerialPort
    {
        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }
        public Parity Parity { get; set; }

        private SerialPort serial = new SerialPort();
        public List<byte> rxBuffer = new List<byte>();
        //private Thread thread = null;

        //public List<byte> ReceiveBuffer = new List<byte>();

        //private void CopyToBuffer()
        //{
        //    while(this.serial.IsOpen)
        //    {
        //        lock(rxBuffer)
        //        {
        //            if(rxBuffer.Count > 0)
        //            {
        //                Debug.WriteLine(string.Format("{0:X2} ", rxBuffer[0]));
        //                ReceiveBuffer.Add(rxBuffer[0]);
        //                rxBuffer.RemoveAt(0);
        //            }
        //        }
        //        Thread.Sleep(1);
        //    }
        //}

        public string[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        public string[] GetAvailableStopBits()
        {
            List<string> stopBits = new List<string>();

            stopBits.Add(System.IO.Ports.StopBits.None.ToString());
            stopBits.Add(System.IO.Ports.StopBits.One.ToString());
            stopBits.Add(System.IO.Ports.StopBits.Two.ToString());
            stopBits.Add(System.IO.Ports.StopBits.OnePointFive.ToString());

            return stopBits.ToArray();
        }

        public string[] GetAvailableParity()
        {
            List<string> parity = new List<string>();

            parity.Add(System.IO.Ports.Parity.None.ToString());
            parity.Add(System.IO.Ports.Parity.Odd.ToString());
            parity.Add(System.IO.Ports.Parity.Even.ToString());
            parity.Add(System.IO.Ports.Parity.Mark.ToString());
            parity.Add(System.IO.Ports.Parity.Space.ToString());

            return parity.ToArray();
        }

        public bool Open()
        {
            try
            {
                if (serial.IsOpen) serial.Close();

                serial.PortName = this.PortName;
                serial.BaudRate = this.BaudRate;
                serial.StopBits = this.StopBits;
                serial.DataBits = this.DataBits;
                serial.Parity = this.Parity;
                
                serial.Open();
                serial.DiscardInBuffer();
                serial.DiscardOutBuffer();

                //ReceiveBuffer.Clear();
                rxBuffer.Clear();
                serial.DataReceived += Serial_DataReceived;

                //if (thread == null)
                //{
                //    thread = new Thread(new ThreadStart(CopyToBuffer));
                //    thread.Start();
                //}
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool IsOpen()
        {
            return serial.IsOpen;
        }

        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
                int bytesToRead = serial.BytesToRead;
                byte[] b = new byte[bytesToRead];
                serial.Read(b, 0, b.Length);

                lock(rxBuffer)
                {
                    rxBuffer.AddRange(b);
                }
        }

        public void Close()
        {
            try
            {
                serial.Close();

                //thread.Abort();
                //thread = null;
            }
            catch(Exception)
            {

            }
        }
    }
}
