public static IEnumerable<(int Start, int End)> PlanSchedule(IEnumerable<(int Start, int End)> meetings)
    {
        var leftEdge = int.MinValue;
        foreach (var meeting in meetings.OrderBy(m => m.End))
        {
            if (meeting.Start >= leftEdge)
            {
                leftEdge = meeting.End;
                yield return meeting;
            }
        }
    }