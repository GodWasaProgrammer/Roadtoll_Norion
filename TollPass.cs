namespace Roadtoll_Norion
{
    internal class TollPass
    {
        public DateTime Date { get; }
        public int Fee { get; }

        public bool IsTollFree { get; set; }

        public TollPass(DateTime date, int fee, bool IsGracePeriod)
        {
            Date = date;
            Fee = fee;
            IsTollFree = IsGracePeriod;
        }

        public override string ToString()
        {
            return $"Date: {Date}, Fee: {Fee} Is Within Grace Period:{IsTollFree}";
        }

    }

}