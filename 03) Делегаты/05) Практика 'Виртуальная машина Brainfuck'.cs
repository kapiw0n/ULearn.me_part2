using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class VirtualMachine : IVirtualMachine
    {
        public VirtualMachine(string program, int memorySize)
        {
            Instructions = program;
            Memory = new byte[memorySize];
            InstructionPointer = 0;
            MemoryPointer = 0;
            InstructionSet = new Dictionary<char, Action<IVirtualMachine>>();
        }

        public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
        {
            InstructionSet[symbol] = execute;
        }

        protected Dictionary<char, Action<IVirtualMachine>> InstructionSet { get; private set; }
        public string Instructions { get; }
        
        public int InstructionPointer { get; set; }
        public byte[] Memory { get; }
        public int MemoryPointer { get; set; }

        public void Run()
        {
            while (InstructionPointer < Instructions.Length)
            {
                char currentInstruction = Instructions[InstructionPointer];
                if (InstructionSet.TryGetValue(currentInstruction, out var action))
                {
                    action(this);
                }
                InstructionPointer++;
            }
        }
    }
}