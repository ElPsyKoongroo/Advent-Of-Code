using System.Text;
using System.Text.RegularExpressions;

namespace cs;

class Program
{
    static Regex r = new("(one|two|three|four|five|six|seven|eight|nine|1|2|3|4|5|6|7|8|9)", RegexOptions.Compiled);
    static void Main(string[] args)
    {
        StringBuilder inp = new();
        string? linea;
        while ((linea = Console.ReadLine()) is not null)
        {
            inp.AppendLine(linea);
        }
        string input = inp.ToString()[..^1];
        Console.WriteLine(Sol1(input));
        Console.WriteLine(Sol2(input));
    }

    static string[] nums = 
    [
        "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
    ];

    static int Sol1(string input) => 
        input.Split("\n").Select(l => {
            var arr = l.Where(char.IsAsciiDigit).ToArray();
            return int.Parse($"{arr.First()}{arr.Last()}");
        }).Sum();

    static int Sol2(string input) =>
        input.Split("\n").Select(l => {
            Match matchObj = r.Match(l);
            var f = matchObj.Value;
            string e = f;
            while (matchObj.Success) {
                e = matchObj.Value;
                matchObj = r.Match(l, matchObj.Index + 1); 
            }
            return int.Parse($"{nums.AsSpan().IndexOf(f)%10}{nums.AsSpan().IndexOf(e)%10}");
        }).Sum();
}
