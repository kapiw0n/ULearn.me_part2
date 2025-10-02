private static IEnumerable<int> ZipSum(IEnumerable<int> first, IEnumerable<int> second)
{
    var e1 = first.GetEnumerator();
    var e2 = second.GetEnumerator();
        
    while (e1.MoveNext() && e2.MoveNext())
    {
        yield return e1.Current + e2.Current; // Возвращаем сумму текущих элементов
    }
}