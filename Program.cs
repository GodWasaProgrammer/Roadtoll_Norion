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
            Console.WriteLine("Public Holiday");
            Guid Reg_plate1 = new Guid();
            Console.WriteLine(tollCalculator.GetTollFee(date, new Car(Reg_plate1)));

            Console.WriteLine("Not a public holiday");
            // day after new years day is not a public holiday
            Guid Reg_plate2 = new Guid();
            Console.WriteLine(tollCalculator.GetTollFee(date2, new Car(Reg_plate2)));

            // declare passes in a single day
            DateTime[] PassesInOneDay = new DateTime[]
            {
                new DateTime(2024, 2, 1, 6, 0, 0),
                new DateTime(2024, 2, 1, 6, 30, 0),
                new DateTime(2024, 2, 1, 7, 0, 0),
                new DateTime(2024, 2, 1, 8, 0, 0),
                new DateTime(2024, 2, 1, 8, 30, 0),
                new DateTime(2024, 2, 1, 15, 0, 0),
                new DateTime(2024, 2, 1, 15, 30, 0),
                new DateTime(2024, 2, 1, 17, 0, 0),
                new DateTime(2024, 2, 1, 18, 0, 0)
            };

            // print the toll fee for each pass
            Console.WriteLine("Passes in a single day by one vehicle:");
            Guid Reg_plate3 = new Guid();
            foreach (DateTime pass in PassesInOneDay)
            {
                Console.WriteLine(tollCalculator.GetTollFee(pass, new Car(Reg_plate3)));
            }

            Console.WriteLine("Total for one car passing:");
            Guid Reg_plate4 = new Guid();
            Console.WriteLine(tollCalculator.GetTollFee(new Car(Reg_plate4), PassesInOneDay));

            Guid RegistryPlate = new Guid();

            Bill exampleBill = new Bill(new Car(RegistryPlate));

            foreach (DateTime pass in PassesInOneDay)
            {
                exampleBill.AddTollPass(new TimeAndFee(pass.Hour, pass.Minute, pass.Hour, pass.Minute, tollCalculator.GetTollFee(pass, new Car(RegistryPlate))));
            }

            foreach (var pass in exampleBill.TimesAndFees)
            {
                Console.WriteLine(pass);
            }

        }

    }

}