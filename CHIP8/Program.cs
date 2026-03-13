
using CHIP8;
using CHIP8.CPU;
using System.Diagnostics;

Stopwatch stopwatchCPU = new Stopwatch();
Stopwatch stopwatchTimers = new Stopwatch();

stopwatchCPU.Start();
stopwatchTimers.Start();    

Core system = new Core("outlaw.ch8");
Console.Clear();

while (true)
{
    if(stopwatchCPU.Elapsed.TotalMilliseconds >= 1) // ~ 1000Hz
    {
        system.Process();
        stopwatchCPU.Restart();
    }

    if (stopwatchTimers.Elapsed.TotalMilliseconds >= 16.67f) // ~ 60Hz
    {
        system.ProcessTimers();
        stopwatchTimers.Restart();
    }
}
