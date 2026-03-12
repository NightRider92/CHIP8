using CHIP8.Input;
using CHIP8.Memory;
using CHIP8.Timers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CHIP8.CPU
{
    /// <summary>
    /// Instruction handlers
    /// Original CHIP8 (1977) implementation of opcodes
    /// This code does not contain modern variants of opcodes
    /// Written by Moran Rod
    /// </summary>
    public class Handlers
    {
        private readonly Registers.Registers registers;
        private readonly Timers.Timers timers;

        private readonly MainMemory mainMemory;
        private readonly VideoMemory videoMemory;

        private readonly Keyboard keyboard;
        private readonly Random rnd;

        private bool isInWaitState = false;
        private int waitingRegister = 0;

        public Handlers(MainMemory mMem, VideoMemory vMem, Registers.Registers reg, Timers.Timers tmr, Keyboard kbd)
        {
            this.mainMemory = mMem;
            if (this.mainMemory == null) throw new ArgumentNullException(nameof(this.mainMemory)); 

            this.videoMemory = vMem;
            if (this.videoMemory == null) throw new ArgumentNullException(nameof(this.videoMemory));

            this.registers = reg;
            if (this.registers == null) throw new ArgumentNullException(nameof(this.registers));

            this.timers = tmr;
            if (this.timers == null) throw new ArgumentNullException(nameof(this.timers));

            this.keyboard = kbd;
            if (this.keyboard == null) throw new ArgumentNullException(nameof(this.keyboard));

            this.rnd = new Random();
        }

        // 0x0NNN group
        public void instruct_cls(ushort opcode)
        {
            this.videoMemory.Clear();
            this.registers.reg_PC += 2;
        }

        // Return from subroutine instruction
        public void instruct_ret(ushort opcode)
        {
            this.registers.reg_SP -= 1;
            this.registers.reg_PC = this.registers.stack[this.registers.reg_SP];
        }

        // NO-OP (ignore)
        public void instruct_sys(ushort opcode)
        {
            this.registers.reg_PC += 2;
        }

        // 1NNN
        public void instruct_jp_addr(ushort opcode)
        {
            int nnn = (opcode & 0x0FFF);
            registers.reg_PC = (ushort)nnn;
        }

        // 2NNN (calls 12 bit address - NNN)
        public void instruct_call_addr(ushort opcode)
        {
            int nnn = (opcode & 0x0FFF);
            this.registers.stack[this.registers.reg_SP] = (ushort)(this.registers.reg_PC + 2);
            this.registers.reg_SP++;
            this.registers.reg_PC = (ushort)nnn;
        }

        // 3XNN
        public void instruct_se_vx_nn(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int nn = (opcode & 0x00FF);

            if (this.registers.reg_V[x] == nn) this.registers.reg_PC += 4;
            else this.registers.reg_PC += 2;
        }

        // 4XNN
        public void instruct_sne_vx_nn(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int nn = (byte)(opcode & 0x00FF);

            if (this.registers.reg_V[x] != nn) this.registers.reg_PC += 4;
            else this.registers.reg_PC += 2;
        }

        // 5XY0
        public void instruct_se_vx_vy(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int y = (opcode & 0x00F0) >> 4;

            if (this.registers.reg_V[x] == this.registers.reg_V[y]) this.registers.reg_PC += 4;
            else this.registers.reg_PC += 2;
        }

        // 6XNN - Load byte (NN) into V register at position X
        public void instruct_ld_vx_nn(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;         // Second byte
            byte nn = (byte)(opcode & 0x00FF);      // Last 2 bytes

            this.registers.reg_V[x] = nn;           // Set vX to NN
            this.registers.reg_PC += 2;             // CHIP8 always increases PC by 2 if not specified differently
        }

        // 7XNN
        public void instruct_add_vx_nn(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int nn = (opcode & 0x00FF);
            this.registers.reg_V[x] += (byte)nn;
            this.registers.reg_PC += 2;
        }

        // 8XY0
        public void instruct_ld_vx_vy(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int y = (opcode & 0x00F0) >> 4;

            this.registers.reg_V[x] = this.registers.reg_V[y];
            this.registers.reg_PC += 2;
        }

        // 8XY1
        public void instruct_or_vx_vy(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int y = (opcode & 0x00F0) >> 4;

            this.registers.reg_V[x] |= this.registers.reg_V[y];
            this.registers.reg_PC += 2;
        }

        // 8XY2
        public void instruct_and_vx_vy(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int y = (opcode & 0x00F0) >> 4;

            this.registers.reg_V[x] &= this.registers.reg_V[y];
            this.registers.reg_PC += 2;
        }

        // 8XY3
        public void instruct_xor_vx_vy(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int y = (opcode & 0x00F0) >> 4;

            this.registers.reg_V[x] ^= this.registers.reg_V[y];
            this.registers.reg_PC += 2;
        }

        // 8XY4
        public void instruct_add_vx_vy(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int y = (opcode & 0x00F0) >> 4;
            int sum = this.registers.reg_V[x] + this.registers.reg_V[y];

            this.registers.reg_V[0xF] = (byte)(sum > 255 ? 1 : 0);
            this.registers.reg_V[x] = (byte)(sum & 255);
            this.registers.reg_PC += 2;
        }

        // 8XY5
        public void instruct_sub_vx_vy(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int y = (opcode & 0x00F0) >> 4;

            byte vx = this.registers.reg_V[x];
            byte vy = this.registers.reg_V[y];

            // VF = 1 if Vx > Vy (no borrow)
            this.registers.reg_V[0xF] = (byte)(vx > vy ? 1 : 0);

            // Vx = Vx - Vy (wrap to 8 bits)
            this.registers.reg_V[x] = (byte)(vx - vy);
            this.registers.reg_PC += 2;
        }

        // 8XY6
        public void instruct_shr_vx(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int y = (opcode & 0x00F0) >> 4;
            byte vy = this.registers.reg_V[y];

            // VF = least significant bit of Vx before shift
            this.registers.reg_V[0xF] = (byte)(vy & 0x01);

            // Shift Vx right by 1
            this.registers.reg_V[x] = (byte)(vy >> 1);
            this.registers.reg_PC += 2;
        }

        // 8XY7
        public void instruct_subn_vx_vy(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int y = (opcode & 0x00F0) >> 4;

            byte vx = this.registers.reg_V[x];
            byte vy = this.registers.reg_V[y];

            // VF = 1 if Vy > Vx (no borrow)
            this.registers.reg_V[0xF] = (byte)(vy > vx ? 1 : 0);

            // Vx = Vy - Vx (wrap to 8 bits)
            this.registers.reg_V[x] = (byte)(vy - vx);
            this.registers.reg_PC += 2;
        }

        // 8XYE
        public void instruct_shl_vx(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int y = (opcode & 0x00F0) >> 4;
            byte vy = this.registers.reg_V[y];

            // VF = most significant bit of Vx before shift
            this.registers.reg_V[0xF] = (byte)((vy >> 7) & 0x01);

            // Shift Vx left by 1
            this.registers.reg_V[x] = (byte)(vy << 1);
            this.registers.reg_PC += 2;
        }

        // 9XY0
        public void instruct_sne_vx_vy(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int y = (opcode & 0x00F0) >> 4;

            byte vx = this.registers.reg_V[x];
            byte vy = this.registers.reg_V[y];

            if (vx != vy) this.registers.reg_PC += 4;
            else this.registers.reg_PC += 2;
        }

        // ANNN = 12 bit memory address
        public void instruct_ld_i_addr(ushort opcode)
        {
            int nnn = (opcode & 0x0FFF); // NNN = memory address (12 bits)
            this.registers.reg_I = (ushort)nnn;
            this.registers.reg_PC += 2;
        }

        // BNNN
        public void instruct_jp_v0_addr(ushort opcode)
        {
            int nnn = (opcode & 0x0FFF);
            this.registers.reg_PC = (ushort)(nnn + this.registers.reg_V[0x0]);
        }

        // CXNN
        public void instruct_rnd_vx_nn(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int nn = (opcode & 0x00FF);

            byte rB = (byte)rnd.Next(0, 256);
            this.registers.reg_V[x] = (byte)(rB & nn);
            this.registers.reg_PC += 2;
        }

        // DXYN
        public void instruct_drw_vx_vy_n(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            int y = (opcode & 0x00F0) >> 4;
            int n = (opcode & 0x000F);
            int I = this.registers.reg_I;

            int vx = this.registers.reg_V[x];
            int vy = this.registers.reg_V[y];

            // Collision register
            this.registers.reg_V[0xF] = 0;

            for (int row = 0; row < n; row++)
            {
                // Position of sprite in main memory
                byte sb = (byte)this.mainMemory.GetValue((uint)(I + row));

                // Extract bits from byte (sprite), MSB first
                for (int bit = 0; bit < 8; bit++)
                {
                    int pixel = (byte)(sb >> (7 - bit)) & 1;
                    if (pixel == 0) continue;

                    int px = (vx + bit) % Constants.SCREEN_W;
                    int py = (vy + row) % Constants.SCREEN_H;

                    // Video memory index
                    int idx = py * Constants.SCREEN_W + px;

                    if ((byte)this.videoMemory.GetValue((uint)(idx)) == 1)
                        this.registers.reg_V[0xF] = 1; // Collision detected

                    // XOR write to video memory
                    byte oldPixel = (byte)this.videoMemory.GetValue((uint)idx);
                    byte newPixel = (byte)(oldPixel ^ 1);
                    this.videoMemory.SetValue((uint)idx, newPixel);
                }
            }
            this.registers.reg_PC += 2;
        }

        // EX??
        public void instruct_skp_vx(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            byte vX = this.registers.reg_V[x];

            bool key = this.keyboard.GetBufferValue(vX);
            if (key == true) this.registers.reg_PC += 4;
            else this.registers.reg_PC += 2;
        }

        //EXA1 — SKNP Vx
        public void instruct_sknp_vx(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            byte vX = this.registers.reg_V[x];

            bool key = this.keyboard.GetBufferValue(vX);
            if (key == false) this.registers.reg_PC += 4;
            else this.registers.reg_PC += 2;
        }

        // FX??
        public void instruct_ld_vx_dt(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            this.registers.reg_V[x] = this.timers.DT;
            this.registers.reg_PC += 2;
        }

        // FX0A - Trigger wait state and wait for key press to continue
        public void instruct_ld_vx_k(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;

            if (!isInWaitState)
            {
                this.isInWaitState = true;
                this.waitingRegister = x;
                return;
            }

            bool[] buffer = this.keyboard.GetBuffer();
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i])
                {
                    this.isInWaitState = false;
                    this.registers.reg_V[this.waitingRegister] = (byte)i;
                    this.registers.reg_PC += 2;
                    return;
                }
            }
        }

        // Set delay timer to V[x]
        public void instruct_ld_dt_vx(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            this.timers.DT = this.registers.reg_V[x];
            this.registers.reg_PC += 2;
        }

        // Set sound timer to V[x]
        public void instruct_ld_st_vx(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            this.timers.ST = this.registers.reg_V[x];
            this.registers.reg_PC += 2;
        }

        // FX1E
        public void instruct_add_i_vx(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            byte vX = this.registers.reg_V[x];
            this.registers.reg_I += vX;
            this.registers.reg_PC += 2;
        }

        // FX29 - Set I to the memory address of the sprite for digit Vx.
        public void instruct_ld_f_vx(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            byte digit = this.registers.reg_V[x];

            // * 5 because we have 16 digits, each 5 bytes long / 5 * 16 = 80
            this.registers.reg_I = (ushort)(Constants.FONTSET_ADDR + digit * 5);
            this.registers.reg_PC += 2;
        }

        // FX33 - Store BCD of Vx at memory[i],[i+1],[i+2] - BCD code
        public void instruct_ld_b_vx(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            byte vX = this.registers.reg_V[x];

            byte BCD_100 = 0x0;
            byte BCD_10 = 0x0;
            byte BCD_1 = 0x0;

            BCD_100 = (byte)(vX / 100);         // Hundreds
            BCD_10 = (byte)((vX / 10) % 10);    // Tens
            BCD_1 = (byte)(vX % 10);            // Ones

            ushort I = this.registers.reg_I;

            this.mainMemory.SetValue(I, BCD_100);
            this.mainMemory.SetValue((uint)(I + 1), BCD_10);
            this.mainMemory.SetValue((uint)(I + 2), BCD_1);
            this.registers.reg_PC += 2;
        }

        // FX55 - Store V0..Vx into memory starting at I.
        public void instruct_ld_i_vx(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            ushort I = this.registers.reg_I;

            for (int i = 0; i <= x; i++)
                this.mainMemory.SetValue((uint)(I + i), this.registers.reg_V[i]);

            // ORIGINAL CHIP‑8: I unchanged
            this.registers.reg_PC += 2;
        }

        // FX65 - Load V0..Vx from memory starting at I.
        public void instruct_ld_vx_i(ushort opcode)
        {
            int x = (opcode & 0x0F00) >> 8;
            ushort I = this.registers.reg_I;

            for (int i = 0; i <= x; i++)
                this.registers.reg_V[i] = (byte)this.mainMemory.GetValue((uint)(I + i));

            // ORIGINAL CHIP‑8: I unchanged
            this.registers.reg_PC += 2;
        }
    }
}
