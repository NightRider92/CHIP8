using CHIP8.Registers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP8.CPU
{
    /// <summary>
    /// Instruction description
    /// </summary>
    public class Instruction
    {
        public ushort Mask { get; } = 0x0;
        public ushort Pattern { get; } = 0x0;
        public Action<ushort> Handler { get; }

        public Instruction(ushort mask, ushort pattern, Action<ushort> handler)
        {
            this.Mask = mask;
            this.Pattern = pattern;
            this.Handler = handler;
        }

        /// <summary>
        /// Match specific instruction
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        public bool Matches(ushort opcode)
        {
            return (opcode & Mask) == Pattern;
        }

        /// <summary>
        /// Execute instruction
        /// </summary>
        public void Execute(ushort opcode)
        {
            Console.WriteLine($"OPCODE: 0x{opcode:X4}");
            this.Handler.Invoke(opcode);
        }
    }
}