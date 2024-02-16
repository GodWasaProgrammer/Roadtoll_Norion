using PublicHoliday;

namespace Roadtoll_Norion
{
    public class TollCalculator
    {
        /// </summary>
        /// Added requirement to supply year to calculate
        /// This is to cut down on possibly expensive API calls
        /// within the calling method.
        /// <param name="YearToCalculate">The year to be calculated</param>
        public TollCalculator(DateOnly YearToCalculate)
        {
            Holidays = new SwedenPublicHoliday().PublicHolidays(YearToCalculate.Year);
        }

        private IList<DateTime> Holidays;

        /// <summary>
        /// List of vehicles that are toll free
        /// I have also created the necessary classes for the vehicles
        /// which adhere to the Vehicle interface, and does not change any implementation
        /// details of Vehicle
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

        /// <summary>
        /// So this has been modified so that it correctly segments the passes into time intervals
        /// then individually calculates the toll fee for each interval
        /// it will then sum up the toll fees for each interval and return the total
        /// In my opinion the method is now much more clear with intent, and also easier to read
        /// It also correctly handles time intervals which the OG method did not
        /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total toll fee for that day
         */
        public int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {
            // if we dont have a vehicle or dates, we cant calculate the toll fee
            // or if the vehicle is tollfree, or the date is tollfree we return 0
            if (vehicle == null || dates == null || IsTollFreeVehicle(vehicle) || IsTollFreeDate(dates[0]))
            {
                return 0;
            }

            // set the initial date to the first date in the array
            DateTime? StartTime = dates.First();

            // since we dont know if the dates are in order, we will sort them
            Array.Sort(dates);

            List<List<DateTime>> GraceTimes = new List<List<DateTime>>();

            // storing the values within an hour-interval
            var GracePeriod = new List<DateTime>();

            foreach (DateTime date in dates)
            {
                int nextFee = GetTollFee(date, vehicle);

                // if the date is within the grace period, we will add it to the list of grace period passes
                if (StartTime != null && (date - StartTime.Value) <= TimeSpan.FromMinutes(60))
                {
                    GracePeriod.Add(date);
                }
                else
                {
                    GraceTimes.Add(GracePeriod);
                    GracePeriod = new List<DateTime>();
                    StartTime = date;
                    GracePeriod.Add(date);
                }

                // if we have made segments of all time intervals, we will add the last segment to the list of grace period passes
                if (date == dates.Last())
                {
                    GraceTimes.Add(GracePeriod);
                }

            }

            int totalFee = 0;
            foreach (var gracePeriod in GraceTimes)
            {
                totalFee += gracePeriod.Max(x => GetTollFee(x, vehicle));
            }

            if (totalFee > 60) totalFee = 60;
            return totalFee;
        }

        /// The vehicle to be checked if it is toll free
        /// This will be checked against the list of toll free vehicles
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
                return false;

            if (TollFreeVehicles.Contains(vehicle.GetType()))
                return true;

            return false;
        }

        
        /// In my opinion this shouldnt be an overload, it could be its own method, because its only ever called in another overload...
        /// </summary>
        /// <param name="date">the single date and time to be checked</param>
        /// <param name="vehicle">the vehicle which is being tolled</param>
        /// <returns></returns>
        private int GetTollFee(DateTime date, Vehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle))
                return 0;

            var TimeFee = TimesAndFees.Where(x => x.IsInTollTime(date)).FirstOrDefault();

            return TimeFee == null ? 0 : TimeFee.Fee;
        }

        /// </summary>
        /// Checks if our supplied date is a holiday, or a weekday
        /// <param name="date"> The Date To Be Checked</param>
        /// <returns>a bool that tells you if its free or not</returns>
        private bool IsTollFreeDate(DateTime date)
        {
            if (FreeWeekDays.Contains(date.DayOfWeek))
                return true;

            var dateOnly = date.Date;
            if (Holidays.Contains(dateOnly))
                return true;

            return false;
        }

    }

}