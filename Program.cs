namespace Roadtoll_Norion
{
    internal class Program
    {
        static void Main()
        {
            int year = 2024;
            TollCalculator tollCalculator = new TollCalculator(year);

            /// this should be a cost of 36
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

            // this should be free since tomorrow is a holiday
            DateTime[] TomorrowHolidayTest = new DateTime[]
            {
                new DateTime(2024,1,5,15, 0, 0),
                new DateTime(2024,1,5,15,30, 0),
            };

            DateTime[] EmptyDateTimes = new DateTime[] { };

            /// just for demonstration purposes
            int totalFee = 0;
            try
            {
                totalFee = tollCalculator.GetTollFee(new Car(), PassesInOneDay);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Exception:{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            ///just for demonstration purposes
            Console.WriteLine(totalFee);
        }

    }

}