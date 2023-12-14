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
        System.Console.WriteLine(Sol1(path));
        // System.Console.WriteLine(Sol2(path));
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
        var input = ParseInput(path);
        return 0;
    }
}
