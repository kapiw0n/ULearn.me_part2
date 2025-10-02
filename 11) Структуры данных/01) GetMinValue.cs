public static int GetMinValue(TreeNode root)
{
	return root.Left!=null? GetMinValue(root.Left):root.Value;
}