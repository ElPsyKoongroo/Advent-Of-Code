using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode;
public static class Program
{
    public static void Main(string[] args)
    {
        Day_14 day = new();
        day.Answer2();
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

        public override bool Equals(object obj)
        {
            return (X == (obj as Coord)!.X && Y == (obj as Coord)!.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }
    }
    public class CoordComparer : IEqualityComparer<Coord>
    {
        public bool Equals(Coord? x, Coord? y)
        {
            return (x.X == y.X && x.Y == y.Y);
        }

        public int GetHashCode([DisallowNull] Coord obj)
        {
            return obj.X.GetHashCode() + obj.Y.GetHashCode();
        }
    }


    string[] testLines;
    string[] inputLines;

    public Day_14()
    {
        testLines = File.ReadAllLines("../AOCtest");
        inputLines = File.ReadAllLines("../AOCinput");
    }

    public HashSet<Coord> MakeCave(bool test = false)
    {
        var input = test ? testLines : inputLines;
        
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

        HashSet<Coord> cave = new(new CoordComparer());

        foreach (var line in inputCoords)
        {
            Coord actual = line[0];
            for(int i = 1; i < line.Count(); i++)
            {
                if(line[i].X == actual.X)
                {
                    int diff = line[i].Y - actual.Y;
                    int dir;
                    if(diff > 0)
                        dir = 1;
                    else
                        dir = -1;
                    
                    for(int j = 0; Math.Abs(j) <= Math.Abs(diff); j+=dir)
                    {
                        cave.Add(new Coord(actual.X, actual.Y+j));
                    }
                    actual = line[i];
                }
                else
                {
                    int diff = line[i].X - actual.X;
                    int dir;
                    if(diff > 0)
                        dir = 1;
                    else
                        dir = -1;
                    
                    for(int j = 0; Math.Abs(j) <= Math.Abs(diff); j+=dir)
                    {
                        cave.Add(new Coord(actual.X+j, actual.Y));
                    }
                    actual = line[i];
                }
            }
        }

        return cave;
    }

    public void Answer1()
    {
        const int x_source = 500, y_source = 0;

        var cave = MakeCave();

        int minx = cave.MinBy(x=> x.X)!.X;
        int maxx = cave.MaxBy(x=> x.X)!.X;
        int maxy = cave.MaxBy(x=> x.Y)!.Y;

        int totalSand = 0;
        while(true)
        {
            Coord actual = new Coord(x_source, y_source);
            bool end = false;

            while(true)
            {
                if(actual.X < minx || actual.X > maxx || actual.Y == maxy)
                {
                    end = true;
                    break;
                }
                if(!cave.Contains(new Coord(actual.X, actual.Y+1)))
                {
                    actual.Y++;
                    continue;
                }
                if(!cave.Contains(new Coord(actual.X-1, actual.Y+1)))
                {
                    actual.Y++;
                    actual.X--;
                    continue;
                }
                if(!cave.Contains(new Coord(actual.X+1, actual.Y+1)))
                {
                    actual.Y++;
                    actual.X++;
                    continue;
                }
                cave.Add(actual);
                break;
            }
            if(end)
                break;
            totalSand++;
        }

        Console.WriteLine(totalSand);
    }

    public void Answer2()
    {
        const int x_source = 500, y_source = 0;

        var cave = MakeCave();

        int floor = cave.MaxBy(x=> x.Y)!.Y + 2;

        int totalSand = 0;
        while(true)
        {
            Coord actual = new Coord(x_source, y_source);
            bool end = false;

            while(true)
            {
                if(actual.Y+1 == floor)
                {
                    cave.Add(actual);
                    break;
                }
                if(!cave.Contains(new Coord(actual.X, actual.Y+1)))
                {
                    actual.Y++;
                    continue;
                }
                if(!cave.Contains(new Coord(actual.X-1, actual.Y+1)))
                {
                    actual.Y++;
                    actual.X--;
                    continue;
                }
                if(!cave.Contains(new Coord(actual.X+1, actual.Y+1)))
                {
                    actual.Y++;
                    actual.X++;
                    continue;
                }
                if(actual.X == x_source && actual.Y == y_source)
                {
                    end = true;
                }
                cave.Add(actual);
                break;
            }
            totalSand++;
            if(end)
                break;
        }

        Console.WriteLine(totalSand);
    }
}