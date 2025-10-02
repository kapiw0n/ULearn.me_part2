public static int GetOptimalScheduleGain(params Event[] events)
{
    var fakeBorderEvent = new Event { StartTime = int.MinValue, FinishTime = int.MinValue, Price = 0 };
    events = events.Concat(new[] { fakeBorderEvent }).OrderBy(e => e.FinishTime).ToArray();
    
    var opt = new int[events.Length];
    opt[0] = 0;
    for (var k = 1; k < events.Length; k++)
    {
        var i = k - 1;
        while (i > 0)
        {
            if (events[i].FinishTime < events[k].StartTime) break;
            i--;
        }
        opt[k] = Math.Max(opt[k - 1], opt[i] + events[k].Price);
    }
    return opt.Last();
}