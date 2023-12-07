namespace cs;

class Program
{
    static void Main(string[] args)
    {
        string path = args.FirstOrDefault() ?? "../AOCinput";
        
        // Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }

    public static int Sol1(string path) {

        var input = ParseInput(path);

        return input.Select(x=> (int)Math.Floor(Math.Pow(2, x-1))).Sum();
    }

    public static int[] ParseInput(string path) {
        
        var inputLines = File.ReadAllLines(path);
        var sets = 
            inputLines
            .Select(
                l => l
                    .Split(":")[1]
                    .Split("|")
                    .Select(hl=> hl.Split(" ").Where(x=> !string.IsNullOrEmpty(x)).Select(x=> int.Parse(x)).ToArray()).ToArray()
            ).ToArray();
        
        var intersections =
            sets.Select(l => 
                l[0].Intersect(l[1]).Count()
            ).ToArray();

        return intersections;
    }

    public static int Sol2(string path) {

        var input = ParseInput(path);
        var cards = Enumerable.Repeat(1, input.Length).Zip(input).Select(x=> new int[]{x.First, x.Second}).ToArray();

        for(int i = 0; i < cards.Length; ++i) {
            int ammount = cards[i][0];
            int matching = cards[i][1];

            for(int j = 1; j <= matching; ++j) {
                cards[i+j][0]+=ammount;
            }
        }

        return cards.Select(x=> x[0]).Sum();
    }
}
