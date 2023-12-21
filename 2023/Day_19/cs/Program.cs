using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RegExtract;
using MoreLinq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Diagnostics;

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


record Part(int X, int M, int A, int S);
record Condition(char Var, char Comparer, int Num, string NextName);
record Workflow(string Name, List<Condition> Conditions, string Final);

record PartRange((int s, int e) X, (int s, int e) M, (int s, int e) A, (int s, int e) S);
class Program
{
    static void Main(string[] args)
    {
        string path = args.FirstOrDefault("../AOCtest");

        Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }

    static (Workflow[], Part[]) ParseInput(string path) {
        var groups = File.ReadAllLines(path).Split(string.IsNullOrEmpty).ToArray();

        var works = groups[0].Extract<Workflow>(@"(\w+){((\w)(.)(\d+):(\w+),)+(\w+)}").ToArray();
        var parts = groups[1].Extract<Part>(@"{x=(\d+),m=(\d+),a=(\d+),s=(\d+)}").ToArray();

        return (works, parts);
    }

    static long Sol1(string path)
    {
        var (worksArr, parts) = ParseInput(path);

        var works = worksArr.ToDictionary(x=> x.Name);

        long total = 0;
        foreach(var part in parts) {
            string actualWork = "in";
            while(true) {
                var conds = works[actualWork].Conditions;
                bool found = false;
                foreach(var cond in conds) {
                    var value = cond.Var switch {
                        'a' => part.A,
                        'x' => part.X,
                        'm' => part.M,
                        _ => part.S
                    };
                    found = cond.Comparer == '<' ? value < cond.Num : value > cond.Num;
                    if(found) {
                        actualWork = cond.NextName;
                        break;
                    }
                }
                if(!found) actualWork = works[actualWork].Final;

                if(actualWork == "A") {
                    var a = part.A + part.X + part.M + part.S;
                    total += a;
                    break;
                }
                if(actualWork == "R") {
                    break;
                }
            }
        }

        return total;
    }

    static long Sol2(string path)
    {
        var works = ParseInput(path).Item1.ToDictionary(x=> x.Name);

        Queue<(PartRange, string, int)> ranges = [];
        ranges.Enqueue((new((1,4000),(1,4000),(1,4000),(1,4000)), "in", 0));

        long total = 0;
        while(ranges.TryDequeue(out var next)) {
            var (partRanges, work, condition) = next;
            
            bool found = false;
            bool breakCycle = false;
            for(int i = condition; i < works[work].Conditions.Count; ++i) {
                var cond = works[work].Conditions[i];
                var (s, e) = cond.Var switch {
                    'a' => partRanges.A,
                    'x' => partRanges.X,
                    'm' => partRanges.M,
                    _ => partRanges.S
                };

                var (rangeEqual, isTrue, offset) = cond.Comparer switch {
                    '<' => (s < cond.Num == e < cond.Num, e < cond.Num, -1),
                    _ => (s > cond.Num == e > cond.Num, e > cond.Num, 0)
                };
                
                if(rangeEqual) {
                    if(isTrue) {
                        work = cond.NextName;
                        found = true;
                        break;
                    }
                } else {
                    var lr = (s, cond.Num+offset);
                    var ur = (cond.Num+offset+1, e);
                    
                    var (newL,newU) = cond.Var switch {
                        'a' =>  (partRanges with {A=lr}, partRanges with {A=ur}),
                        'x' =>  (partRanges with {X=lr}, partRanges with {X=ur}),
                        'm' =>  (partRanges with {M=lr}, partRanges with {M=ur}),
                        _ =>    (partRanges with {S=lr}, partRanges with {S=ur}),
                    };
                    ranges.Enqueue((newL, work, i));
                    ranges.Enqueue((newU, work, i));
                    breakCycle = true;
                    break;
                }
            }
            if(breakCycle) continue;
            if(!found) {
                work = works[work].Final;
            }
            
            if(work == "R") continue;
            if(work == "A")  {
                long totalx = partRanges.X.e - partRanges.X.s + 1;
                long totalm = partRanges.M.e - partRanges.M.s + 1;
                long totala = partRanges.A.e - partRanges.A.s + 1;
                long totals = partRanges.S.e - partRanges.S.s + 1;
                total += totalx * totalm * totals * totala;
                continue;
            }
            ranges.Enqueue((partRanges, work, 0));
        }

        return total;
    }
}
