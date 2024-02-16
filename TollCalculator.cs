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
        public TollCalculator(int yearToCalculate)
        {
            _Holidays = new SwedenPublicHoliday().PublicHolidays(yearToCalculate);
        }

        /// <summary>
        /// Private list of holidays which is taken from the constructor tollcalculator
        /// </summary>
        private IList<DateTime> _Holidays;

        /// <summary>
        /// The maximum toll fee for one day
        /// </summary>
        private const int MaxTollFee = 60;

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
        /// also includes exception handling for the parameters
        /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total toll fee for that day
         */
        public int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {
            /// exception handling for the parameters
            try
            {
                if (vehicle == null)
                    throw new ArgumentNullException("Vehicle cannot be null");
                if (dates == null)
                    throw new ArgumentNullException("Dates cannot be null");
                if (dates.Length == 0)
                    throw new ArgumentException("Dates cannot be empty");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Exception:{e.Message}");
                return 0;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Exception:{e.Message}");
                return 0;
            }

            // if the vehicle is toll free, or the date is toll free, there will be no toll fee
            if (IsTollFreeVehicle(vehicle) || IsTollFreeDate(dates.First()))
                return 0;

            // set the initial date to the first date in the array
            DateTime? startTime = dates.First();

            // since we dont know if the dates are in order, we will sort them
            Array.Sort(dates);

            List<List<DateTime>> graceTimes = new List<List<DateTime>>();

            // storing the values within an hour-interval
            var gracePeriod = new List<DateTime>();

            foreach (DateTime date in dates)
            {
                // if the date is within the grace period, we will add it to the list of grace period passes
                if (startTime != null && (date - startTime.Value) <= TimeSpan.FromMinutes(60))
                {
                    gracePeriod.Add(date);
                }
                else
                {
                    graceTimes.Add(gracePeriod);
                    gracePeriod = new List<DateTime>();
                    startTime = date;
                    gracePeriod.Add(date);
                }

                // if we have made segments of all time intervals, we will add the last segment to the list of grace period passes
                if (date == dates.Last())
                {
                    graceTimes.Add(gracePeriod);
                }

            }

            int totalFee = 0;
            foreach (var currentGracePeriod in graceTimes)
            {
                totalFee += currentGracePeriod.Max(x => GetTollFee(x, vehicle));
            }

            if (totalFee > MaxTollFee) totalFee = MaxTollFee;
            return totalFee;
        }

        /// In my opinion this shouldnt be an overload, it could be its own method, because its only ever called in another overload...
        /// </summary>
        /// <param name="date">the single date and time to be checked</param>
        /// <param name="vehicle">the vehicle which is being tolled</param>
        /// <returns></returns>
        public int GetTollFee(DateTime date, Vehicle vehicle)
        {
            //This is only ever needed if this is called by itself and not from its other overload, but since its not specified
            // in the instruction, i put it in anyway.
            try
            {
                if (vehicle == null)
                    throw new ArgumentNullException("vehicle cant be null");
            }
            catch(ArgumentNullException ex)
            {
                Console.WriteLine($"Exception:{ex.Message}");
            }
            // since datetime is a value type we dont need to check that as its a struct and cannot be null or empty.

            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle))
                return 0;

            var TimeFee = TimesAndFees.Where(x => x.IsInTollTime(date)).FirstOrDefault();

            return TimeFee == null ? 0 : TimeFee.Fee;
        }

        /// The vehicle to be checked if it is toll free
        /// This will be checked against the list of toll free vehicles
        /// this does not have exception handling because it is private and only ever called in the GetTollFee methods
        /// Which has themselves exception handling so empty or null will never be passed here
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

        /// </summary>
        /// Checks if our supplied date is a holiday, or a weekday
        /// <param name="date"> The Date To Be Checked</param>
        /// DateTime is a value type and will not be null/empty so no exception handling here
        /// <returns>a bool that tells you if its free or not</returns>
        private bool IsTollFreeDate(DateTime date)
        {
            if (FreeWeekDays.Contains(date.DayOfWeek))
                return true;

            var dateOnly = date.Date;
            if (_Holidays.Contains(dateOnly))
                return true;

            return false;
        }

    }

}