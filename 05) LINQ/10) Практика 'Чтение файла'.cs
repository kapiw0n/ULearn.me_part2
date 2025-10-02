using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace linq_slideviews;

public class ParsingTask
{
    public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
    {
        return lines
            .Skip(1)
            .Select(TryParseSlide)
            .Where(slide => slide != null)
            .ToDictionary(slide => slide.SlideId);
    }

    private static SlideRecord TryParseSlide(string line)
    {
        var parts = line.Split(';');
        if (parts.Length != 3) return null;
        
        if (!int.TryParse(parts[0], out int slideId)) return null;
        if (!Enum.TryParse(parts[1], true, out SlideType slideType)) return null;
        
        return new SlideRecord(slideId, slideType, parts[2]);
    }

    public static IEnumerable<VisitRecord> ParseVisitRecords(
        IEnumerable<string> lines, 
        IDictionary<int, SlideRecord> slides)
    {
        return lines
            .Skip(1)
            .Select(line => ParseVisit(line, slides))
            .Where(visit => visit != null);
    }
	//согласен это было ужасно :(
	private static VisitRecord ParseVisit(string line, IDictionary<int, SlideRecord> slides)
	{
		try
		{
			var parts = line.Split(';');
			int userId = int.Parse(parts[0]);
			int slideId = int.Parse(parts[1]);
			var slide = slides[slideId];
			var dateTime = DateTime.ParseExact(
				$"{parts[2]} {parts[3]}", 
				"yyyy-MM-dd HH:mm:ss", 
				CultureInfo.InvariantCulture);
			return new VisitRecord(userId, slideId, dateTime, slide.SlideType);
		}
		catch (Exception ex) when (ex is FormatException 
								   || ex is KeyNotFoundException 
								   || ex is IndexOutOfRangeException)
		{
			throw new FormatException($"Wrong line [{line}]");
		}
	}

    private static DateTime ParseDateTime(string dateStr, string timeStr, string line)
    {
        try
        {
            return DateTime.ParseExact(
                $"{dateStr} {timeStr}", 
                "yyyy-MM-dd HH:mm:ss", 
                CultureInfo.InvariantCulture
            );
        }
        catch
        {
            throw new FormatException($"Wrong line [{line}]");
        }
    }
}