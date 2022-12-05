namespace AdventOfCode;

public static class Program
{
    public static void Main()
    {
        Day_5 day = new();

        System.Console.WriteLine(day.Answer1());
    }
}

public class Day_5
{
    string input;

    public Day_5()
    {
        input = File.ReadAllText("../AOCinput");
    }

    public string Answer1()
    {
        return "";
    }

    public string Answer2()
    {
        return "";
    }
}