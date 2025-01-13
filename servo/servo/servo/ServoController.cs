/*
 * Servo NETMF Driver
 *	Coded by Chris Seto August 2010
 *	<chris@chrisseto.com> 
 *	
 * Use this code for whatever you want. Modify it, redistribute it, I don't care.
 * I do ask that you please keep this header intact, however.
 * If you modify the driver, please include your contribution below:
 * 
 * Chris Seto: Initial release (1.0)
 * Chris Seto: Netduino port (1.0 -> Netduino branch)
 * Chris Seto: bool pin state fix (1.1 -> Netduino branch)
 * 
 * Theo Browning: Release (2.0) modified to work with current framework.
 * 
 * */

using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;


namespace Netduino2PlusServo
{
    public class ServoController : IDisposable
    {
        /// <summary>
        /// PWM handle
        /// </summary>
        private PWM servo;

        /// <summary>
        /// Timings range
        /// </summary>      
        private int[] range = new int[2];

        /// <summary>
        /// Set servo inversion
        /// </summary>    
        public bool inverted = false;

        /// <summary>
        /// Create the PWM Channel, set it low and configure timings
        /// Changes here PWM Channel requires Channel, Period, Duration, 
        /// Scale and Inversion on instantiation.
        /// </summary>
        /// <param name="pwmChannel"></param>
        public ServoController()
        {
            // Initialize the PWM Channel, set to pin 5 as default.            
            servo = new PWM(
                PWMChannels.PWM_PIN_D5,
                20000,
                1500,
                Microsoft.SPOT.Hardware.PWM.ScaleFactor.Microseconds,
                false);

            // Full range for FS90 servo is 0 - 3000.
            // For safety limits I set the default just above/below that.
            range[0] = 600;
            range[1] = 2400;
        }

        /// <summary>
        /// Allow for consumer to set own range.
        /// </summary>
        /// <param name="leftStop", "rightStop"></param>
        public void SetRange(int leftStop, int rightStop)
        {
            range[1] = leftStop;
            range[0] = rightStop;
        }

        /// <summary>
        /// Dispose implementation.
        /// </summary>
        public void Dispose()
        {
            Disengage();
            servo.Dispose();
        }

        /// <summary>
        /// Disengage the servo. 
        /// The servo motor will stop, and try to maintain an angle
        /// </summary>
        public void Disengage()
        {
            servo.DutyCycle = 0; //SetDutyCycle(0);
        }


        /// <summary>
        /// Set the servo degree
        /// </summary>
        public double Degree
        {
            set
            {
                /// Range checks
                if (value > 180)
                    value = 180;

                if (value < 0)
                    value = 0;

                // Are we inverted?
                if (inverted)
                    value = 180 - value;

                // Set duration "pulse" and start the servo. 
                // Changes here are PWM.Duration and PWM.Start() instead of PWM.SetPulse().                
                servo.Duration = (uint)map((long)value, 0, 180, range[0], range[1]);
                servo.Start();
            }
        }

        /// <summary>
        /// Used internally to map a value of one scale to another
        /// </summary>
        /// <param name="x"></param>
        /// <param name="in_min"></param>
        /// <param name="in_max"></param>
        /// <param name="out_min"></param>
        /// <param name="out_max"></param>
        /// <returns></returns>

        private long map(long x, long in_min, long in_max, long out_min, long out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
    }
}