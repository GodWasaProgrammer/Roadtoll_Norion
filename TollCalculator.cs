using PublicHoliday;

namespace Roadtoll_Norion
{
    public class TollCalculator
    {
        /// <summary>
        /// List of vehicles that are toll free
        /// </summary>
        static List<Type> TollFreeVehicles = new()
        {
            typeof(Motorbike),
            typeof(Tractor),
            typeof(Emergency),
            typeof(Diplomat),
            typeof(Foreign)
        };

        /// <summary>
        /// List of days that are toll free
        /// </summary>
        static List<DayOfWeek> FreeWeekDays = new List<DayOfWeek>
        {
            DayOfWeek.Saturday,
            DayOfWeek.Sunday
        };

        /// <summary>
        /// List of times and fees, which is using the TimeAndFee class for a structured approach
        /// </summary>
        static List<TimeAndFee> TimesAndFees = new()
        {
            new(06,00,06,29,8),
            new(06,30,06,59,13),
            new(07,00,07,59,18),
            new(08,00,08,29,13),
            new(08,30,14,59,8),
            new(15,00,15,29,13),
            new(15,30,16,59,18),
            new(17,00,17,59,13),
            new(18,00,18,29,8)
        };

        /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total toll fee for that day
         */
        public int GetTollFee(IVehicle vehicle, DateTime[] dates)
        {
            DateTime intervalStart = dates[0];
            int totalFee = 0;
            foreach (DateTime date in dates)
            {
                int nextFee = GetTollFee(date, vehicle);
                int tempFee = GetTollFee(intervalStart, vehicle);

                long diffInMillies = date.Millisecond - intervalStart.Millisecond;
                long minutes = diffInMillies / 1000 / 60;

                if (minutes <= 60)
                {
                    if (totalFee > 0) totalFee -= tempFee;
                    if (nextFee >= tempFee) tempFee = nextFee;
                    totalFee += tempFee;
                }
                else
                {
                    totalFee += nextFee;
                }

            }

            if (totalFee > 60) totalFee = 60;
            return totalFee;

        }

        /// <summary>
        /// The vehicle to be checked if it is toll free
        /// This will be checked against the list of toll free vehicles
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        private bool IsTollFreeVehicle(IVehicle vehicle)
        {
            if (vehicle == null) 
                return false;

            if(TollFreeVehicles.Contains(vehicle.GetType()))
                return true;
            
            return false;
        }

        public int GetTollFee(DateTime date, IVehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle))
            return 0;

            var TimeFee = TimesAndFees.Where(x => x.IsInTollTime(date)).FirstOrDefault();

            return TimeFee == null ? 0 : TimeFee.Fee;
        }

        private bool IsTollFreeDate(DateTime date)
        {
            IList<DateTime> Holidays = new SwedenPublicHoliday().PublicHolidays(date.Year);
            
            if (FreeWeekDays.Contains(date.DayOfWeek)) 
                return true;

            var dateOnly = date.Date;
            if (Holidays.Contains(dateOnly))
                return true;
            
            return false;
        }

    }

}