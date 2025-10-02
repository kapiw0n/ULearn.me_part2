namespace BinaryTrees;
public class BinaryTree<T> where T : IComparable
{
    private class Node<TNode>
    {
        public TNode Key { get; set; }
        public Node<TNode> Left { get; set; }
        public Node<TNode> Right { get; set; }

        public Node(TNode key)
        {
            Key = key;
        }
    }

    private Node<T> root;

	public void Add(T key)
	{
		if (root == null)
		{
			root = new Node<T>(key);
			return;
		}

		var parent = FindParentForNewNode(key);
		AttachToParent(parent, key);
	}

	private Node<T> FindParentForNewNode(T key)
	{
		Node<T> current = root;
		Node<T> parent = null;

		while (current != null)
		{
			parent = current;
			int comparison = key.CompareTo(current.Key);

			if (comparison == 0) 
				return null; // Обнаружен дубликат

			current = comparison < 0 ? current.Left : current.Right;
		}

		return parent;
	}

	private void AttachToParent(Node<T> parent, T key)
	{
		if (parent == null) return; // Для случая дубликата

		int comparison = key.CompareTo(parent.Key);
		var newNode = new Node<T>(key);

		if (comparison < 0)
			parent.Left = newNode;
		else
			parent.Right = newNode;
	}

    public bool Contains(T key)
    {
        Node<T> current = root;
        while (current != null)
        {
            int comparison = key.CompareTo(current.Key);
            if (comparison == 0)
                return true;
            
            current = comparison < 0 ? current.Left : current.Right;
        }
        return false;
    }
}