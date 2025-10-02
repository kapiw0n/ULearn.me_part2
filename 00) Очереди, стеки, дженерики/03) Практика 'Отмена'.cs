using System;
using System.Collections.Generic;
namespace LimitedSizeStack
{
    public class ListModel<TItem>
    {
        public List<TItem> Items { get; }
        private readonly LimitedSizeStack<Action> _actionHistory;

        public int UndoLimit { get; }

        public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
        {
        }

        public ListModel(List<TItem> items, int undoLimit)
        {
            Items = items;
            UndoLimit = undoLimit;
            _actionHistory = new LimitedSizeStack<Action>(undoLimit);
        }

        public void AddItem(TItem item)
        {
            Items.Add(item);
            _actionHistory.Push(new Action(ActionType.Add, item));
        }

        public void RemoveItem(int index)
        {
            if (index < 0 || index >= Items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }

            var item = Items[index];
            Items.RemoveAt(index);
            _actionHistory.Push(new Action(ActionType.Remove, item, index));
        }

        public bool CanUndo()
        {
            return _actionHistory.Count > 0;
        }

        public void Undo()
        {
            if (!CanUndo())
            {
                throw new InvalidOperationException("No actions to undo.");
            }

            var lastAction = _actionHistory.Pop();
            if (lastAction.Type == ActionType.Add)
            {
                Items.Remove(lastAction.Item);
            }
            else if (lastAction.Type == ActionType.Remove)
            {
                Items.Insert(lastAction.Index, lastAction.Item);
            }
        }

        private class Action
        {
            public ActionType Type { get; }
            public TItem Item { get; }
            public int Index { get; }

            public Action(ActionType type, TItem item, int index = -1)
            {
                Type = type;
                Item = item;
                Index = index;
            }
        }

        private enum ActionType
        {
            Add,
            Remove
        }
    }
}