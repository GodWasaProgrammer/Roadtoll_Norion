namespace Roadtoll_Norion
{
    internal class Program
    {
        static void Main()
        {
            DateOnly CurrentDate = new DateOnly(2024, 2, 1);

            TollCalculator tollCalculator = new TollCalculator(CurrentDate);

            // declare passes in a single day
            DateTime[] PassesInOneDay = new DateTime[]
            {
                new DateTime(2024, 2, 1, 15, 0, 0),
                new DateTime(2024, 2, 1, 15, 30, 0),
                new DateTime(2024, 2, 1, 15, 45, 0),
                new DateTime(2024, 2, 1, 15, 55, 0),
                new DateTime(2024, 2, 1, 16, 45, 0),
                new DateTime(2024, 2, 1, 17, 25, 0),
            };

            //DateTime[] NullPass = new DateTime[] { };

            // Call GetTollFee method
            int totalfee = tollCalculator.GetTollFee(new Car(), PassesInOneDay);

            Console.WriteLine(totalfee);
        }

    }

}