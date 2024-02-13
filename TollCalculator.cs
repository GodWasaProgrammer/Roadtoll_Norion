﻿    namespace Roadtoll_Norion
{

    public class TollCalculator
    {
        static List<Type> TollFreeVehicles = new List<Type>
        {
            typeof(Motorbike),
            typeof(Tractor),
            typeof(Emergency),
            typeof(Diplomat),
            typeof(Foreign)
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
            if (IsTollFreeDate(date))
            {
                return 0;
            }

            if (IsTollFreeVehicle(vehicle))
            {
                return 0;
            }

            int hour = date.Hour;
            int minute = date.Minute;

            // if between 6:00 and 6:29 cost is 8
            if (hour == 6 && minute >= 0 && minute <= 29) return 8;

            // if between 6:30 and 6:59 cost is 13
            if (hour == 6 && minute >= 30 && minute <= 59) return 13;

            // if between 7:00 and 7:59 cost is 18
            if (hour == 7 && minute >= 0 && minute <= 59) return 18;

            // if between 8:00 and 8:29 cost is 13
            if (hour == 8 && minute >= 0 && minute <= 29) return 13;

            /// this one is strange why the => on hour
            if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;

            // if between 15:00 and 15:29 cost is 13
            if (hour == 15 && minute >= 0 && minute <= 29) return 13;

            // if between 15:30 and 16:59 cost is 18
            if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;

            // if between 17:00 and 17:59 cost is 13
            if (hour == 17 && minute >= 0 && minute <= 59) return 13;

            // if between 18:00 and 18:29 cost is 8
            if (hour == 18 && minute >= 0 && minute <= 29) return 8;

            // all other times cost is 0
            else return 0;
        }

        private bool IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

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