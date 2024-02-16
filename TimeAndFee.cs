namespace Roadtoll_Norion
{
    internal class TimeAndFee
    {
        private TimeOnly _start;
        private TimeOnly _end;
        private int _fee;

        public int Fee => _fee;

        public TimeAndFee(int startHour, int startMinute, int stopHour, int stopMinute, int fee)
        {
            _start = new TimeOnly(startHour, startMinute);
            _end = new TimeOnly(stopHour, stopMinute);
            _fee = fee;
        }

        /// <summary>
        /// method to let caller know if the time is in the toll time
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        internal bool IsInTollTime(TimeOnly time) 
        {
            return time >= _start && time <= _end;
        }

    }   

}