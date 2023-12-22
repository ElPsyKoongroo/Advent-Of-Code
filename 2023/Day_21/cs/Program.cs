using System.Collections.Immutable;
using MoreLinq;
using Newtonsoft.Json;

namespace cs;

public static class Extensions {
    public static void Dump<T>(this T obj, bool indent = false)
    {
        Console.WriteLine(
        JsonConvert.SerializeObject(obj, indent ? Formatting.Indented : Formatting.None,
            new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore})
        );
    }
}

class Program
{
    static void Main(string[] args)
    {
        string path = args.FirstOrDefault("../AOCtest");
        Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }

    static char[][] ParseInput(string path) => 
        File.ReadAllLines(path)
        .Select(l => l.ToCharArray())
        .ToArray();

    static long Sol1(string path) {

        var input = ParseInput(path);

        int maxX = input[0].Length;
        int maxY = input.Length;

        bool found = false;
        int iniX = 0;
        int iniY = 0;

        for(int i = 0; i < maxY && !found; ++i) {
            for(int j = 0; j < maxX && !found; ++j) {
                if(input[i][j] == 'S') {
                    found = true;
                    input[i][j] = '.';
                    (iniX, iniY) = (j,i);
                }
            }   
        }

        int totalSteps = path.Contains("test") ? 6 : 64;

        HashSet<(int X, int Y)> positions = [];
        positions.Add((iniX,iniY));

        (int X, int Y)[] dirs = [
            (0,1),
            (0,-1),
            (1,0),
            (-1,0)
        ];

        for(int i = 0; i < totalSteps; ++i) {
            List<(int X, int Y)> actPositions = [.. positions];
            positions.Clear();
            
            foreach(var (X, Y) in actPositions) {
                foreach(var (dX, dY) in dirs) {
                    var (nX, nY) = (X+dX, Y+dY);
                    if(nX >= 0 && nX < maxX && nY >= 0 && nY < maxY && input[nY][nX] == '.') {
                        positions.Add((nX, nY));
                    }
                }
            }
        }

        return positions.Count;
    }

    static long Sol2(string path) {
        return -1; 
    }
}
