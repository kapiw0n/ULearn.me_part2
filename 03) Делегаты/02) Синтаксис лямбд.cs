private static readonly Func<int> zero = () => 0 ;
private static readonly Func<int, string> toString = x => $"{x}" ; 
private static readonly Func<double, double, double> add = (x, y) => x + y ;
private static readonly Action<string> print = x => Console.WriteLine(x) ;