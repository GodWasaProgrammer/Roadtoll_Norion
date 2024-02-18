using PublicHoliday;

namespace Roadtoll_Norion;

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

    /// Private list of holidays which is taken from the constructor tollcalculator
    private readonly IList<DateTime> _Holidays;

    /// The maximum toll fee for one day
    private const int MaxTollFee = 60;

    /// List of vehicles that are toll free
    /// I have also created the necessary classes for the vehicles
    /// which adhere to the Vehicle interface, and does not change any implementation <summary>
    static readonly List<Type> TollFreeVehicles =
    [
        typeof(Motorbike),
        typeof(Tractor),
        typeof(Emergency),
        typeof(Diplomat),
        typeof(Foreign)
    ];

    /// List of days that are toll free
    /// This is a list because its flexible and easily extendable if necessary 
    static readonly List<DayOfWeek> FreeWeekDays =
    [
        DayOfWeek.Saturday,
        DayOfWeek.Sunday
    ];

    /// List of times and fees, which is using the TimeAndFee class for a structured approach
    static readonly List<TimeAndFee> TimesAndFees =
    [
        new(06,00,06,29,8),
        new(06,30,06,59,13),
        new(07,00,07,59,18),
        new(08,00,08,29,13),
        new(08,30,14,59,8),
        new(15,00,15,29,13),
        new(15,30,16,59,18),
        new(17,00,17,59,13),
        new(18,00,18,29,8)
    ];

    /// <summary>
    /// So this has been modified so that it correctly segments the passes into time intervals
    /// then individually calculates the toll fee for each interval
    /// it will then sum up the toll fees for each interval and return the total
    /// In my opinion the method is now much more clear with intent, and also easier to read
    /// This in my opinion should rather return a custom class that lets you see which passes
    /// was free and so on, but i didnt want to mess with the methods signature.
    /// It also correctly handles time intervals which the OG method did not
    /// also includes exception handling for the parameters
    /// Will throw exceptions on nulls/empty/if you passed an array with more then one day
    /*
     * Calculate the total toll fee for one day
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        /// exception handling for the parameters
        ArgumentNullException.ThrowIfNull(vehicle);
        ArgumentNullException.ThrowIfNull(dates);

        // if the vehicle is toll free, or the date is toll free, there will be no toll fee
        if (IsTollFreeVehicle(vehicle) || IsTollFreeDate(dates.First())) return 0;

        // set the initial date to the first date in the array
        DateTime? startTime = dates.First();

        foreach (DateTime date in dates)
        {
            if (date.Day != startTime.Value.Day)
            {
                throw new ArgumentException("This method is not made to handle more then one day, pass only a single day");
            }
        }

        // since we dont know if the dates are in order, we will sort them
        Array.Sort(dates);
        // our list of lists of time intervals, this is a list of all the passes within an hour
        List<List<DateTime>> graceTimes = [];

        // storing the values within an hour-interval
        var gracePeriod = new List<DateTime>();

        foreach (DateTime date in dates)
        {
            // if the date is within the grace period, we will add it to the list of grace period passes
            if (date - startTime.Value <= TimeSpan.FromMinutes(60))
            {
                gracePeriod.Add(date);
            }
            else
            {
                graceTimes.Add(gracePeriod);
                gracePeriod = [];
                startTime = date;
                gracePeriod.Add(date);
            }
            // weird
            // if we have made segments of all time intervals, we will add the last segment to the list of grace period passes
            if (date == dates.Last())
            {
                graceTimes.Add(gracePeriod);
            }
        }

        // we then loop on our period
        int totalFee = 0;
        foreach (var currentGracePeriod in graceTimes)
        {
            // this list will store all our fees for the tollpasses
            List<int> fees = [];

            foreach (var pass in currentGracePeriod)
            {
                if (TimesAndFees.FirstOrDefault(x => x.IsInTollTime(TimeOnly.FromDateTime(pass))) is { } timeFee)
                {
                    fees.Add(timeFee.Fee);
                }
            }

            // we then only add the max one,if there is one.
            if (fees.Count > 0)
            {
                totalFee += fees.Max();
            }
        }

        if (totalFee > MaxTollFee)
        {
            totalFee = MaxTollFee;
        }

        return totalFee;
    }

    /// </summary>
    /// In my opinion this shouldnt be an overload, it could be its own method, because its only ever called in another overload...
    /// atleast in the original code, since i dont like that it was being called by another overload, i made the necessary calls
    /// directly from the other overload.
    /// <param name="date"> The single date and time to be checked </param>
    /// <param name="vehicle"> The vehicle which is being tolled </param>
    /// <returns> An int representing the fee for one day </returns>
    public int GetTollFee(DateTime date, IVehicle vehicle)
    {
        // Since datetime is a value type we dont need to check that as its a struct and cannot be null or empty.
        //This is only ever needed if this is called by itself and not from its other overload, but since its not specified
        // in the instruction, i put it in anyway.
        ArgumentNullException.ThrowIfNull(vehicle);

        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        var timeFee = TimesAndFees.Where(x => x.IsInTollTime(TimeOnly.FromDateTime(date))).FirstOrDefault();

        return timeFee?.Fee ?? 0;
    }

    /// </summary>
    /// <param name="vehicle"> The vehicle to check </param>
    /// <returns> A bool indicating if the vehicle is free of toll charges or not </returns>
    private static bool IsTollFreeVehicle(IVehicle vehicle)
    {
        if (vehicle is null) return false;

        if (TollFreeVehicles.Contains(vehicle.GetType())) return true;

        return false;
    }

    private readonly List<int> FreeMonths =
    [
        7,
    ];

    /// </summary>
    /// Checks if our supplied date is a holiday, or a weekday
    /// Also checks if its the month of july, which is free
    /// Also checks if tomorrow is a holiday, then that day will be fre aswell
    /// <param name="date"> The Date To Be Checked </param>
    /// DateTime is a value type and will not be null/empty so no exception handling here
    /// <returns> A bool that tells you if its free or not </returns>
    private bool IsTollFreeDate(DateTime date)
    {
        if (FreeWeekDays.Contains(date.DayOfWeek))
            return true;

        var dateOnly = date.Date;
        if (_Holidays.Contains(dateOnly))
            return true;

        var isTomorrowAHoliday = date.AddDays(1).Date;
        if (_Holidays.Contains(isTomorrowAHoliday))
            return true;

        // this will allow the month of July to be free since this is specified in the instruction 
        foreach (var freeMonth in FreeMonths)
        {
            if (date.Month == freeMonth)
                return true;
        }

        return false;
    }

}