using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CHIP8.Memory
{
    /// <summary>
    /// Main memory class
    /// </summary>
    public class MainMemory : MemoryBase
    {
        public MainMemory() 
        {
            Console.WriteLine("Main memory has been initialized"); 
        }
        protected override byte[] Buffer { get; set; } = new byte[4096];
    }
}
