using System.Security.Cryptography;

namespace cs;


class Program
{
    static void Main(string[] args)
    {
        string path = args.FirstOrDefault("../AOCtest");
        Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }

    static (int x, int y, int z)[] ParseInput(string path) =>
        File.ReadAllLines(path)
        .Select(l => l.Split(",").Select(int.Parse).ToArray())
        .Select(x => (x[0],x[1],x[2]))
        .ToArray();

    static int Sol1(string path) {
        var input = ParseInput(path);

        var total = 0;
        foreach(var (x,y,z) in input) {
            if(!input.Contains((x+1,y,z))) total++;
            if(!input.Contains((x-1,y,z))) total++;
            if(!input.Contains((x,y+1,z))) total++;
            if(!input.Contains((x,y-1,z))) total++;
            if(!input.Contains((x,y,z+1))) total++;
            if(!input.Contains((x,y,z-1))) total++;
        }
        return total;
    }

    static int Sol2(string path) {
        var input = ParseInput(path);

        int maxX = input.MaxBy(X=>X.x).x;
        int maxY = input.MaxBy(X=>X.y).y;
        int maxZ = input.MaxBy(X=>X.z).z;
        int minX = input.MinBy(X=>X.x).x;
        int minY = input.MinBy(X=>X.y).y;
        int minZ = input.MinBy(X=>X.z).z;

        // Console.WriteLine($"X {minX} - {maxX}");
        // Console.WriteLine($"Y {minY} - {maxY}");
        // Console.WriteLine($"Z {minZ} - {maxZ}");

        HashSet<(int x, int y, int z)> noInfo = [];

        for(int x = minX; x <= maxX; ++x) {
            for(int y = minY; y <= maxY; ++y) {
                for(int z = minZ; z <= maxZ; ++z) {
                    var p = (x,y,z);
                    if(!input.Contains(p)) noInfo.Add(p);
                }
            }
        }

        HashSet<(int x, int y, int z)> inside = [];
        HashSet<(int x, int y, int z)> outside = [];
        Queue<(int x, int y, int z)> actualQueue = [];
        List<(int x, int y, int z)> actualPath = [];

        

        return 1;
    }
}
