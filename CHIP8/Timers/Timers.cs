using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP8.Timers
{
    /// <summary>
    /// Timers class containing display and sound timer
    /// </summary>
    public class Timers
    {
        public Timers() 
        {
            Console.WriteLine("Timers have been initialized"); 
        }

        // Timers
        public byte DT { get; set; } = 0x0;
        public byte ST { get; set; } = 0x0;
    }
}
