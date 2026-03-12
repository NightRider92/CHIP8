using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP8.Memory
{
    /// <summary>
    /// Memory abstract class
    /// </summary>
    public abstract class MemoryBase
    {
        protected abstract byte[] Buffer { get; set; }

        /// <summary>
        /// Get memory array (as copy)
        /// </summary>
        /// <returns></returns>
        public virtual Array? GetClone()
        {
            return Buffer.ToArray();
        }

        /// <summary>
        /// Get value at specific index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// 
        public virtual object GetValue(uint index)
        {
            if (index >= Buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(index)); 
            return Buffer[index];
        }

        /// <summary>
        /// Set value at specific index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public virtual void SetValue(uint index, byte value)
        {
            if (index >= Buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Buffer[index] = value;
        }
    }
}
