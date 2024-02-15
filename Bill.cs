using System.Text;

namespace Roadtoll_Norion
{
    internal class Bill
    {
        private int _totalFee;
        public int TotalFee => _totalFee;

        private List<TollPass> _tollPasses = new();
        public IReadOnlyList<TollPass> TollPasses => _tollPasses;

        private Guid _regPlate;
        public Guid RegPlate => _regPlate;

        public Bill(Car billedCar)
        {
            _totalFee = 0;
            _regPlate = billedCar.GetRegistryPlate();
        }

        public void AddTotalFee(int totalFee)
        {
            _totalFee = totalFee;
        }

        public void AddTollPasses(IEnumerable<DateTime> passDates, IEnumerable<int> fees, IEnumerable<bool> isTollFreePerPass)
        {
            if (passDates == null || fees == null || isTollFreePerPass == null)
                throw new ArgumentNullException("passDates, fees, or isTollFreePerPass");

            if (passDates.Count() != fees.Count() || passDates.Count() != isTollFreePerPass.Count())
                throw new ArgumentException("passDates, fees, and isTollFreePerPass must have the same number of elements");

            for (int i = 0; i < passDates.Count(); i++)
            {
                _tollPasses.Add(new TollPass(passDates.ElementAt(i), fees.ElementAt(i), isTollFreePerPass.ElementAt(i)));
            }

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Registry Plate: {_regPlate}");
            sb.AppendLine($"Total Fee: {_totalFee}");

            foreach (TollPass pass in _tollPasses)
            {
                sb.AppendLine(pass.ToString());
            }

            return sb.ToString();
        }

    }

}