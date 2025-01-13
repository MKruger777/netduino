using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace Netduino2PlusServo
{
    public class Program
    {

        public static void Main()
        {
            ServoController servo = new ServoController();

            servo.SetRange(500, 2400);

            bool runForever = true;

            while (runForever)
            {
                for (int i = 1; i < 179; i++)
                {
                    servo.Degree = i;
                    Thread.Sleep(25);
                }

                for (int i = 179; i > 1; i--)
                {
                    servo.Degree = i;
                    Thread.Sleep(25);
                }
            }
            servo.Dispose();
        }
    }
}