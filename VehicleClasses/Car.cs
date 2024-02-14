namespace Roadtoll_Norion
{
    public class Car : IVehicle
    {
        public Car(Guid Reg_plate)
        {
            Registry_plate = Reg_plate;
        }

        public string GetVehicleType()
        {
            return "Car";
        }

        private Guid Registry_plate;

        public Guid GetRegistryPlate()
        {
            return Registry_plate;
        }

    }

}