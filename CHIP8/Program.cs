
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

    if (cpuAccumulator >= Constants.CPU_TIMING_MS)      // ~ 1000Hz
    {
        system.Process();
        cpuAccumulator -= Constants.CPU_TIMING_MS;
    }

    if (timersAccumulator >= Constants.TIMER_TIMING_MS) // ~ 60Hz
    {
        system.ProcessTimers();
        timersAccumulator-= Constants.TIMER_TIMING_MS;    
    }
}
