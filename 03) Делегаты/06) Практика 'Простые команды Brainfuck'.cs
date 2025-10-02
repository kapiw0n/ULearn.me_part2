using System;
namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            RegisterBasicCommands(vm, read, write);
            RegisterASCIICommands(vm);
        }

        private static void RegisterBasicCommands(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            vm.RegisterCommand('.', machine => WriteCurrentMemory(machine, write));
            vm.RegisterCommand('+', machine => IncrementCurrentMemory(machine));
            vm.RegisterCommand('-', machine => DecrementCurrentMemory(machine));
            vm.RegisterCommand(',', machine => ReadToCurrentMemory(machine, read));
            vm.RegisterCommand('>', machine => MovePointerRight(machine));
            vm.RegisterCommand('<', machine => MovePointerLeft(machine));
        }

        private static void WriteCurrentMemory(IVirtualMachine machine, Action<char> write)
        {
            char outputChar = (char)machine.Memory[machine.MemoryPointer];
            write(outputChar);
        }

        private static void IncrementCurrentMemory(IVirtualMachine machine)
        {
            unchecked 
            {
                machine.Memory[machine.MemoryPointer]++;
            }
        }

        private static void DecrementCurrentMemory(IVirtualMachine machine)
        {
            unchecked 
            {
                machine.Memory[machine.MemoryPointer]--;
            }
        }

        private static void ReadToCurrentMemory(IVirtualMachine machine, Func<int> read)
        {
            int input = read();
            machine.Memory[machine.MemoryPointer] = (byte)input;
        }

        private static void MovePointerRight(IVirtualMachine machine)
        {
            if (machine.MemoryPointer == machine.Memory.Length - 1)
                machine.MemoryPointer = 0;
            else
                machine.MemoryPointer++;
        }

        private static void MovePointerLeft(IVirtualMachine machine)
        {
            if (machine.MemoryPointer == 0)
                machine.MemoryPointer = machine.Memory.Length - 1;
            else
                machine.MemoryPointer--;
        }

        private static void RegisterASCIICommands(IVirtualMachine vm)
        {
            for (char c = 'A'; c <= 'Z'; c++)
                RegisterASCIICommand(c, vm);

            for (char c = 'a'; c <= 'z'; c++)
                RegisterASCIICommand(c, vm);

            for (char c = '0'; c <= '9'; c++)
                RegisterASCIICommand(c, vm);
        }

        private static void RegisterASCIICommand(char c, IVirtualMachine vm)
        {
            byte asciiValue = (byte)c;
            vm.RegisterCommand(c, machine => 
            {
                machine.Memory[machine.MemoryPointer] = asciiValue;
            });
        }
    }
}