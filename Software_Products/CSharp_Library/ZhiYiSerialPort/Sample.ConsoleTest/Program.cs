using System;
using System.Collections.Generic;
using System.Text;

using ZhiYiSerialPort;

namespace Sample.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ClsZhiYiSerialPort.PortName = "COM1";
            ClsZhiYiSerialPort.BaudRate = 9600;
            ClsZhiYiSerialPort.DataBits = 8;
            ClsZhiYiSerialPort.StopBits = System.IO.Ports.StopBits.One;
            ClsZhiYiSerialPort.Parity = System.IO.Ports.Parity.None;
            ClsZhiYiSerialPort.GetByteEvent += ClsZhiYiSerialPort_GetByteEvent;
            if (!ClsZhiYiSerialPort.Open())
            {
                return;
            }

            Console.ReadKey();
        }

        static int recvBytesNum = 0;
        private static void ClsZhiYiSerialPort_GetByteEvent(byte b)
        {
            Console.Write(Convert.ToChar(b).ToString());
            recvBytesNum++;
            Console.Title = recvBytesNum.ToString();
        }
    }
}
