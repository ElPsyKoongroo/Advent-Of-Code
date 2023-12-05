namespace cs;

using Range = (uint, uint);

class Program
{
    static void Main(string[] args)
    {
        string path = args.FirstOrDefault() ?? "../AOCinput";
        
        // Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }


    public static (List<uint>, List<Map>[]) ParseInput1(string path) {
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
    public static uint Sol1(string path) {
        
        var (seeds, sections) = ParseInput1(path);

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

    public static (List<Range>, List<Map>[]) ParseInput2(string path) {
        var fileText = File.ReadAllText(path);

        var sections = fileText.Split("\n\n");

        var data = new List<Map>[7];

        var seeds = 
            sections[0]
                .Split(":")[1]
                .Trim()
                .Split(" ")
                .Select(x => uint.Parse(x.Trim()))
                .Chunk(2)
                .Select(x=> new Range(x[0], x[0]+x[1]-1))
                .ToList();

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

    public static uint Sol2(string path) {
        uint min = uint.MaxValue;
        var (seeds, sections) = ParseInput2(path);
        return min;
    }
}

public record Map(uint destI, uint sourI, uint range);