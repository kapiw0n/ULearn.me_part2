using System.Collections.Generic;
namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static void RegisterTo(IVirtualMachine vm)
        {
            var loopMap = new Dictionary<int, int>();
            var loopStack = new Stack<int>();

            vm.RegisterCommand('[', machine => HandleLoopStart(machine, loopMap, loopStack));
            vm.RegisterCommand(']', machine => HandleLoopEnd(machine, loopStack));
        }

        private static void HandleLoopStart(IVirtualMachine machine, Dictionary<int, int> loopMap, Stack<int> loopStack)
        {
            if (loopMap.Count == 0)
            {
                BuildLoopMap(machine, loopMap);
            }

            if (machine.Memory[machine.MemoryPointer] == 0)
            {
                machine.InstructionPointer = loopMap[machine.InstructionPointer];
            }
            else
            {
                loopStack.Push(machine.InstructionPointer);
            }
        }

        private static void BuildLoopMap(IVirtualMachine machine, Dictionary<int, int> loopMap)
        {
            var loopStartStack = new Stack<int>();
            loopStartStack.Push(machine.InstructionPointer);

            for (int index = machine.InstructionPointer + 1; index < machine.Instructions.Length; index++)
            {
                if (machine.Instructions[index] == '[')
                {
                    loopStartStack.Push(index);
                }
                else if (machine.Instructions[index] == ']')
                {
                    loopMap[loopStartStack.Pop()] = index;
                }
            }
        }

        private static void HandleLoopEnd(IVirtualMachine machine, Stack<int> loopStack)
        {
            if (machine.Memory[machine.MemoryPointer] != 0)
            {
                machine.InstructionPointer = loopStack.Peek();
            }
            else
            {
                loopStack.Pop();
            }
        }
    }
}