using System.Diagnostics;

namespace cs;

public record struct State((int X, int Y) Pos, (int X, int Y) Dir, int dirTimes);

class Program
{
    static void Main(string[] args)
    {
        string path = args.FirstOrDefault("../AOCinput");
        var ini = Stopwatch.GetTimestamp();
        
        // Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));

        var fin = Stopwatch.GetTimestamp();

        System.Console.WriteLine(Stopwatch.GetElapsedTime(ini, fin));
    }

    static int[][] ParseInput(string path) =>
        File.ReadAllLines(path)
        .Select(l=> l.Select(x => int.Parse(x.ToString())).ToArray())
        .ToArray();

    static int Sol1(string path)
    {
        var input = ParseInput(path);

        var totalCols = input[0].Length;
        var totalRows = input.Length;

        (int x, int y) objetive = (totalCols-1, totalRows-1);

        (int x, int y)[] dirs = [
            ( 0,-1),
            ( 0, 1),
            (-1, 0),
            ( 1, 0),
        ];
        List<State> closed = [];
        PriorityQueue<State, int> open = new();
        open.Enqueue(new State((0,0),(0,0),0), 0);

        int finalCost = 0;


        while(open.TryDequeue(out var actual, out int cost)) {

            if(closed.Contains(actual)) continue;
            closed.Add(actual);

            if(actual.Pos == objetive) {
                finalCost = cost;
                break;
            }


            foreach(var (X, Y) in dirs) {
                if((X,Y) == actual.Dir && actual.dirTimes >= 3) {
                    continue;
                }
                if((-X, -Y) == actual.Dir) continue;

                int nextY = actual.Pos.Y + Y;
                int nextX = actual.Pos.X + X;
                if(nextX < 0 || nextX >= totalCols || nextY < 0 || nextY >= totalRows) continue;
                int dirTimes = (X,Y) == actual.Dir ? actual.dirTimes+1 : 1;
                var next = new State((nextX, nextY), (X,Y), dirTimes);
                open.Enqueue(next, cost+input[nextY][nextX]);
            }
        }

        return finalCost;
    }

    static int Sol2(string path)
    {
        var input = ParseInput(path);

        var totalCols = input[0].Length;
        var totalRows = input.Length;

        (int x, int y) objetive = (totalCols-1, totalRows-1);

        (int x, int y)[] dirs = [
            ( 0,-1),
            ( 0, 1),
            (-1, 0),
            ( 1, 0),
        ];
        List<State> closed = [];
        PriorityQueue<State, int> open = new();
        open.Enqueue(new State((0,0),(0,1),0), 0);
        open.Enqueue(new State((0,0),(1,0),0), 0);

        int finalCost = 0;


        while(open.TryDequeue(out var actual, out int cost)) {

            if(closed.Contains(actual)) continue;
            closed.Add(actual);

            if(actual.Pos == objetive && actual.dirTimes >= 4) {
                finalCost = cost;
                break;
            }

            foreach(var (X, Y) in dirs) {
                if((X,Y) == actual.Dir && actual.dirTimes >= 10) {
                    continue;
                }
                if((X,Y) != actual.Dir && actual.dirTimes < 4) {
                    continue;
                }
                if((-X, -Y) == actual.Dir) continue;

                int nextY = actual.Pos.Y + Y;
                int nextX = actual.Pos.X + X;
                if(nextX < 0 || nextX >= totalCols || nextY < 0 || nextY >= totalRows) continue;
                int dirTimes = (X,Y) == actual.Dir ? actual.dirTimes+1 : 1;
                var next = new State((nextX, nextY), (X,Y), dirTimes);
                open.Enqueue(next, cost+input[nextY][nextX]);
            }
        }

        return finalCost;
    }
}
