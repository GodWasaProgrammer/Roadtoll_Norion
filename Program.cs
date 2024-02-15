namespace Roadtoll_Norion
{
    internal class Program
    {
        static void Main()
        {
            TollCalculator tollCalculator = new TollCalculator();

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

            Guid RegistryPlate = new Guid();

            // Call GetTollFee method
            TollFeeResult tollFeeResult = tollCalculator.GetTollFee(new Car(RegistryPlate), PassesInOneDay);

            // Create a new instance of Bill
            Bill exampleBill = new(new Car(RegistryPlate));

            exampleBill.AddTotalFee(tollFeeResult.TotalFee);

            // Add toll passes with fees to the bill
            exampleBill.AddTollPasses(PassesInOneDay, tollFeeResult.FeesPerPass, tollFeeResult.IsTollFreePerPass);

            Console.WriteLine(exampleBill);

        }

    }

}