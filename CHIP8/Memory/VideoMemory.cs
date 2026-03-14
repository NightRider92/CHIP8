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
        protected override byte[] Buffer { get; set; } = new byte[Constants.SCREEN_W * Constants.SCREEN_H];

        /// <summary>
        /// Clear video memory
        /// </summary>
        public void Clear()
        {
            Array.Clear(this.Buffer, 0, this.Buffer.Length);
        }
    }
}
