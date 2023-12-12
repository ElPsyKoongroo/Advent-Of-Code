using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace cs;

class Program
{
    enum State {
        Good,
        Bad,
        Unknown
    };

    static State ParseState(char c) => c switch {
        '.' => State.Good,
        '#' => State.Bad,
        _ => State.Unknown,
    };

    record Row(State[] States, int[] Info);

    static void Main(string[] args)
    {
        var path = args.FirstOrDefault("../AOCtest");
        // Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }

    static Row[] ParseInput(string path) {
        return File.ReadAllLines(path)
            .Select(X=> X.Split(' ', ','))
            .Select(x=> 
                new Row(
                    x[0].Select(ParseState).ToArray(),
                    x[1..].Select(int.Parse).ToArray()
                )
            ).ToArray();
    }

    static Row[] ParseInput2(string path) {
        return File.ReadAllLines(path)
            .Select(X=> X.Split(' ', ','))
            .Select(x=> 
                new Row(
                    String.Join("?",Enumerable.Range(0, 5).Select(y => x[0])).Select(ParseState).ToArray(),
                    Enumerable.Range(0, 5).Select(y => x[1..].Select(int.Parse)).SelectMany(x=>x).ToArray()
                )
            ).ToArray();
    }

    static int Sol1(string path) {
        var input = ParseInput(path);
        System.Console.WriteLine("Go");
        int total = 0;

        foreach(var row in input) {
            var actualState = 
                row.States.ToArray().AsSpan();
            var info = row.Info.AsSpan();
            var unknowns = row.States.Select((x, i)=> (x,i)).Where(x=> x.x == State.Unknown).Select(x=> x.i).ToArray().AsSpan();
            total += CountPossibilites(actualState, info, unknowns, 0);
        }
        return total;
    }

    static int CountPossibilites(Span<State> actState, ReadOnlySpan<int> info, ReadOnlySpan<int> unknowns, int actUnknown) {
        if (actUnknown == unknowns.Length) {
            // if(actState.ToArray().Count( x => x == State.Bad ) != info.ToArray().Sum()) return 0;
            return CheckState(actState, info) ? 1 : 0;
        }
        
        if(actState.ToArray().Count( x => x == State.Bad ) > info.ToArray().Sum()) {
            return 0;
        }

        actState[unknowns[actUnknown]] = State.Good;
        int actTotal = CountPossibilites(actState, info, unknowns, actUnknown+1);
        actState[unknowns[actUnknown]] = State.Bad;
        actTotal += CountPossibilites(actState, info, unknowns, actUnknown+1);

        return actTotal;
    }

    static bool CheckState(Span<State> actState, ReadOnlySpan<int> info) {
        int actInfo = 0;
        for(int i = 0; i < actState.Length; ++i) {
            if(actState[i] == State.Good) continue;
            int total = 1;
            i++;
            for(; i < actState.Length; ++i) {
                if(actState[i] == State.Good) break;
                total++;
            }
            if(info.Length == actInfo) return false;
            if(info[actInfo++] != total) return false;
        }
        return info.Length == actInfo;
    }

    static int Sol2(string path) {
        var input = ParseInput2(path);
        System.Console.WriteLine("Go");
        int total = 0;

        foreach(var row in input) {
            var actualState = 
                row.States.ToArray().AsSpan();
            var info = row.Info.AsSpan();
            var unknowns = row.States.Select((x, i)=> (x,i)).Where(x=> x.x == State.Unknown).Select(x=> x.i).ToArray().AsSpan();
            total += CountPossibilites(actualState, info, unknowns, 0);
        }
        return total;
    }
}
