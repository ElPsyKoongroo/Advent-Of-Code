namespace AdventOfCode;
public class Program
{
    public static void Main(string[] args)
    {
        Day_01 day = new();
        day.Answer2();
    }
}

public class Day_01
{
    string inputText;
    public Day_01()
    {
        inputText = File.ReadAllText("../AOCinput");
    }

    public void Answer1()
    {
        var inputs = inputText
            .Split(",")
            .Select(x=> new {D = x.Trim()[0], L = int.Parse(x.Trim().Substring(1))})
            .ToList();

        int x = 0, y = 0;

        var dirs = new[]
        {
            new{X = 0, Y = 1},
            new{X = 1, Y = 0},
            new{X = 0, Y = -1},
            new{X = -1, Y = 0}
        };

        int actualDir = 0;

        foreach (var input in inputs)
        {
            if(input.D == 'L')
                actualDir = actualDir == 0 ? 3 : actualDir-1;
            else
                actualDir = actualDir == 3 ? 0 : actualDir+1;
            
            x += dirs[actualDir].X * input.L;
            y += dirs[actualDir].Y * input.L;
        }

        int distance = Math.Abs(x) + Math.Abs(y);

        System.Console.WriteLine(distance);
    }

    public void Answer2()
    {
        var inputs = inputText
            .Split(",")
            .Select(x=> new {D = x.Trim()[0], L = int.Parse(x.Trim().Substring(1))})
            .ToList();

        int x = 0, y = 0;

        var dirs = new[]
        {
            new{X = 0, Y = 1},
            new{X = 1, Y = 0},
            new{X = 0, Y = -1},
            new{X = -1, Y = 0}
        };

        int actualDir = 0;

        HashSet<KeyValuePair<int, int>> visited = new();

        foreach (var input in inputs)
        {
            if(input.D == 'L')
                actualDir = actualDir == 0 ? 3 : actualDir-1;
            else
                actualDir = actualDir == 3 ? 0 : actualDir+1;
            
            for(int i = 0;i < input.L; ++i)
            {
                x += dirs[actualDir].X;
                y += dirs[actualDir].Y;

                if(!visited.Add(new KeyValuePair<int, int>(x,y)))
                {
                    int distance = Math.Abs(x) + Math.Abs(y);
                    System.Console.WriteLine(distance);
                    return;
                }
            }
        }
    }
}