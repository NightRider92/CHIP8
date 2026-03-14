using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP8.Graphics
{
    /// <summary>
    /// Pixel structure
    /// </summary>
    public struct Pixel
    {
        public byte Value { get; set; }
        public int Index { get; set; }
    }
}
