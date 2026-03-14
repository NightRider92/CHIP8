using CHIP8.CPU;
using CHIP8.Graphics;
using CHIP8.Input;
using CHIP8.Memory;
using CHIP8.Registers;
using CHIP8.ROM;
using CHIP8.Timers;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP8
{
    /// <summary>
    /// Main class (system core)
    /// </summary>
    public class Core
    {
        private string romFile = string.Empty;

        private readonly ROM.ROM? rom = null;
        private readonly Keyboard? keyboard = null;

        private readonly Registers.Registers? registers = null;
        private readonly Timers.Timers? timers = null;

        private readonly MainMemory? mainMemory = null;
        private readonly VideoMemory? videoMemory = null;

        private readonly CPU.CPU? cpu = null;
        private readonly Display? display = null;

        public Core(string ROMFile)
        {
            this.romFile = ROMFile;
            if (string.IsNullOrEmpty(this.romFile))
                throw new ArgumentNullException(nameof(this.romFile));

            this.rom = new ROM.ROM();
            this.keyboard = new Keyboard();
            this.registers = new Registers.Registers();
            this.timers = new Timers.Timers();
            this.mainMemory = new MainMemory();
            this.videoMemory = new VideoMemory();

            // Set program counter to start address
            this.registers.reg_PC = (ushort)Constants.START_ADDR;

            // Load fontset to main memory
            for (int i = 0; i < Constants.fontset.Length; i++)
            {
                uint index = (uint)(Constants.FONTSET_ADDR + i);
                byte value = Constants.fontset[i];

                // Write into the main memory
                this.mainMemory.SetValue(index, value);
            }

            byte[]? romfile = rom.Load(this.romFile);
            if (romfile == null) throw new FileLoadException();

            // Load ROM to main memory
            for (int i = 0; i < romfile.Length; i++)
            {
                uint index = (uint)(Constants.START_ADDR + i);
                byte value = romfile[i];

                // Write into the main memory
                mainMemory.SetValue(index, value);
            }

            // Initialize CPU
            this.cpu = new CPU.CPU(this.mainMemory, this.videoMemory, this.registers, this.timers, this.keyboard);

            // Initialize display
            this.display = new Display();
            Console.WriteLine("System has been initialized");
        }

        /// <summary>
        /// Capture keyboard keys
        /// </summary>
        private void captureKeyboardKeys()
        {
            this.keyboard!.SetBufferValue(0x1, Raylib.IsKeyDown(KeyboardKey.One));
            this.keyboard!.SetBufferValue(0x2, Raylib.IsKeyDown(KeyboardKey.Two));
            this.keyboard!.SetBufferValue(0x3, Raylib.IsKeyDown(KeyboardKey.Three));
            this.keyboard!.SetBufferValue(0xC, Raylib.IsKeyDown(KeyboardKey.Four));

            this.keyboard!.SetBufferValue(0x4, Raylib.IsKeyDown(KeyboardKey.Q));
            this.keyboard!.SetBufferValue(0x5, Raylib.IsKeyDown(KeyboardKey.W));
            this.keyboard!.SetBufferValue(0x6, Raylib.IsKeyDown(KeyboardKey.E));
            this.keyboard!.SetBufferValue(0xD, Raylib.IsKeyDown(KeyboardKey.R));

            this.keyboard!.SetBufferValue(0x7, Raylib.IsKeyDown(KeyboardKey.A));
            this.keyboard!.SetBufferValue(0x8, Raylib.IsKeyDown(KeyboardKey.S));
            this.keyboard!.SetBufferValue(0x9, Raylib.IsKeyDown(KeyboardKey.D));
            this.keyboard!.SetBufferValue(0xE, Raylib.IsKeyDown(KeyboardKey.F));

            this.keyboard!.SetBufferValue(0xA, Raylib.IsKeyDown(KeyboardKey.Z));
            this.keyboard!.SetBufferValue(0x0, Raylib.IsKeyDown(KeyboardKey.X));
            this.keyboard!.SetBufferValue(0xB, Raylib.IsKeyDown(KeyboardKey.C));
            this.keyboard!.SetBufferValue(0xF, Raylib.IsKeyDown(KeyboardKey.V));
        }

        /// <summary>
        /// Process
        /// </summary>
        public void ProcessCPU()
        {
            Console.SetCursorPosition(0, 0);

            Console.WriteLine("REG_PC: " + this.registers!.reg_PC);
            Console.WriteLine("REG_SP: " + this.registers!.reg_SP);
            Console.WriteLine("REG_I: " + this.registers!.reg_I);
            Console.WriteLine("REG_V: " + string.Join(", ", this.registers!.reg_V));
            Console.WriteLine("STACK: " + string.Join(", ", this.registers!.stack));

            this.cpu!.ProcessCPU();
            this.captureKeyboardKeys();
        }

        /// <summary>
        /// Process timers
        /// </summary>
        public void ProcessTimers()
        {
            this.cpu!.ProcessTimers();
        }

        /// <summary>
        /// Process display
        /// </summary>
        public void ProcessDisplay()
        {
            byte[] buffer = (byte[])this.videoMemory!.GetClone()!;
            this.display!.Draw(buffer);
        }
    }
}