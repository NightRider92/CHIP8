using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CHIP8.Memory
{
    /// <summary>
    /// Video memory class
    /// </summary>
    public class VideoMemory : MemoryBase
    {
        public VideoMemory()
        {
            Console.WriteLine("Video memory has been initialized");
        }
        protected override byte[] Buffer { get; set; } = new byte[64 * 32]; 

        /// <summary>
        /// Clear video memory
        /// </summary>
        public void Clear()
        {
            Array.Clear(Buffer, 0, Buffer.Length);
        }
    }
}
