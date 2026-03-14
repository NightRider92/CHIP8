**CHIP‑8 Emulator (C# • Raylib‑cs)**

<img width="810" height="620" alt="image" src="https://github.com/user-attachments/assets/f7086828-f27e-43b1-8e2f-980a00473c5b" />

A clean, fully‑from‑scratch CHIP‑8 emulator written in C#, using raylib‑cs for graphics, input and window management.
The project is structured for readability and modularity, with separate components for CPU, memory, input, graphics, timers and ROM loading.

This emulator aims to stay faithful to the original 1977 COSMAC VIP interpretation of CHIP‑8 while adding a few modern touches—like a subtle pixel‑shading effect 
to give the display a more “phosphor‑glow” feel.

<img width="793" height="567" alt="image" src="https://github.com/user-attachments/assets/5f289bed-ae3b-454d-a78c-e5a3fd36b30a" />

**Full CHIP‑8 instruction set implementation**

- Modular architecture (CPU, Memory, Graphics, Input, Timers, ROM loader)
- Raylib‑cs rendering
- Configurable display scaling
- Custom shading effect for pixels
- Runs classic CHIP‑8 ROMs (Pong, Space Invaders, Tetris, etc.)

**About CHIP‑8 (1977)**
CHIP‑8 isn’t actually a hardware system—it’s an interpreted virtual machine created in the late 1970s for hobbyist computers like the COSMAC VIP (1977).
It was designed to make game programming accessible, with simple opcodes, a 64×32 monochrome display and a hex‑based keypad.

**Despite its age, CHIP‑8 remains a favorite for emulator developers because:**
- The architecture is tiny and elegant
- The instruction set is easy to implement
- ROMs are small and fun to test
- It’s a perfect introduction to emulation concepts

**Pixel Shading Effect**
- To give the display a more interesting look, the emulator draws each lit pixel twice:
- A dark green offset pixel (shadow)
- The main bright green pixel
- This creates a subtle depth effect reminiscent of old CRT glow.
