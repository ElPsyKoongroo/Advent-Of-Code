using System.Collections.Immutable;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
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


    static long Sol1BruteForce(string path) {

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

    static int Fill(int x, int y, int totalSteps, char[][] input) {
        (int X, int Y)[] dirs = [
            (0,1),
            (0,-1),
            (1,0),
            (-1,0)
        ];
        int maxX = input[0].Length;
        int maxY = input.Length;

        HashSet<(int X, int Y)> answer = [];
        HashSet<(int X, int Y)> seen = [(x, y)];

        Queue<(int x, int Y, int Steps)> q = [];
        q.Enqueue((x, y, totalSteps)); 

        while(q.TryDequeue(out var act)) {
            var (r,c,s) = act;

            if(s % 2 == 0) {
                answer.Add((r,c));
            }
            if(s == 0) continue;

            foreach(var (dX, dY) in dirs) {
                var (nX, nY) = (r+dX, c+dY);
                if(nX < 0 || nX >= maxX || nY < 0 || nY >= maxY || input[nY][nX] != '.' || seen.Contains((nX, nY))) 
                    continue;
                
                seen.Add((nX, nY));
                q.Enqueue((nX, nY, s-1));
                
            }
        
        }

        return answer.Count;
    }

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
        
        return Fill(iniX, iniY, 64, input);
    }

    static long Sol2(string path) {
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
        
        int steps = 26501365;
        int size = input.Length;

        int grid_width = steps / size - 1;
        long odd = (long)Math.Pow(grid_width / 2 * 2 + 1,2);
        long even = (long)Math.Pow((grid_width+1) / 2 * 2,2);
        //even = ((grid_width + 1) // 2 * 2) ** 2


        long odd_points = Fill(iniX, iniY, size*2+1, input);
        long even_points = Fill(iniX, iniY, size*2, input);

        long corner_t = Fill(size-1, iniY, size-1, input);
        long corner_r = Fill(iniX, 0, size-1, input);
        long corner_b = Fill(0, iniY, size-1, input);
        long corner_l = Fill(iniX, size-1, size-1, input);

        long small_tr = Fill(size-1, 0, size/2-1, input);
        long small_tl = Fill(size-1, size-1, size/2-1, input);
        long small_br = Fill(0, 0, size/2-1, input);
        long small_bl = Fill(0, size-1, size/2-1, input);

        long large_tr = Fill(size-1, 0, size*3/2-1, input);
        long large_tl = Fill(size-1, size-1, size*3/2-1, input);
        long large_br = Fill(0, 0, size*3/2-1, input);
        long large_bl = Fill(0, size-1, size*3/2-1, input);

        long result =
            odd * odd_points +
            even * even_points +
            corner_t + corner_r + corner_b + corner_l +
            (grid_width+1) * (small_bl+small_br+small_tl+small_tr) +
            grid_width * (large_bl+large_br+large_tl+large_tr);

        return result;
    }
}
