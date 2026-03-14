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
            return this.Buffer.ToArray();
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
            if (index >= this.Buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(index)); 
            return this.Buffer[index];
        }

        /// <summary>
        /// Set value at specific index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public virtual void SetValue(uint index, byte value)
        {
            if (index >= this.Buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            this.Buffer[index] = value;
        }
    }
}
