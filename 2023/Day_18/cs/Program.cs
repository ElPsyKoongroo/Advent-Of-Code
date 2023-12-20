
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace cs;

record Instruction(char Direction, int Amountm);

class Program
{
    static readonly Regex regex = new(@"(\w) (\d+) \(#(\w+)\)", RegexOptions.Compiled);
    static void Main(string[] args)
    {
        string path = args.FirstOrDefault("../AOctest");
        // Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }

    static Instruction[] ParseInput(string path) =>
        File.ReadAllLines(path)
        .Select(l => {
            var m =regex.Match(l);
            var d = m.Groups[1].Value;
            var a = m.Groups[2].Value;
            return new Instruction(d[0], int.Parse(a));
        }).ToArray();

    static bool IsInside(HashSet<(int X, int Y)> trench, (int X, int Y) point) {
        var rightOf = trench.Where(x=> x.Y == point.Y && x.X > point.X).OrderBy(x=> x.X).ToArray();
        var amoLeft = rightOf.Length;
        if(amoLeft == 0) return false;
        if(amoLeft == 1) return true;

        int hits = 0;

        bool isVertical((int X, int Y) p) =>
            trench.Contains((p.X, p.Y + 1)) && trench.Contains((p.X, p.Y - 1));
        bool isHorizontal((int X, int Y) p) =>
            !trench.Contains((p.X, p.Y + 1)) && !trench.Contains((p.X, p.Y - 1));

        for (int i = 0; i < rightOf.Length; ++i) {
            if(isVertical(rightOf[i])) {
                ++hits;
                continue;
            }

            var down = trench.Contains((rightOf[i].X, rightOf[i].Y-1));
            ++i;

            while(isHorizontal(rightOf[i])) {++i;}
            if(trench.Contains((rightOf[i].X, rightOf[i].Y-1)) != down) {
                ++hits;
            }
        }
        return hits % 2 == 1;
    }

    static void FillTrench(HashSet<(int X, int Y)> trench, (int X, int Y) point) {
        Queue<(int X, int Y)> queue = [];
        queue.Enqueue(point);

        while(queue.TryDequeue(out var p)) {
            if(!trench.Add(p)) continue;

            (int X, int Y)[] dirs = [(0,1), (0,-1), (1,0), (-1,0)];
            foreach(var (X, Y) in dirs) {
                var nX = p.X+X;
                var nY = p.Y+Y;

                if(!trench.Contains((nX, nY))) queue.Enqueue((nX, nY));
            }
        }
    }

    static HashSet<(int X, int Y)> GenerateTrench(Instruction[] input) {
        HashSet<(int X, int Y)> trench = [];

        (int X, int Y) act = (0,0);

        foreach(var (d, c) in input) {
            (int X, int Y) = d switch {
                'R' => (1, 0),
                'L' => (-1, 0),
                'U' => (0, 1),
                'D' => (0, -1),
                _ => throw new Exception("Pincho")
            };
            int i = 0;
            do {
                trench.Add(act);
                act.X+= X;
                act.Y+= Y;
                ++i;
            } while(i < c);
        }

        int minX = trench.MinBy(x=> x.X).X;
        int minY = trench.MinBy(x=> x.Y).Y;
        int maxX = trench.MaxBy(x=> x.X).X;
        int maxY = trench.MaxBy(x=> x.Y).Y;

        (int X, int Y) posInicial = (0,0);
        bool foundStart = false;
        for(int i = -1; i <= 1 && !foundStart; ++i) {
            for(int j = -1; j <= 1 && !foundStart; ++j) {
                if(i == 0 && j == 0) continue;
                if(!trench.Contains((i,j)) && IsInside(trench, (i,j))) {
                    foundStart = true;
                    posInicial = (i,j);
                }
            }   
        }

        FillTrench(trench, posInicial);

        return trench;
    }

    static int Sol1(string path)
    {
        var input = ParseInput(path);

        var t = GenerateTrench(input);

        return t.Count;
    }

    static Instruction[] ParseInput2(string path) =>
        File.ReadAllLines(path)
        .Select(l => {
            var m =regex.Match(l);
            var c = m.Groups[3].Value;

            int num = int.Parse(c[..^1], System.Globalization.NumberStyles.HexNumber);

            char dir = c[^1] switch {
                '0' => 'R',
                '1' => 'D',
                '2' => 'L',
                '3' => 'U',
            };

            return new Instruction(dir, num);
        }).ToArray();

    static int Sol2(string path)
    {
        var input = ParseInput(path);



        return -1;
    }
}
