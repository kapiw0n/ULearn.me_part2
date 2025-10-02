public static void HeapifyUp(List<int> heap)
{
	var itemIndex = heap.Count-1;
	while (heap[itemIndex] < heap[itemIndex / 2] && itemIndex > 1)
	{
		var parentIndex = itemIndex / 2;
		var t = heap[itemIndex];
		heap[itemIndex] = heap[parentIndex];
		heap[parentIndex] = t;
		itemIndex = parentIndex;
	}
}