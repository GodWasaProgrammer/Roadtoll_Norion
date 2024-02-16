namespace Roadtoll_Norion
{
    internal class Program
    {
        static void Main()
        {
            // passed into tollcalculator
            // this obviously need to match with the year you want to calculate,
            // the month and day doesnt matter since its only the year that is used
            DateOnly CurrentDate = new DateOnly(2024, 2, 1);
            
            /// <summary>
            /// instantiate tollcalculator
            TollCalculator tollCalculator = new TollCalculator(CurrentDate);

            /// <summary>
            /// Just for demonstration purposes
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

            /// just for demonstration purposes
            // Call GetTollFee method
            int totalfee = tollCalculator.GetTollFee(new Car(), PassesInOneDay);
            
            ///just for demonstration purposes
            Console.WriteLine(totalfee);
        }

    }

}