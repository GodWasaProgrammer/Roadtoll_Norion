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
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (FreeWeekDays.Contains(date.DayOfWeek)) 
                return true;

            if (year == 2013)
            {
                if (month == 1 && day == 1 ||
                    month == 3 && (day == 28 || day == 29) ||
                    month == 4 && (day == 1 || day == 30) ||
                    month == 5 && (day == 1 || day == 8 || day == 9) ||
                    month == 6 && (day == 5 || day == 6 || day == 21) ||
                    month == 7 ||
                    month == 11 && day == 1 ||
                    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
                {
                    return true;
                }

            }
            return false;

        }

    }

}