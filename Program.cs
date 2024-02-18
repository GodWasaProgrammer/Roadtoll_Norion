using Roadtoll_Norion;

int year = 2024;
TollCalculator tollCalculator = new(year);

/// this should be a cost of 36
// declare passes in a single day
DateTime[] PassesInOneDay =
[
    new(year, 2, 1, 15, 0, 59),
    new(year, 2, 1, 15, 29, 59),
    new(year, 2, 1, 15, 59, 59),
    new(year, 2, 1, 14, 59, 59),
    new(year, 2, 1, 17, 25, 59),
    new(year, 2, 1, 16, 45, 59),
];

/// just for demonstration purposes
int totalFee = 0;
try
{
    totalFee = tollCalculator.GetTollFee(new Car(), PassesInOneDay);
    Console.WriteLine(totalFee);
}
catch (Exception ex)
{
#if DEBUG
    Console.WriteLine($"Exception:{ex.Message}");
#else 
    //log the error (out of scope) 
    Console.WriteLine("An Error Occured Please Inform your Friendly Developers");
#endif
}
