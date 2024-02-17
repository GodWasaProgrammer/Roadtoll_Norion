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
                new DateTime(2024, 2, 1, 15, 29, 0),
                new DateTime(2024, 2, 1, 15, 59, 0),
                new DateTime(2024, 2, 1, 14, 59, 0),
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

            DateTime[] fuckery = new DateTime[]
            {
                new DateTime(2024,2,1,06,59,59,999,999),
            };

            DateTime[] CtorDoubleCheck = new DateTime[]
            {
                new DateTime(2024,2,1,15,29,59,999,999).AddTicks(99999),
                new DateTime(2024,2,1,17,29,59,999,999),
                new DateTime(2024,2,1,18,59,59,999,999)
            };

            /// just for demonstration purposes
            int totalFee = 0;
            try
            {
                totalFee = tollCalculator.GetTollFee(new Car(), CtorDoubleCheck);
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