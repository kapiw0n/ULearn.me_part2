using System;
using System.Collections;
using System.Collections.Generic;
namespace BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        private class TreeNode // 
        {
            public T Value; //
            public TreeNode LeftChild, RightChild; 
            public int NodeCount = 1; 
            public TreeNode(T value) => Value = value; 
        }

        private TreeNode rootNode; 

        public void Add(T value) 
        {
            if (rootNode == null) // root is null
            {
                rootNode = new TreeNode(value);
                return;
            }
            InsertTo(rootNode, value); 
        }

        private void InsertTo(TreeNode currentNode, T value)
        {
            while (true)
            {
                currentNode.NodeCount++; //node count
                int comparison = value.CompareTo(currentNode.Value); 
                if (comparison < 0) // If less than
                {
                    if (currentNode.LeftChild == null) // Check left child
                    {
                        currentNode.LeftChild = new TreeNode(value);
                        return;
                    }
                    currentNode = currentNode.LeftChild; // Move left
                }
                else // If greater or equal
                {
                    if (currentNode.RightChild == null) // Check right child
                    {
                        currentNode.RightChild = new TreeNode(value);
                        return;
                    }
                    currentNode = currentNode.RightChild; // Move right
                }
            }
        }

        public bool Contains(T value) 
        {
            var currentNode = rootNode; // 
            while (currentNode != null) // Traverse tree
            {
                int comparison = value.CompareTo(currentNode.Value); 
                if (comparison == 0) // Found
                    return true;
                currentNode = comparison < 0 ? currentNode.LeftChild : currentNode.RightChild; // Move accordingly
            }
            return false; // Not found
        }

        public T this[int index] // Index
        {
            get
            {
                if (index < 0 || index >= (rootNode?.NodeCount ?? 0)) 
                    throw new ArgumentOutOfRangeException();
                return GetByIndex(rootNode, index); // Call GetByIndex
            }
        }

        private T GetByIndex(TreeNode currentNode, int index) 
        {
            int leftCount = currentNode.LeftChild?.NodeCount ?? 0; 
            if (index < leftCount) // Go left
                return GetByIndex(currentNode.LeftChild, index);
            if (index == leftCount) // Found
                return currentNode.Value; // Ret
            return GetByIndex(currentNode.RightChild, index - leftCount - 1); // Go right
        }

        public IEnumerator<T> GetEnumerator() // Enumerator
        {
            return Traverse(rootNode).GetEnumerator(); // 
        }

        private IEnumerable<T> Traverse(TreeNode currentNode) //
        {
            if (currentNode == null) // Check if null
                yield break; // Exit
            foreach (var value in Traverse(currentNode.LeftChild)) // Traverse left
                yield return value;
            yield return currentNode.Value; // Yield current value
            foreach (var value in Traverse(currentNode.RightChild)) // Traverse right
                yield return value;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); // Non-generic
    }
}