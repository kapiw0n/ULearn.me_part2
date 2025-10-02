using System.Collections.Generic;

namespace Clones
{
    public class CloneVersionSystem : ICloneVersionSystem
    {
        private readonly List<Clone> clones;

        public CloneVersionSystem()
        {
            clones = new List<Clone> { new Clone() };
        }

		public string Execute(string query)
		{
			var parts = query.Split(' ');
			int cloneIndex = int.Parse(parts[1]) - 1;

			return parts[0] switch
			{
				"check" => HandleCheck(cloneIndex),
				"learn" => HandleLearn(cloneIndex, int.Parse(parts[2])),
				"rollback" => HandleRollback(cloneIndex),
				"relearn" => HandleRelearn(cloneIndex),
				"clone" => HandleClone(cloneIndex),
				_ => null
			};
		}

		private string HandleCheck(int cloneIndex)
		{
			return clones[cloneIndex].Check();
		}

		private string HandleLearn(int cloneIndex, int programId)
		{
			clones[cloneIndex].Learn(programId);
			return null;
		}

		private string HandleRollback(int cloneIndex)
		{
			clones[cloneIndex].RollBack();
			return null;
		}

		private string HandleRelearn(int cloneIndex)
		{
			clones[cloneIndex].Relearn();
			return null;
		}

		private string HandleClone(int cloneIndex)
		{
			Clone newClone = new Clone(clones[cloneIndex]);
			clones.Add(newClone);
			return null;
		}
	}

    public class Clone
    {
        private readonly Stack learnedPrograms;
        private readonly Stack rollbackHistory;

        public Clone()
        {
            learnedPrograms = new Stack();
            rollbackHistory = new Stack();
        }

        public Clone(Clone existingClone)
        {
            learnedPrograms = new Stack(existingClone.learnedPrograms);
            rollbackHistory = new Stack(existingClone.rollbackHistory);
        }

        public void Learn(int programId)
        {
            rollbackHistory.Clear();
            learnedPrograms.Push(programId);
        }

        public void RollBack()
        {
            if (!learnedPrograms.IsEmpty())
            {
                rollbackHistory.Push(learnedPrograms.Pop());
            }
        }

        public void Relearn()
        {
            if (!rollbackHistory.IsEmpty())
            {
                learnedPrograms.Push(rollbackHistory.Pop());
            }
        }

        public string Check()
        {
            return learnedPrograms.IsEmpty() ? "basic" : learnedPrograms.Peek().ToString();
        }
    }

    public class Stack
    {
        private StackItem top;

        public Stack() { }

        public Stack(Stack existingStack)
        {
            top = existingStack.top;
        }

        public void Push(int value)
        {
            top = new StackItem(value, top);
        }

        public int Peek()
        {
            return top != null ? top.Value : 0;
        }

        public int Pop()
        {
            if (top == null) return 0;
            int value = top.Value;
            top = top.Previous;
            return value;
        }

        public bool IsEmpty()
        {
            return top == null;
        }

        public void Clear()
        {
            top = null;
        }
    }

    public class StackItem
    {
        public readonly int Value;
        public readonly StackItem Previous;

        public StackItem(int value, StackItem previous)
        {
            Value = value;
            Previous = previous;
        }
    }
}