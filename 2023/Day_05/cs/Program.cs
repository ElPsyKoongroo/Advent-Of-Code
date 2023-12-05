namespace cs;

using System.Runtime.InteropServices;
using Range = (uint ini, uint fin);

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
                .Select(x=> new Range(x[0], x[0]+x[1]))
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
        var (seeds, sections) = ParseInput2(path);
        
        var actualSeeds = new Queue<Range>(seeds);

        foreach(var section in sections) {
            Queue<Range> sectionSeeds = [];

            while(actualSeeds.TryDequeue(out Range actual)) {
                bool anyChange = false;
                foreach (var map in section) {
                    var overlapS = Math.Max(actual.ini, map.sourI);
                    var overlapE = Math.Min(actual.fin, map.sourI+map.range);
                    
                    if(overlapS < overlapE) {
                        sectionSeeds.Enqueue(new Range(overlapS-map.sourI+map.destI, overlapE-map.sourI+map.destI));
                        if(overlapS > actual.ini)
                            actualSeeds.Enqueue(new Range(actual.ini, overlapS));
                        if(actual.fin > overlapE) 
                            actualSeeds.Enqueue(new Range(overlapE, actual.fin));
                        anyChange = true;
                        break;
                    }
                }
                if(!anyChange)
                    sectionSeeds.Enqueue(actual);
            }
            actualSeeds = sectionSeeds;
        }
        return actualSeeds.MinBy(x => x.ini).ini;
    }
    public static bool Overlap(Range r1, uint r2i, uint r2f) {
        return
            r1.ini >= r2i && r1.ini <= r2f
            || r1.fin >= r2i && r1.fin <= r2f;
    }
}

public record Map(uint destI, uint sourI, uint range);