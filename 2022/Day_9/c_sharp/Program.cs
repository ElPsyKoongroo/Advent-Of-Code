using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode;

public static class Program
{
    public static void Main()
    {
        Day_9 day = new();
        day.Answer1();
        //day.Answer2();
    }
}



public class Day_9
{
    string[] inputLines;
    string[] testLines;

    private class PuntitoComparer : IEqualityComparer<Puntito>
    {
        public bool Equals(Puntito? x, Puntito? y)
        {
            return x.Equals(y);
        }

        public int GetHashCode([DisallowNull] Puntito obj)
        {
            return obj.x + obj.y;
        }
    }

    private class Puntito
    {
        public int x;
        public int y;

        public Puntito(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public Puntito(Puntito p)
        {
            x = p.x;
            y = p.y;
        }

        public override string ToString()
        {
            return $"X: {x}, Y: {y}";
        }

        public override bool Equals(object? obj)
        {
            if(obj is Puntito p)
            {
                if(p.x != x) return false;
                if(p.y != y) return false;
                return true;
            }
            return false;
        }

        public static bool operator == (Puntito tis, Puntito p)
        {
            return tis.Equals(p);
        }

        public static bool operator != (Puntito tis, Puntito p)
        {
            return !tis.Equals(p);
        }

    }

    public Day_9()
    {
        inputLines = File.ReadAllLines("../AOCinput");
        testLines = File.ReadAllLines("../AOCtest");
    }

    public void Answer1()
    {
        HashSet<Puntito> visited = new(new PuntitoComparer());

        Puntito head = new(0, 0);
        Puntito tail = new(0, 0);

        visited.Add(new Puntito(0, 0));

        foreach(var line in inputLines)
        {
            string direction = line.Split(" ")[0];
            int count = int.Parse(line.Split(" ")[1]);

            for(int step = 0; step < count; ++step)
            {
                Puntito prevHead = new(head);
                
                head.x += direction switch 
                {
                    "R" => 1,
                    "L" => -1,
                    _ => 0
                };

                head.y += direction switch 
                {
                    "U" => 1,
                    "D" => -1,
                    _ => 0
                };

                if(Math.Abs(head.x - tail.x) == 2 || Math.Abs(head.y - tail.y) == 2)
                {
                    tail = new(prevHead);
                    visited.Add(new Puntito(tail));
                }
            }
        }

        System.Console.WriteLine(visited.Count);
    }

    public void Answer2()
    {
        
    }
}