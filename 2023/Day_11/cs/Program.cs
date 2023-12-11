using System.Net;

namespace cs;

class Program
{
    static void Main(string[] args)
    {
        var path = args.FirstOrDefault("../AOCinput");
        // Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }

    static bool[][] ParseInput(string path) {
        #if DEBUG
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
        path = Path.Combine(projectDirectory,path);
        #endif
        return
            File.ReadAllLines(path)
            .Select(l=>
                l.Select(c=> c=='#').ToArray()
            ).ToArray();
    }

    static IEnumerable<(int y, int x)> GetGalaxies(bool[][] input) {
        for(int i = 0; i < input.Length; ++i) {
            for(int j = 0; j < input[0].Length; ++j) {
                if(input[i][j]) yield return (i,j);
            }
        }
        yield break;
    }

    static int DistanceManhatthan((int y, int x) g1, (int y, int x) g2) {
        return Math.Abs(g1.x-g2.x) + Math.Abs(g1.y-g2.y);
    }

    static int Sol1(string path) {
        var input = ParseInput(path);

        List<int> freeRows = [];

        for(int i = 0; i < input.Length; ++i) {
            bool empty = true;
            for(int j = 0; j < input[0].Length && empty; ++j) {
                empty = !input[i][j];
            }
            if(empty) freeRows.Add(i);
        }

        List<int> freeColumns = [];
        for(int j = 0; j < input[0].Length; ++j) {
            bool empty = true;
            for(int i = 0; i < input.Length && empty; ++i) {
                empty = !input[i][j];
            }
            if(empty) freeColumns.Add(j);
        }

        var galaxies = GetGalaxies(input).ToArray();

        int total = 0;
        for(int i = 0; i < galaxies.Length-1; ++i) {
            var g1 = galaxies[i];
            for(int j = i+1; j < galaxies.Length; ++j) {
                var g2 = galaxies[j];
                var distance = DistanceManhatthan(g1, g2);
                var rowExpansion =
                    freeRows
                    .Where(y=> y >= Math.Min(g1.y,g2.y) && y <= Math.Max(g1.y,g2.y))
                    .Count();
                var colExpansion =
                    freeColumns
                    .Where(x=> x >= Math.Min(g1.x,g2.x) && x <= Math.Max(g1.x,g2.x))
                    .Count();
                    
                distance+= colExpansion + rowExpansion;
                    
                total+=distance;
                // System.Console.WriteLine($"{i+1}-{j+1}={distance}");
                // System.Console.WriteLine(++cont);
            }
        }
        return total;
    }

    static long Sol2(string path) {
        var input = ParseInput(path);

        List<int> freeRows = [];
        for(int i = 0; i < input.Length; ++i) {
            bool empty = true;
            for(int j = 0; j < input[0].Length && empty; ++j) {
                empty = !input[i][j];
            }
            if(empty) freeRows.Add(i);
        }

        List<int> freeColumns = [];
        for(int j = 0; j < input[0].Length; ++j) {
            bool empty = true;
            for(int i = 0; i < input.Length && empty; ++i) {
                empty = !input[i][j];
            }
            if(empty) freeColumns.Add(j);
        }

        var galaxies = GetGalaxies(input).ToArray();

        int separation = 1_000_000;

        long total = 0;
        for(int i = 0; i < galaxies.Length-1; ++i) {
            var g1 = galaxies[i];
            for(int j = i+1; j < galaxies.Length; ++j) {
                var g2 = galaxies[j];
                long distance = DistanceManhatthan(g1, g2);
                long rowExpansion =
                    freeRows
                    .Where(y=> y >= Math.Min(g1.y,g2.y) && y <= Math.Max(g1.y,g2.y))
                    .Count()*(separation-1);
                long colExpansion =
                    freeColumns
                    .Where(x=> x >= Math.Min(g1.x,g2.x) && x <= Math.Max(g1.x,g2.x))
                    .Count()*(separation-1);
                    
                distance+= colExpansion + rowExpansion;
                    
                total+=distance;
                // System.Console.WriteLine($"{i+1}-{j+1}={distance}");
                // System.Console.WriteLine(++cont);
            }
        }
        return total;
    }
}
