namespace cs;

class Program
{
    static void Main(string[] args)
    {
        string path = args.FirstOrDefault() ?? "../AOCinput";
        var (seeds, sect) = ParseInput(path);
        Console.WriteLine(Sol1(seeds, sect));
    }


    public static (List<uint>, List<Map>[]) ParseInput(string path) {
        var fileText = File.ReadAllText(path);

        var sections = fileText.Split("\n\n");

        var data = new List<Map>[7];

        var seeds = sections[0].Split(":")[1].Trim().Split(" ").Select(x => uint.Parse(x.Trim())).ToList();

        for(int i = 1; i <= 7; ++i) {
            data[i-1] = 
                sections[i]
                    .Split("\n")
                    .Skip(1)
                    .Select(l => l.Split(" ").Select(x => uint.Parse(x.Trim())).ToArray())
                    .Select(l => new Map(l[0],l[1],l[2]))
                    .ToList();
        }

        return (seeds, data);
    }

    public static uint Sol1(List<uint> seeds, List<Map>[] sections) {
        
        uint min = uint.MaxValue;
        foreach(var seed in seeds) {
            var actualValue = seed;
            foreach(var section in sections) {
                foreach(var map in section) {
                    var diff = actualValue-map.sourI;
                    if(diff >= 0 && diff < map.range) {
                        actualValue = map.destI+diff;
                        break;
                    }
                }
            }
            min = Math.Min(actualValue, min);
        }
        return min;
    }
}

public record Map(uint destI, uint sourI, uint range);