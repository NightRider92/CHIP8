
using CHIP8;
using CHIP8.CPU;
using System.Diagnostics;

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

double cpuAccumulator = 0;
double timersAccumulator = 0;  

Core system = new Core("outlaw.ch8");
Console.Clear();

while (true)
{
    double elapsed = stopwatch.Elapsed.TotalMilliseconds;
    stopwatch.Restart();

    cpuAccumulator += elapsed;
    timersAccumulator += elapsed;

    if (cpuAccumulator >= 1.0f) // ~ 1000Hz
    {
        system.Process();
        cpuAccumulator -= 1;
    }

    if (timersAccumulator >= 16.67f) // ~ 60Hz
    {
        system.ProcessTimers();
        timersAccumulator-= 16.67f;    
    }
}
