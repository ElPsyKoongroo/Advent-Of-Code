using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Numerics;

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

    static char CharState(State s) => s switch {
        State.Good => '.',
        State.Bad => '#',
        _ => '?',
    };

    record Row(State[] States, int[] Info);

    static void Main(string[] args)
    {
        var path = args.FirstOrDefault("../AOCinput");
        Console.WriteLine(Sol1(path));
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

    static long Sol1(string path) {
        var input = ParseInput(path);
        System.Console.WriteLine("Go");
        long total = 0;

        foreach(var row in input) {
            var actualState = 
                row.States;
            var info = row.Info;
            Dictionary<(string, string), long> cache = [];
            total += CountPossibilites(actualState, info, cache);
        }
        return total;
    }

    static long CountPossibilites(State[] actState, int[] info, Dictionary<(string, string), long> cache) {

        if(actState.Length == 0) {
            return info.Length == 0 ? 1 : 0;
        }
        if(info.Length == 0) {
            return actState.Contains(State.Bad) ? 0 : 1;
        }
        long total = 0;

        var key = (string.Concat(actState.Select(CharState)), string.Concat(info.Select(x=> x.ToString())));

        if(cache.TryGetValue(key, out long v)) {
            return v;
        }

        if(actState[0] != State.Bad) {
            total += CountPossibilites(actState[1..], info, cache);
        }

        if(actState[0] != State.Good) {
            int blk = info[0];
            if(blk <= actState.Length && actState[0..blk].All(x=> x != State.Good) && (actState.Length == blk || actState[blk] != State.Bad)) {
                total += CountPossibilites([..actState.Skip(blk+1)], info[1..], cache);
            }
        }
        cache.Add(key, total);
        return total;
    }

    static long Sol2(string path) {
        var input = ParseInput2(path);
        Console.WriteLine("Go");
        long total = 0;

        foreach(var row in input) {
            var actualState = 
                row.States;
            var info = row.Info;
            Dictionary<(string, string), long> cache = [];
            total += CountPossibilites(actualState, info, cache);
        }
        return total;
    }
}
