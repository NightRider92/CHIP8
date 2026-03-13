
using CHIP8;
using CHIP8.CPU;
using System.Diagnostics;

Stopwatch stopwatch = new Stopwatch();

double cpuAccumulator = 0;
double timersAccumulator = 0;
double displayAccumulator = 0;

Core system = new Core("outlaw.ch8");
Console.Clear();

while (true)
{
    double elapsed = stopwatch.Elapsed.TotalMilliseconds;
    stopwatch.Restart();

    cpuAccumulator += elapsed;
    timersAccumulator += elapsed;
    displayAccumulator+= elapsed;   

    while (cpuAccumulator >= Constants.CPU_TIMING_MS)     
    {
        system.Process();
        cpuAccumulator -= Constants.CPU_TIMING_MS;
    }

    while (timersAccumulator >= Constants.TIMER_TIMING_MS) 
    {
        system.ProcessTimers();
        timersAccumulator -= Constants.TIMER_TIMING_MS;
    }

    while (displayAccumulator >= Constants.DISPLAY_TIMING_MS)
    {
        system.ProcessDisplay();
        displayAccumulator -= Constants.DISPLAY_TIMING_MS;
    }

    Thread.Sleep(1); // Yield to other threads (e.g. input handling)
}
