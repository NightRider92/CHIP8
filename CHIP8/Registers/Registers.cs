using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP8.Registers
{
    /// <summary>
    /// Register class containing GP registers and special registers
    /// </summary>
    public class Registers
    {
        public Registers()
        {
            Console.WriteLine("Registers have been initialized");
        }

        // General purpose registers
        public byte[] reg_V { get; set; } = new byte[16];

        // Special purpose registers
        public ushort reg_I { get; set; } = 0x0; 
        public ushort reg_PC { get; set; } = 0x0;
        public ushort reg_SP { get; set; } = 0x0;

        // Stack
        public ushort[] stack { get; set; } = new ushort[16];
    }
}
