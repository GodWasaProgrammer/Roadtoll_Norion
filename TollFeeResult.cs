namespace Roadtoll_Norion
{
    public class TollFeeResult
    {
        public int TotalFee { get; }
        public List<bool> IsTollFreePerPass { get; }

        public List<int> FeesPerPass { get; set; }

        public TollFeeResult(int totalFee, List<int> feesPerPass, List<bool> isTollFreePerPass)
        {
            TotalFee = totalFee;
            FeesPerPass = feesPerPass;
            IsTollFreePerPass = isTollFreePerPass;
        }
    }
}