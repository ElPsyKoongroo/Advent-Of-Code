namespace cs;

class Program
{
    static void Main(string[] args)
    {
        var path = args.FirstOrDefault("../AOCinput");

        // Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }

    static long Sol1(string path) {
        var input = ParseInput(path);

        long total = 0;

        foreach(var line in input) {
            List<int[]> ladder = [line];

            while(ladder[^1].Any(x=> x!=0)) {
                int actCnt = ladder.Last().Length-1;
                ladder.Add(new int[actCnt]);
                for(int i = 0; i < actCnt; ++i) {
                    ladder[^1][i] = ladder[^2][i+1] - ladder[^2][i];
                }
            }
            total += ladder.Select(x=> x[^1]).Sum();
        }
        return total;
    }

    static long Sol2(string path) {
        var input = ParseInput(path);

        long total = 0;

        foreach(var line in input) {
            List<int[]> ladder = [line];

            while(ladder[^1].Any(x=> x!=0)) {
                int actCnt = ladder.Last().Length-1;
                ladder.Add(new int[actCnt]);
                for(int i = 0; i < actCnt; ++i) {
                    ladder[^1][i] = ladder[^2][i+1] - ladder[^2][i];
                }
            }
            total += 
                ladder
                .Select(x=> x[0])
                .Reverse()
                .Aggregate(0, (acc, act) => act-acc);
        }
        return total;
    }

    static int[][] ParseInput(string path) {
        return 
            File.ReadAllLines(path)
            .Select(l=> l.Split(" ").Select(x => int.Parse(x)).ToArray())
            .ToArray();
    }
}
