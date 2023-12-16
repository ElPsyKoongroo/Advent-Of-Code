
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography.X509Certificates;

namespace cs;

class Program
{
    static void Main(string[] args)
    {
        string path = args.FirstOrDefault("../AOCtest");
        // Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }

    public static char[][] ParseInput(string path) =>
        File.ReadAllLines(path)
        .Select(x=> x.ToCharArray())
        .ToArray();

    public static int Sol1(string path)
    {
        var input = ParseInput(path);

        var ini = Stopwatch.GetTimestamp();

        Queue<(int X, int Y, int dX, int dY)> beams = [];
        beams.Enqueue((-1,0,1,0));

        HashSet<(int X, int Y, int dX, int dY)> possitionsPassed = [];

        while(beams.TryDequeue(out var act)) {

            var newX = act.X+act.dX;
            var newY = act.Y+act.dY;

            if(newX < 0 || newX >= input[0].Length) continue;
            if(newY < 0 || newY >= input.Length) continue;

            switch(input[newY][newX]) {
                case '.': 
                case '-' when act.dX != 0:
                case '|' when act.dY != 0: {
                    var next = (newX, newY, act.dX, act.dY);
                    if(!possitionsPassed.Contains(next)) {
                        possitionsPassed.Add(next);
                        beams.Enqueue(next);
                    }
                    break;
                }
                case '/': {
                    var next = (newX, newY, -act.dY, -act.dX);
                    if(!possitionsPassed.Contains(next)) {
                        possitionsPassed.Add(next);
                        beams.Enqueue(next);
                    }
                    break;
                }
                case '\\': {
                    var next = (newX, newY, act.dY, act.dX);
                    if(!possitionsPassed.Contains(next)) {
                        possitionsPassed.Add(next);
                        beams.Enqueue(next);
                    }
                    break;
                }
                case '-': {
                    var next1 = (newX, newY, 1, 0);
                    var next2 = (newX, newY, -1, 0);
                    if(!possitionsPassed.Contains(next1)) {
                        possitionsPassed.Add(next1);
                        beams.Enqueue(next1);
                    }
                    if(!possitionsPassed.Contains(next2)) {
                        possitionsPassed.Add(next2);
                        beams.Enqueue(next2);
                    }
                    break;
                }
                case '|': {
                    var next1 = (newX, newY, 0, 1);
                    var next2 = (newX, newY, 0, -1);
                    if(!possitionsPassed.Contains(next1)) {
                        possitionsPassed.Add(next1);
                        beams.Enqueue(next1);
                    }
                    if(!possitionsPassed.Contains(next2)) {
                        possitionsPassed.Add(next2);
                        beams.Enqueue(next2);
                    }
                    break;
                }
            }
        }

        // for(int y = 0; y < input.Length; ++y) {
        //     for(int x = 0; x < input[0].Length; ++x) {
        //         if(possitionsPassed.Any(a=> (a.X, a.Y) == (x,y))) {
        //             Console.Write("#");
        //         } else {
        //             Console.Write(input[y][x]);
        //         }
        //     }
        //     Console.WriteLine();
        // }

        var fin = Stopwatch.GetTimestamp();

        Console.WriteLine(Stopwatch.GetElapsedTime(ini, fin).Duration());

        return possitionsPassed.Select(x=> (x.X, x.Y)).Distinct().Count();
    }
    public static int Sol2(string path)
    {
        var input = ParseInput(path);

        var ini = Stopwatch.GetTimestamp();

        var topRow = Enumerable.Range(0, input[0].Length).Select(x=> (x, -1, 0, 1)).ToArray();
        var bottomRow = Enumerable.Range(0, input[0].Length).Select(x=> (x, input.Length, 0, -1)).ToArray();
        
        var leftRow = Enumerable.Range(0, input.Length).Select(x=> (-1, x, 1, 0)).ToArray();
        var rightRow = Enumerable.Range(0, input.Length).Select(x=> (input[0].Length, x, -1, 0)).ToArray();

        int maxActual = 0;

        (int x,int y,int dx,int dy)[] inits = [..topRow, ..bottomRow, ..leftRow, ..rightRow];

        foreach(var init in inits) {

            Queue<(int X, int Y, int dX, int dY)> beams = [];
            beams.Enqueue((init.x,init.y,init.dx,init.dy));

            HashSet<(int X, int Y, int dX, int dY)> possitionsPassed = [];

            while(beams.TryDequeue(out var act)) {

                var newX = act.X+act.dX;
                var newY = act.Y+act.dY;

                if(newX < 0 || newX >= input[0].Length) continue;
                if(newY < 0 || newY >= input.Length) continue;

                switch(input[newY][newX]) {
                    case '.': 
                    case '-' when act.dX != 0:
                    case '|' when act.dY != 0: {
                        var next = (newX, newY, act.dX, act.dY);
                        if(!possitionsPassed.Contains(next)) {
                            possitionsPassed.Add(next);
                            beams.Enqueue(next);
                        }
                        break;
                    }
                    case '/': {
                        var next = (newX, newY, -act.dY, -act.dX);
                        if(!possitionsPassed.Contains(next)) {
                            possitionsPassed.Add(next);
                            beams.Enqueue(next);
                        }
                        break;
                    }
                    case '\\': {
                        var next = (newX, newY, act.dY, act.dX);
                        if(!possitionsPassed.Contains(next)) {
                            possitionsPassed.Add(next);
                            beams.Enqueue(next);
                        }
                        break;
                    }
                    case '-': {
                        var next1 = (newX, newY, 1, 0);
                        var next2 = (newX, newY, -1, 0);
                        if(!possitionsPassed.Contains(next1)) {
                            possitionsPassed.Add(next1);
                            beams.Enqueue(next1);
                        }
                        if(!possitionsPassed.Contains(next2)) {
                            possitionsPassed.Add(next2);
                            beams.Enqueue(next2);
                        }
                        break;
                    }
                    case '|': {
                        var next1 = (newX, newY, 0, 1);
                        var next2 = (newX, newY, 0, -1);
                        if(!possitionsPassed.Contains(next1)) {
                            possitionsPassed.Add(next1);
                            beams.Enqueue(next1);
                        }
                        if(!possitionsPassed.Contains(next2)) {
                            possitionsPassed.Add(next2);
                            beams.Enqueue(next2);
                        }
                        break;
                    }
                }
            }
            maxActual = Math.Max(maxActual, possitionsPassed.Select(x=> (x.X, x.Y)).Distinct().Count());
        }

        var fin = Stopwatch.GetTimestamp();

        Console.WriteLine(Stopwatch.GetElapsedTime(ini, fin).Duration());

        return maxActual;
    }

}
