using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CHIP8.Graphics
{
    /// <summary>
    /// Display (reads video-memory and displays pixels on screen)
    /// </summary>
    public class Display
    {
        public Display()
        {
            Console.WriteLine("Display has been initialized");
            Raylib.InitWindow(Constants.SCREEN_W * Constants.SCREEN_SCALE, Constants.SCREEN_H * Constants.SCREEN_SCALE, "CHIP8");
            Raylib.SetTargetFPS(Constants.GRAPHICS_FPS);
        }

        /// <summary>
        /// Draw pixels on screen
        /// </summary>
        /// <param name="videoBuffer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Draw(byte[] videoBuffer)
        {
            if (videoBuffer == null)
                throw new ArgumentNullException(nameof(videoBuffer));

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Pixel pixel = new Pixel();
            int shaderOffsetX = 2;

            for (int y = 0; y < Constants.SCREEN_H; y++)
            {
                for (int x = 0; x < Constants.SCREEN_W; x++)
                {
                    pixel.Value = videoBuffer[y * Constants.SCREEN_W + x];
                    pixel.Index = y * Constants.SCREEN_W + x;

                    if (pixel.Value == 1)
                    {
                        // Pixel shader
                        Raylib.DrawRectangle(
                             (x * Constants.SCREEN_SCALE) + shaderOffsetX,
                             (y * Constants.SCREEN_SCALE) + shaderOffsetX,
                             Constants.SCREEN_SCALE,
                             Constants.SCREEN_SCALE,
                             Color.DarkGreen);

                        // Draw pixel
                        Raylib.DrawRectangle(
                             x * Constants.SCREEN_SCALE,
                             y * Constants.SCREEN_SCALE,
                             Constants.SCREEN_SCALE,
                             Constants.SCREEN_SCALE,
                             Color.Green
                        );
                    }
                }
            }
            Raylib.EndDrawing();
        }
    }
}