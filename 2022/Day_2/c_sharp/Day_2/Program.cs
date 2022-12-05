namespace Program;

public static class Program
{
    public static void Main()
    {
        Day_2 answer = new();
        answer.Answer2();
    }
}

public class Day_2
{

    /*
    A, X = Rock
    B, Y = Paper
    C, Z = Scissors
    
    */
    string[] inputLines;
    public Day_2()
    {
        inputLines = File.ReadAllLines("../../AOCinput");
    }
    public void Answer1()
    {
        int total = 0;

        foreach(var line in inputLines)
        {
            var input = line
                .Split(" ")
                .Select(x=> char.Parse(x) - 'A')
                .Select(x=> x < 3 ? x : x-23)
                .ToArray();
            total += GetScoreAnswer1(input);
        }
        System.Console.WriteLine(total);
    }
    public void Answer2()
    {
        int total = 0;

        foreach(var line in inputLines)
        {
            var input = line
                .Split(" ")
                .Select(x=> char.Parse(x) - 'A')
                .Select(x=> x < 3 ? x : x-23)
                .ToArray();
            total += GetScoreAnswer2(input);
        }
        System.Console.WriteLine(total);
    }

    private int Next(int num)
    {
        return (num+1)%3;
    }

    private int Previous(int num)
    {
        return num == 0 ? 2 : num-1;
    }
    private int GetScoreAnswer1(int[] input) => (input[0], input[1]) switch
    {
        (int a, int b) when a == Next(b)    => 0 + b+1,
        (int a, int b) when a == b          => 3 + b+1,
        (int a, int b) when Next(a) == b    => 6 + b+1,
        _ => 0
    };
     private int GetScoreAnswer2(int[] input) => (input[0], input[1]) switch
    {
        (int a, int b) when b == 0 => Previous(a) + 1 + 0,
        (int a, int b) when b == 1 => a + 1 + 3,
        (int a, int b) when b == 2 => Next(a) + 1 + 6,
        _ => 0
    };

}