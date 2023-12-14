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
        var path = args.FirstOrDefault("../AOCinput");
        // Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }
    static int Sol1(string path) {
        var input = ParseInput(path);
        
        int columns = 0;
        int rows = 0;

        foreach (var section in input)
        {
            //Check row
            int foundRow = -1;
            for(int i = 1; i < section.Length; ++i) {
                bool found = true;
                int offset = 0;
                while(i-(offset+1) >= 0 && i+offset < section.Length && found) {
                    if(section[i+offset] != section[i-offset-1]) {
                        found = false;
                    }
                    offset++;
                }
                if(found) {
                    foundRow = i;
                    break;
                }
            }
            if(foundRow != -1) {
                // System.Console.WriteLine($"Row: {foundRow}");
                rows += foundRow;
                continue;
            }

            //Check row
            int foundCol = -1;
            for(int i = 1; i < section[0].Length; ++i) {
                bool found = true;
                int offset = 0;
                while(i-(offset+1) >= 0 && i+offset < section[0].Length && found) {
                    if(string.Concat(section.Select(x=> x[i+offset])) != 
                        string.Concat(section.Select(x=> x[i-offset-1]))
                        ) {
                        found = false;
                    }
                    offset++;
                }
                if(found) {
                    foundCol = i;
                    break;
                }
            }
            if(foundCol != -1) {
                // System.Console.WriteLine($"Col: {foundCol}");
                columns += foundCol;
                continue;
            }
            Console.WriteLine("Not found");
        }

        return columns + rows*100;
    }

    static string[][] ParseInput(string path) => 
        File.ReadAllLines(path)
        .SplitEmptyLines()
        .ToArray();

    static int Sol2(string path) {
                var input = ParseInput(path);
        
        int columns = 0;
        int rows = 0;

        foreach (var section in input)
        {
            //Check row
            bool badFound;
            int foundRow = -1;
            for(int i = 1; i < section.Length; ++i) {
                badFound = false;
                bool found = true;
                int offset = 0;
                while(i-(offset+1) >= 0 && i+offset < section.Length && found) {
                    var strDiff = CompareStr(section[i+offset], section[i-offset-1]);

                    if(strDiff > 1) found = false;
                    if(strDiff == 1) {
                        if(badFound) found = false;
                        else badFound = true;
                    }
                    offset++;
                }
                if(found && badFound) {
                    foundRow = i;
                    break;
                }
            }
            if(foundRow != -1) {
                rows += foundRow;
                continue;
            }

            //Check row
            int foundCol = -1;
            for(int i = 1; i < section[0].Length; ++i) {
                badFound = false;
                bool found = true;
                int offset = 0;
                while(i-(offset+1) >= 0 && i+offset < section[0].Length && found) {

                    var strDiff = CompareStr(
                        string.Concat(section.Select(x=> x[i+offset]))
                        , string.Concat(section.Select(x=> x[i-offset-1])));

                    if(strDiff > 1) found = false;
                    if(strDiff == 1) {
                        if(badFound) found = false;
                        else badFound = true;
                    }
                    offset++;
                }
                if(found && badFound) {
                    foundCol = i;
                    break;
                }
            }
            if(foundCol != -1) {
                columns += foundCol;
                continue;
            }
            Console.WriteLine("Not found");
        }

        return columns + rows*100;
    }

    static int CompareStr(string s1, string s2) {
        int diff = 0;
        for(int i = 0; i < s1.Length; ++i) {
            if(s1[i] != s2[i]) diff++;
        }
        return diff;
    }
}
