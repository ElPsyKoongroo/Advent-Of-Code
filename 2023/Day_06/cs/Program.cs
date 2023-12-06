namespace cs;

using Race1 = (int time, int distance);
using Race2 = (ulong time, ulong distance);


class Program
{
    static void Main(string[] args) {
        string path = args.FirstOrDefault() ?? "../AOCinput";
        
        Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }

    public static int Sol1(string path) {

        /*
            TotalTime = ChargingTime + MovingTime
            Distance = ChargingTime(=speed) * MovingTime
            Solve for CT/MT
        */

        var input = ParseInput1(path);

        int total = 1;
        foreach(var (time, distance) in input) {
            
            double sqrtRes = Math.Sqrt(Math.Pow(time, 2) - 4 * (distance+1));
            int minTime = (int)Math.Ceiling((time - sqrtRes)/2);
            int maxTime = (int)Math.Floor((time + sqrtRes)/2);
            int diffWays = maxTime - minTime + 1;
            total*=diffWays;
        }

        return total;
    }

    public static Race1[] ParseInput1(string path) {
        var inputLines = File.ReadAllLines(path);
         
        var nums = 
            inputLines
            .Select(
                l => l
                    .Split(" ")
                    .Skip(1)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => int.Parse(x.Trim())))
            .ToArray();

        return nums[0].Zip(nums[1]).Select(x=> new Race1(x.First, x.Second)).ToArray();
    }

    public static int Sol2(string path) {

        var (time, distance) = ParseInput2(path);

        double sqrtRes = Math.Sqrt(Math.Pow(time, 2) - 4 * (distance+1));
        int minTime = (int)Math.Ceiling((time - sqrtRes)/2);
        int maxTime = (int)Math.Floor((time + sqrtRes)/2);
        int diffWays = maxTime - minTime + 1;
        
        return diffWays;
    }

    public static Race2 ParseInput2(string path) {
        var inputLines = File.ReadAllLines(path);
         
        var nums = 
            inputLines
            .Select(
                l => l
                .Split(":")
                .Skip(1)
                .Select(
                    l => ulong.Parse(string.Concat(l.Where(x => x!=' ')))).First()).ToArray();

        return new Race2(nums[0], nums[1]);
    }

}
