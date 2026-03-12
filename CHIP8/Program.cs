
using CHIP8;
using CHIP8.CPU;

Core system = new Core("outlaw.ch8");
Console.Clear();
while (true)
{
    system.Process();
    //Thread.Sleep(1); 
}