using PublicHoliday;

namespace Roadtoll_Norion
{
    internal class Program
    {
        static void Main()
        {
            TollCalculator tollCalculator = new TollCalculator();

            // new years day
            DateTime date = new DateTime(2024, 1, 1, 6, 0, 0);

            // day after new years day
            DateTime date2 = new DateTime(2024, 1, 2, 6, 0, 0);

            // new years day is a public holiday
            Console.WriteLine(tollCalculator.GetTollFee(date,new Car()));

            // day after new years day is not a public holiday
            Console.WriteLine(tollCalculator.GetTollFee(date2,new Car()));
        }

    }

}