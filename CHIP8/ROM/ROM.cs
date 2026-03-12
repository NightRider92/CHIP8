using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP8.ROM
{
    /// <summary>
    /// ROM file loader
    /// </summary>
    public class ROM
    {
        public ROM()
        {
            Console.WriteLine("ROM loader has been initialized");
        }

        /// <summary>
        /// Load into the main memory
        /// </summary>
        /// <param name="filename"></param>
        /// <exception cref="ArgumentNullException"></exception> 
        /// <exception cref="FileNotFoundException"></exception>
        public byte[]? Load(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");

            if (!File.Exists(filename))
                throw new FileNotFoundException("filename");

            try { return File.ReadAllBytes(filename); }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to load ROM file: {ex.Message.ToString()}");
                return null;
            }
        }
    }
}
