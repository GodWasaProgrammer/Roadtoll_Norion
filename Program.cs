namespace Roadtoll_Norion
{
    internal class Program
    {
        static void Main()
        {
            TollCalculator tollCalculator = new TollCalculator();

            IVehicle Car = new Car();

            DateTime[] dateTimes = new DateTime[5];

            dateTimes[0] = new DateTime(2024, 01, 09, 17, 05, 0);
            dateTimes[1] = new DateTime(2024, 02, 09, 17, 05, 0);
            dateTimes[2] = new DateTime(2024, 02, 05, 17, 05, 0);
            dateTimes[3] = new DateTime(2024, 02, 02, 08, 05, 0);
            dateTimes[4] = new DateTime(2024, 02, 08, 15, 05, 0);


            foreach (DateTime dt in dateTimes)
            {
                Console.WriteLine(tollCalculator.GetTollFee(dt, Car));
            }

        }

    }

}