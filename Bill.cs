namespace Roadtoll_Norion
{
    internal class Bill
    {
        private int _totalFee;
        public int TotalFee => _totalFee;

        private List<TimeAndFee> _timesAndFees = new();
        public IReadOnlyList<TimeAndFee> TimesAndFees => _timesAndFees;

        private Guid _regPlate;
        public Guid RegPlate => _regPlate;

        public Bill(Car billedCar)
        {
            _totalFee = 0;
            _regPlate = billedCar.GetRegistryPlate();
        }

        public void AddTollPass(TimeAndFee timeAndFee)
        {
            _timesAndFees.Add(timeAndFee);
            _totalFee += timeAndFee.Fee;
        }

    }

}
