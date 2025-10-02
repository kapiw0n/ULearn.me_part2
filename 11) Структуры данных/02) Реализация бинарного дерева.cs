public static TreeNode Search(TreeNode root, int element)
{
    if (root == null) return null;
    if (element == root.Value) return root;
    return element < root.Value 
        ? Search(root.Left, element) 
        : Search(root.Right, element);
}