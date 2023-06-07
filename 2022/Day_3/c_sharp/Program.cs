namespace Program;

public static class Program
{
    public static void Main()
    {
        Day_3 day = new();
        day.Answer2();
    }
}

public class Day_3
{
    string[] inputLines;
    public Day_3()
    {
        inputLines  = File.ReadAllLines("../AOCinput");
    }

    public void Answer1()
    {
        int sum = 0;

        foreach(var line in inputLines)
        {
            var line0 = line.Substring(0, line.Length/2);
            var line1 = line.Substring(line.Length/2);


            var badSnack = line0.First(x=> line1.Count(y=> y == x) > 0);              

            int badSnackInt = badSnack >= 'a' ? badSnack - 'a' + 1 : badSnack - 'A' + 27;
            sum += badSnackInt;
        }

        System.Console.WriteLine(sum);
    }

    public void Answer2()
    {
        int sum = 0;

        var groupedLines = inputLines.Chunk(3).ToList();

        foreach(var lines in groupedLines)
        {
            var badSnack = lines[0]
                .First(
                    x=>    
                    lines[1].Count(y=> y == x) > 0 &&
                    lines[2].Count(y=> y == x) > 0
                    );              

            int badSnackInt = badSnack >= 'a' ? badSnack - 'a' + 1 : badSnack - 'A' + 27;
            sum += badSnackInt;
        }

        System.Console.WriteLine(sum);
    }
}