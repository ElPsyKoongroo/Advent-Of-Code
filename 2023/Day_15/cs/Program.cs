namespace cs;

public static class Extensions {
    public static (T, T) TakeTwo<T>(this T[] arr) {
        return (arr[0], arr[1]);
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


    static string[] ParseInput(string path) =>
        File.ReadAllText(path).Split(",");

    static int Hash(string s) =>
        s.Aggregate(0, (acc, c)=> (acc+c)*17%256);

    static int Sol1(string path) =>
        ParseInput(path)
        .Select(Hash)
        .Sum();

    static long Sol2(string path) {
        var input = ParseInput(path);

        Dictionary<int, List<(string s, int v)>> hashmap = [];

        for(int i = 0; i < 256; ++i)
            hashmap[i] = [];

        foreach(var label in input) {
            if(label[^1] == '-') {
                string s = label[0..^1];
                int hash = Hash(s);
                hashmap[hash].RemoveAll(x=> x.s == s);
            } else {
                var (s, valueS) = label.Split("=").ToArray().TakeTwo();
                int hash = Hash(s);
                int value = int.Parse(valueS);

                int idx;
                if((idx = hashmap[hash].FindIndex(x=> x.s == s)) != -1) {
                    hashmap[hash][idx] = (s, value);
                } else {
                    hashmap[hash].Add((s, value));
                }
            }
        }
        long total = 0;

        for(int i = 0; i < 256; ++i) {
            for(int j = 0; j < hashmap[i].Count; ++j) {
                total+= (i+1) * (j+1) * hashmap[i][j].v;
            }
        }

        return total;
    }
}
