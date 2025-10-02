using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public class StatisticsTask
    {
        public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
        {
            var timeSpans = visits
                .GroupBy(v => v.UserId)
                .SelectMany(g => ProcessUserVisits(g.OrderBy(v => v.DateTime).ToList(), slideType))
                .ToList();

            return timeSpans.Count > 0 ? timeSpans.Median() : 0.0;
        }

		private static IEnumerable<double> ProcessUserVisits(IEnumerable<VisitRecord> orderedVisits,
															SlideType slideType)
        {
            var visitsList = orderedVisits.ToList();
            return visitsList
                .Select((visit, index) => new { Visit = visit, Index = index })
                .Where(x => x.Visit.SlideType == slideType)
                .Select(x => new
                {
                    Current = x.Visit,
                    Next = visitsList
                        .Skip(x.Index + 1)
                        .FirstOrDefault(v => v.SlideId != x.Visit.SlideId)
                })
                .Where(pair => pair.Next != null)
                .Select(pair => pair.Next.DateTime - pair.Current.DateTime)
                .Where(ts => ts >= TimeSpan.FromMinutes(1) && ts <= TimeSpan.FromMinutes(120))
                .Select(ts => ts.TotalMinutes);
        }
    }
}