using CHIP8.Input;
using CHIP8.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CHIP8.CPU
{
    /// <summary>
    /// Central Processing Unit
    /// </summary>
    public class CPU
    {
        // Instructions and handlers
        private readonly Keyboard? keyboard = null;

        private readonly Registers.Registers? registers = null;
        private readonly Timers.Timers? timers = null;

        private readonly MainMemory? mainMemory = null;
        private readonly VideoMemory? videoMemory = null;

        private readonly Handlers? handlers = null;
        private readonly List<Instruction> instructions;

        public CPU(MainMemory mMem, VideoMemory vMem, Registers.Registers registers, Timers.Timers timers, Keyboard keyboard)
        {
            this.mainMemory = mMem;
            if (this.mainMemory == null)
                throw new ArgumentNullException(nameof(this.mainMemory));

            this.videoMemory = vMem;
            if (this.videoMemory == null)
                throw new ArgumentNullException(nameof(this.videoMemory));

            this.registers = registers;
            if (this.registers == null)
                throw new ArgumentNullException(nameof(this.registers));

            this.timers = timers;
            if (this.timers == null)
                throw new ArgumentNullException(nameof(this.timers));

            this.keyboard = keyboard;
            if (this.keyboard == null)
                throw new ArgumentNullException(nameof(this.keyboard));

            // Initialize instruction handlers
            this.handlers = new Handlers(this.mainMemory, this.videoMemory, this.registers, this.timers, this.keyboard);

            // Initialize list of instructions
            this.instructions = new()
            {
                // 0x0NNN group
                new Instruction(0xFFFF, 0x00E0, handlers.instruct_cls),   // CLS
                new Instruction(0xFFFF, 0x00EE, handlers.instruct_ret),   // RET
                new Instruction(0xF000, 0x0000, handlers.instruct_sys),   // SYS (ignored)

                // 1NNN
                new Instruction(0xF000, 0x1000, handlers.instruct_jp_addr),

                // 2NNN
                new Instruction(0xF000, 0x2000, handlers.instruct_call_addr),

                // 3XNN
                new Instruction(0xF000, 0x3000, handlers.instruct_se_vx_nn),

                // 4XNN
                new Instruction(0xF000, 0x4000, handlers.instruct_sne_vx_nn),

                // 5XY0
                new Instruction(0xF00F, 0x5000, handlers.instruct_se_vx_vy),

                // 6XNN
                new Instruction(0xF000, 0x6000, handlers.instruct_ld_vx_nn),

                // 7XNN
                new Instruction(0xF000, 0x7000, handlers.instruct_add_vx_nn),

                // 8XY?
                new Instruction(0xF00F, 0x8000, handlers.instruct_ld_vx_vy),
                new Instruction(0xF00F, 0x8001, handlers.instruct_or_vx_vy),
                new Instruction(0xF00F, 0x8002, handlers.instruct_and_vx_vy),
                new Instruction(0xF00F, 0x8003, handlers.instruct_xor_vx_vy),
                new Instruction(0xF00F, 0x8004, handlers.instruct_add_vx_vy),
                new Instruction(0xF00F, 0x8005, handlers.instruct_sub_vx_vy),
                new Instruction(0xF00F, 0x8006, handlers.instruct_shr_vx),
                new Instruction(0xF00F, 0x8007, handlers.instruct_subn_vx_vy),
                new Instruction(0xF00F, 0x800E, handlers.instruct_shl_vx),

                 // 9XY0
                new Instruction(0xF00F, 0x9000, handlers.instruct_sne_vx_vy),

                 // ANNN
                new Instruction(0xF000, 0xA000, handlers.instruct_ld_i_addr),

                 // BNNN
                new Instruction(0xF000, 0xB000, handlers.instruct_jp_v0_addr),

                // CXNN
                new Instruction(0xF000, 0xC000, handlers.instruct_rnd_vx_nn),

                // DXYN
                new Instruction(0xF000, 0xD000, handlers.instruct_drw_vx_vy_n),

                // EX??
                new Instruction(0xF0FF, 0xE09E, handlers.instruct_skp_vx),
                new Instruction(0xF0FF, 0xE0A1, handlers.instruct_sknp_vx),

                // FX??
                new Instruction(0xF0FF, 0xF007, handlers.instruct_ld_vx_dt),
                new Instruction(0xF0FF, 0xF00A, handlers.instruct_ld_vx_k),
                new Instruction(0xF0FF, 0xF015, handlers.instruct_ld_dt_vx),
                new Instruction(0xF0FF, 0xF018, handlers.instruct_ld_st_vx),
                new Instruction(0xF0FF, 0xF01E, handlers.instruct_add_i_vx),
                new Instruction(0xF0FF, 0xF029, handlers.instruct_ld_f_vx),
                new Instruction(0xF0FF, 0xF033, handlers.instruct_ld_b_vx),
                new Instruction(0xF0FF, 0xF055, handlers.instruct_ld_i_vx),
                new Instruction(0xF0FF, 0xF065, handlers.instruct_ld_vx_i),
            };
            Console.WriteLine("CPU has been initialized");
        }

        /// <summary>
        /// Process operation codes
        /// </summary>
        /// <param name="opcode"></param>
        public void ProcessCPU()
        {
            int n = this.registers!.reg_PC;
            byte b = (byte)(this.mainMemory!.GetValue((uint)n))!;
            byte b1 = (byte)(this.mainMemory!.GetValue((uint)(n + 1)))!;

            ushort opcode = (ushort)((b << 8) | b1);
            foreach (var i in this.instructions)
            {
                if (i.Matches(opcode))
                {
                    i.Execute(opcode);
                    break;
                }
            }
        }

        /// <summary>
        /// Process timers
        /// </summary>
        public void ProcessTimers()
        {
           if (timers!.DT > 0) timers.DT--;
           if (timers!.ST > 0) timers.ST--;
        }
    }
}
