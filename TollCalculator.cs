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
        public TollFeeResult GetTollFee(IVehicle vehicle, DateTime[] dates)
        {
            List<int> feesPerPass = new List<int>();

            List<bool> isTollFreePerPass = new List<bool>();

            DateTime? previousPassDate = null;

            int totalFeeForDay = 0;

            foreach (DateTime passDate in dates)
            {
                // Get the toll fee for the pass
                int nextFee = GetTollFee(passDate, vehicle);
                bool isWithinGracePeriod = false;

                // Determine if the pass is within the 60-minute grace period
                if (previousPassDate != null && (passDate - previousPassDate.Value) <= TimeSpan.FromMinutes(60))
                {
                    isWithinGracePeriod = true;

                    var lastFee = feesPerPass.LastOrDefault();

                    if (nextFee > lastFee)
                    {
                        // we deduct the last fee if it was lower than the current fee
                        totalFeeForDay -= lastFee;
                        // we then add the current fee which was higher than the last fee
                        totalFeeForDay += nextFee;
                    }

                }
                else
                {
                    totalFeeForDay += nextFee;
                }
                // Add the fee to the list of fees
                feesPerPass.Add(nextFee);

                // Add the result to the list of toll-free passes
                isTollFreePerPass.Add(isWithinGracePeriod);

                // sets our previous pass date to the current pass date
                // to be able to compare the next pass date with the current pass date
                previousPassDate = passDate;
            }

            // Ensure the total fee does not exceed 60
            totalFeeForDay = Math.Min(totalFeeForDay, 60);

            return new TollFeeResult(totalFeeForDay, feesPerPass, isTollFreePerPass);
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

            if (TollFreeVehicles.Contains(vehicle.GetType()))
                return true;

            return false;
        }

        private int GetTollFee(DateTime date, IVehicle vehicle)
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