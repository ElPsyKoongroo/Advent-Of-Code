namespace AdventOfCode;
public static class Program
{
    public static void Main(string[] args)
    {
        Day_14 day = new();
        day.Answer1();
    }
}

public class Day_14
{
    public class Coord
    {
        public int X ,Y;
        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Coord()
        {
            X = 0;
            Y = 0;
        }
    } 


    string[] testLines;
    string[] inputLines;

    public Day_14()
    {
        testLines = File.ReadAllLines("../AOCtest");
        inputLines = File.ReadAllLines("../AOCinput");
    }

    public void MakeCave(bool test = false)
    {
        string[] input = test ? testLines : inputLines;
        
        List<Coord[]> inputCoords = 
            input
            .Select(
                line=> line
                .Split("->")
                .Select(x=>
                {
                    var ints = x.Trim().Split(",").Select(y=> int.Parse(y)).ToArray();
                    return new Coord(ints[0], ints[1]);
                }).ToArray()
            ).ToList();
        
        int minx = inputCoords.SelectMany(x=>x).MinBy(x=> x.X)!.X;
        int miny = inputCoords.SelectMany(x=>x).MinBy(x=> x.Y)!.Y;
        int maxx = inputCoords.SelectMany(x=>x).MaxBy(x=> x.X)!.X;
        int maxy = inputCoords.SelectMany(x=>x).MaxBy(x=> x.Y)!.Y;

        System.Console.WriteLine($"Minx: {minx}, miny: {miny}");
        System.Console.WriteLine($"Maxx: {maxx}, maxy: {maxy}");
    }

    public void Answer1()
    {
        MakeCave(true);
    }

    public void Answer2()
    {
        
    }
}