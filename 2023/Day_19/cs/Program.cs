
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
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

record Part(int X, int M, int A, int S) {
    private static readonly Regex r = new(@"{x=(\d+),m=(\d+),a=(\d+),s=(\d+)}");
    public Part(string line):this(0,0,0,0) {
        var match = r.Match(line);
        X = int.Parse(match.Groups[1].Value);
        M = int.Parse(match.Groups[2].Value);
        A = int.Parse(match.Groups[3].Value);
        S = int.Parse(match.Groups[4].Value);
    }
};

record Condition(string Var, char Comparer, int Num, string NextName) {
    public Condition() : this("", '_', 0, "") {

    }
};

record Workflow(string Name, (string, string, int)[] Conditions);

class Program
{
    static void Main(string[] args)
    {
        string path = args.FirstOrDefault("../AOCtest");
        Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }

    static (Workflow[], Part[]) ParseInput(string path) {
        return (null, null);
    }

    static long Sol1(string path)
    {
        return -1;
    }

    static long Sol2(string path)
    {
        return -1;
    }
}
