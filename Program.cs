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
                new DateTime(2024, 2, 1, 15, 0, 59),
                new DateTime(2024, 2, 1, 15, 29, 59),
                new DateTime(2024, 2, 1, 15, 59, 59),
                new DateTime(2024, 2, 1, 14, 59, 59),
                new DateTime(2024, 2, 1, 16, 45, 59),
                new DateTime(2024, 2, 1, 17, 25, 59),
            };


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
                Console.WriteLine($"Exception:{ex.Message}");
            }

            ///just for demonstration purposes
            Console.WriteLine(totalFee);
        }

    }

}