using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using RegExtract;

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


record class Node(string Name, int Type, Dictionary<string, bool> Pre, List<string> Post, bool State) {
    public bool State {get;set;} = State;
};
//type 0 -> broadcast, 1 -> FF, 2 -> Conj

class Program
{
    static void Main(string[] args)
    {
        string path = args.FirstOrDefault("../AOCtest");
        // Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }

    internal static readonly char[] symbols = ['&', '%'];

    static List<Node> ParseInput(string path) {
        var modules = File.ReadAllLines(path).Extract<(string, List<string>)>(@"(.?\w+) -> ((\w+),? ?)+").ToArray();
        
        List<Node> input = [];

        foreach(var (n, next) in modules) {

            var (type, name) = n[0] switch {
                '&' => (2, n[1..]),
                '%' => (1, n[1..]),
                _ => (0, n)
            };

            input.Add(new Node(name, type, [], next, false));
        }

        foreach(var (n, nexts) in modules) { 
            foreach(var next in nexts) {
                var node = input.FirstOrDefault(x=> x.Name == next);
                if(node is null) continue;
                if(node.Type != 2) continue;
                var name = n[0] switch {
                    '&' => n[1..],
                    '%' => n[1..],
                    _ => n
                };
                node.Pre.Add(name, false);
            }
        }
        

        return input;
    }

    static long Sol1(string path) {
        var modules = ParseInput(path);


        long highPulses = 0;
        long lowPulses = 0;

        List<(bool[] state, long highs, long lows)> seen = [];

        long totalIters = 0;

        while(!seen.Any(x=> x.state.SequenceEqual(modules.Select(x=> x.State))) && totalIters<1000) {
            totalIters++;
            seen.Add((modules.Select(x=> x.State).ToArray(), highPulses, lowPulses));

            Queue<(string nodeFrom, string nodeTo, bool signal)> signals = [];
            // Console.WriteLine("{0} - {2} -> {1}","buttom", "broadcaster", false);
            signals.Enqueue(("buttom", "broadcaster", false));
            ++lowPulses;

            while(signals.TryDequeue(out var next)) {
                var idx = modules.FindIndex(x=> x.Name == next.nodeTo);
                if(idx == -1) continue;
                Node node = modules[idx];

                switch (node.Type) {
                    case 0: {
                        foreach(var nextNodes in node.Post) {
                            // Console.WriteLine("{0} - {2} -> {1}",node.Name, nextNodes, next.signal);
                            signals.Enqueue((node.Name, nextNodes, next.signal));
                        }
                        int total = node.Post.Count;
                        if(next.signal) {
                            highPulses+=total;
                        } else {
                            lowPulses+=total;
                        }
                        break;
                    }
                    case 1: {
                        if(next.signal) {
                            break;
                        }
                        node.State = !node.State;
                        foreach(var nextNodes in node.Post) {
                            // Console.WriteLine("{0} - {2} -> {1}",node.Name, nextNodes, node.State);
                            signals.Enqueue((node.Name, nextNodes, node.State));
                        }
                        int total = node.Post.Count;
                        if(node.State) {
                            highPulses+=total;
                        } else {
                            lowPulses+=total;
                        }
                        break;
                    }
                    default: {
                        node.Pre[next.nodeFrom] = next.signal;
                        bool signal = !node.Pre.Values.All(x=>x);
                        node.State = !signal;

                        foreach(var nextNodes in node.Post) {
                            // Console.WriteLine("{0} - {2} -> {1}",node.Name, nextNodes, signal);
                            signals.Enqueue((node.Name, nextNodes, signal));
                        }
                        int total = node.Post.Count;
                        if(signal) {
                            highPulses+=total;
                        } else {
                            lowPulses+=total;
                        }
                        break;
                    }
                }
            }

        }


        // Console.WriteLine("Terminao");
        // Console.WriteLine(highPulses);
        // Console.WriteLine(lowPulses);

        // if(totalIters == 1000) {
            return highPulses * lowPulses;
        // }

        // var index = seen.FindIndex(x=> x.state.SequenceEqual(modules.Select(x=> x.State)));
        // var totalseen = seen.Count;

        // long totalHigh = seen[index].highs;
        // long totalLow = seen[index].lows;

        // var delta = totalseen-index;

        // long range = (1000-index) / delta;
        // long extra = (1000-index) % delta;

        // long deltaHigh = highPulses - totalHigh;
        // long deltaLow = lowPulses - totalLow;

        // totalHigh+= deltaHigh*range;
        // totalLow+= deltaLow*range;

        // Console.WriteLine(totalHigh);
        // Console.WriteLine(totalLow);
        // Console.WriteLine(extra);

        // return totalHigh*totalLow;
    }

    static long Sol2(string path) {
        var modules = ParseInput(path);

        var feed = modules.Single(x=> x.Post.Contains("rx")).Name; 

        var seenNames = modules.Where(X=> X.Post.Contains(feed)).Select(x=> x.Name);

        Dictionary<string, int> seen = [];
        Dictionary<string, int> cycles_lenght = [];

        foreach(var n in seenNames) {
            seen.Add(n, 0);
        }

        int totalIters = 0;
        bool exit = false;

        while(!exit) {
            totalIters++;

            Queue<(string nodeFrom, string nodeTo, bool signal)> signals = [];
            // Console.WriteLine("{0} - {2} -> {1}","buttom", "broadcaster", false);
            signals.Enqueue(("buttom", "broadcaster", false));
            
            if(totalIters % 100_000 == 0) {
                System.Console.WriteLine(totalIters);
            }

            while(signals.TryDequeue(out var next)) {
                var idx = modules.FindIndex(x=> x.Name == next.nodeTo);
                if(idx == -1) continue;
                Node node = modules[idx];

                if (node.Name == feed && next.signal) {
                    seen[next.nodeFrom] += 1;

                    if(!cycles_lenght.TryGetValue(next.nodeFrom, out int value)) {
                        cycles_lenght.Add(next.nodeFrom, totalIters);
                    } else {
                        Trace.Assert(totalIters == seen[next.nodeFrom] * value);
                    }
                    if(seen.Values.All(x=> x > 0)) {
                        BigInteger result = new(1);

                        foreach(var x in cycles_lenght.Values) {
                            result = result * x /  BigInteger.GreatestCommonDivisor(result, x);
                        }
                        return (long)result;
                    }
                }

                switch (node.Type) {
                    case 0: {
                        foreach(var nextNodes in node.Post) {
                            // Console.WriteLine("{0} - {2} -> {1}",node.Name, nextNodes, next.signal);
                            signals.Enqueue((node.Name, nextNodes, next.signal));
                        }
                        break;
                    }
                    case 1: {
                        if(next.signal) {
                            break;
                        }
                        node.State = !node.State;
                        foreach(var nextNodes in node.Post) {
                            // Console.WriteLine("{0} - {2} -> {1}",node.Name, nextNodes, node.State);
                            signals.Enqueue((node.Name, nextNodes, node.State));
                        }
                        break;
                    }
                    default: {
                        node.Pre[next.nodeFrom] = next.signal;
                        bool signal = !node.Pre.Values.All(x=>x);
                        node.State = !signal;

                        foreach(var nextNodes in node.Post) {
                            // Console.WriteLine("{0} - {2} -> {1}",node.Name, nextNodes, signal);
                            signals.Enqueue((node.Name, nextNodes, signal));
                        }
                        break;
                    }
                }
            }

        }

        return totalIters;
        
    }
}
