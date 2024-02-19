namespace Roadtoll_Norion;

internal class TimeAndFee
{
    private readonly TimeOnly _start;
    private readonly TimeOnly _end;
    private readonly int _fee;
    private const int MaxSeconds = 59;
    private const int MaxMillies = 999;
    private const int MaxMicros = 999;
    public int Fee => _fee;
    public TimeAndFee(int startHour, int startMinute, int stopHour, int stopMinute, int fee)
    {
        _start = new TimeOnly(startHour, startMinute);
        _end = new TimeOnly(stopHour, stopMinute, MaxSeconds, MaxMillies, MaxMicros);
        _fee = fee;
    }

    /// method to let caller know if the time is in the toll time
    /// </summary>
    /// <param name="Date"></param>
    /// <returns></returns>
    internal bool IsInTollTime(TimeOnly time)
    {
        return time >= _start && time <= _end;
    }
}