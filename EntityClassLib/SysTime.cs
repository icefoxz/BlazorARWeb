namespace EntityClassLib;

public static class SysTime
{
    /// <summary>
    /// 获取当前Unix时间戳
    /// </summary>
    /// <returns></returns>
    public static long EpochSecondsNow() => (long)(DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;
    public static long EpochMilliSecondsNow() => (long)(DateTime.UtcNow - DateTime.UnixEpoch).TotalMilliseconds;

    public static TimeSpan EpochNowSecDiff(long epochSecs)=> DateTime.UtcNow - DateTime.UnixEpoch.AddSeconds(epochSecs);

    public static TimeSpan EpochNowMillSecDiff(long epochMilliseconds) =>
        DateTime.UtcNow - DateTime.UnixEpoch.AddMilliseconds(epochMilliseconds);

    public static DateTime FromEpochSeconds(long seconds, bool localTime = true)
    {
        var dt = new DateTime(DateTime.UnixEpoch.AddSeconds(seconds).Ticks, DateTimeKind.Utc);
        return localTime ? dt.AddHours(8) : dt;
    }

    public static DateTime FromEpochMilliSeconds(long milliseconds,bool localTime = true)
    {
        var dt = DateTime.UnixEpoch.AddMilliseconds(milliseconds);
        return localTime ? dt.AddHours(8) : dt;
    }

    public static string TimeAgo(this TimeSpan timeSpan,string agoText,string dayText = "d",string hourText = "h",string minText = "m",string justNowText = "just now")
    {
        var hours = timeSpan.Hours;
        var days = timeSpan.Days;
        var mins = timeSpan.Minutes;
        if (hours == 0 && days == 0 && mins == 0)
            return justNowText;
        var dText = days <= 0 ? "" : days + dayText;
        var hText = hours <= 0 ? "" : hours + hourText;
        var mText = mins <= 0 ? "" : mins + minText;
        return $"{dText} {hText} {mText} {agoText}".Trim();
    }

    public static string ToMyString(this DateTime date) => date.ToString("ddd dd-MMM-yy");

    public static DateTime SecondsToUnixEpochDate(this long seconds, bool localTime = true) => FromEpochSeconds(seconds, localTime);

    public static string SecondsToDate(this long seconds, bool localTime = true) =>
        FromEpochSeconds(seconds, localTime).ToMyString();

    public static DateTime MilSecondsToUnixEpochDate(this long milliseconds, bool localTime = true) => FromEpochMilliSeconds(milliseconds, localTime);

    public static string MilSecondsToDate(this long milliseconds, bool localTime = true) => FromEpochMilliSeconds(milliseconds, localTime).ToString("g");

    public static long SecEpochNowAdd(TimeSpan timeSpan)
    {
        var dt = DateTime.UtcNow.Add(timeSpan);
        return (long)(dt - DateTime.UnixEpoch).TotalSeconds;
    }
    public static long MilliSecEpochNowAdd(TimeSpan timeSpan)
    {
        var dt = DateTime.UtcNow.Add(timeSpan);
        return (long)(dt - DateTime.UnixEpoch).TotalMilliseconds;
    }
    public static double ConvertFromUtcInSec(DateTime utcDate) => (utcDate - DateTime.UnixEpoch).TotalSeconds;
    public static double ConvertFromUtcInMilSec(DateTime utcDate) => (utcDate - DateTime.UnixEpoch).TotalMilliseconds;
}