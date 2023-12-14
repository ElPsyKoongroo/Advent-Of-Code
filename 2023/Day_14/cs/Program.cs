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

    public static IEnumerable<string[]> SplitEmptyLines(this string[] arr) {
        int act = 0;
        bool next = false;
        while(act < arr.Length) {
            for(int i = act; i < arr.Length; ++i) {
                if(string.IsNullOrEmpty(arr[i])) {
                    yield return arr[act..i];
                    act = i+1;
                    next = true;
                    break;
                }
            }
            if(!next) {
                yield return arr[act..];
                break;
            }
            next = false;
        }
        yield break;
    }

}

class Program
{
    static void Main(string[] args)
    {
        string path = args.FirstOrDefault("../AOCinput");
        // System.Console.WriteLine(Sol1(path));
        System.Console.WriteLine(Sol2(path));
    }

    static char[][] ParseInput(string path) =>
        File.ReadAllLines(path)
        .Select(x=> x.ToCharArray())
        .ToArray();

    static int Sol1(string path) {
        var input = ParseInput(path);

        for(int i = 1; i < input.Length; ++i) {
            for(int j = 0; j < input[0].Length; ++j) {
                if(input[i][j] != 'O') continue;
                int actualI = i;
                while(actualI-1 >= 0 && input[actualI-1][j] == '.') {
                    input[actualI-1][j] = 'O';
                    input[actualI][j] = '.';
                    actualI--;
                }
            }
        }

        // for(int i = 0; i < input.Length; ++i) {
        //     for(int j = 0; j < input[0].Length; ++j) {
        //         Console.Write(input[i][j]);
        //     }
        //     Console.WriteLine();
        // }


        int total = 0;
        var totalRows = input.Length;
        for(int i = 0; i < totalRows; ++i) {
            total += input[i].Count(x=> x=='O') * (totalRows-i);
        }

        return total;
    }
    static int Sol2(string path) {
        var actualCycle = ParseInput(path);

        List<char[][]> cycles = [];

        TiltNorth(actualCycle);
        TiltWest(actualCycle);
        TiltSourth(actualCycle);
        TiltEast(actualCycle);

        while(!cycles.Any(c=> PlatformEqual(c, actualCycle))) {
            cycles.Add(actualCycle.Select(x=> x.ToArray()).ToArray());

            TiltNorth(actualCycle);
            TiltWest(actualCycle);
            TiltSourth(actualCycle);
            TiltEast(actualCycle);
        }

        int untilCycle = 0, sizeCycle = 0;
        for(int i = 0; i < cycles.Count - 1; ++i) {
            if(PlatformEqual(cycles[i], actualCycle)) {
                untilCycle = i+1;
                sizeCycle = cycles.Count - i;
                break;
            }
        }
        int totalCycles = 1_000_000_000;
        int finalCycleIndex = ((totalCycles - untilCycle)%sizeCycle)+untilCycle-1;
        var finalCycle = cycles[finalCycleIndex];

        int total = 0;
        var totalRows = finalCycle.Length;
        for(int i = 0; i < totalRows; ++i) {
            total += finalCycle[i].Count(x=> x=='O') * (totalRows-i);
        }

        return total;
    }

    static bool PlatformEqual(char[][] p1, char[][] p2) {
        for(int i = 0; i < p1.Length; ++i) {
            for(int j = 0; j < p1[0].Length; ++j) {
                if(p1[i][j] != p2[i][j]) return false;
            }
        }
        return true;
    }

    static void TiltNorth(char[][] input) {
        for(int i = 1; i < input.Length; ++i) {
            for(int j = 0; j < input[0].Length; ++j) {
                if(input[i][j] != 'O') continue;
                int actualI = i;
                while(actualI-1 >= 0 && input[actualI-1][j] == '.') {
                    input[actualI-1][j] = 'O';
                    input[actualI][j] = '.';
                    actualI--;
                }
            }
        }
    }

    static void TiltSourth(char[][] input) {
        for(int i = input.Length-2; i >= 0; --i) {
            for(int j = 0; j < input[0].Length; ++j) {
                if(input[i][j] != 'O') continue;
                int actualI = i;
                while(actualI+1 < input.Length && input[actualI+1][j] == '.') {
                    input[actualI+1][j] = 'O';
                    input[actualI][j] = '.';
                    actualI++;
                }
            }
        }
    }

    static void TiltWest(char[][] input) {
        for(int j = 1; j < input[0].Length; ++j) {
            for(int i = 0; i < input.Length; ++i) {
                if(input[i][j] != 'O') continue;
                int actualJ = j;
                while(actualJ-1 >= 0 && input[i][actualJ-1] == '.') {
                    input[i][actualJ-1] = 'O';
                    input[i][actualJ] = '.';
                    actualJ--;
                }
            }
        }
    }

    static void TiltEast(char[][] input) {
        for(int j = input[0].Length-2; j >= 0; --j) {
            for(int i = 0; i < input.Length; ++i) {
                if(input[i][j] != 'O') continue;
                int actualJ = j;
                while(actualJ+1 < input[0].Length && input[i][actualJ+1] == '.') {
                    input[i][actualJ+1] = 'O';
                    input[i][actualJ] = '.';
                    actualJ++;
                }
            }
        }
    }

}
