namespace Roadtoll_Norion
{
    internal class TimeAndFee
    {
        private TimeOnly _start;
        private TimeOnly _end;
        private int _fee;

        public int Fee => _fee;

        public TimeAndFee(int startH, int startM, int stopH, int stopM, int fee)
        {
            _start = new TimeOnly(startH, startM);
            _end = new TimeOnly(stopH, stopM);
            _fee = fee;
        }

        internal bool IsInTollTime(DateTime Date)
        {
            TimeOnly time = TimeOnly.FromDateTime(Date);
            return time >= _start && time <= _end;
        }

    }

}