using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP8.Input
{
    /// <summary>
    /// Keyboard input class
    /// </summary>
    public class Keyboard
    {
        private bool[] buffer = new bool[Constants.KBD_SIZE_BYTES];
        public Keyboard()
        {
            Console.WriteLine("Keyboard has been initialized");
        }

        /// <summary>
        /// Get buffer array
        /// </summary>
        /// <returns></returns>
        public bool[] GetBuffer()
        {
            return this.buffer;
        }

        /// <summary>
        /// Get buffer value at specific index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GetBufferValue(int index)
        {
            if (index < 0 || index >= buffer.Length)
                throw new ArgumentOutOfRangeException();
            return this.buffer[index];
        }

        /// <summary>
        /// Set buffer value at specific index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SetBufferValue(int index, bool value)
        {
            if (index < 0 || index >= buffer.Length)
                throw new ArgumentOutOfRangeException();
            this.buffer[index] = value;
        }
    }
}
