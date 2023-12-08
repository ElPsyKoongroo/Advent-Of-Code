using System.Numerics;
using System.Text.RegularExpressions;

namespace cs;

class Program
{
    static void Main(string[] args)
    {
        string path = args.FirstOrDefault("../AOCinput");
        // Console.WriteLine($"Sol1: {Sol1(path)}");
        Console.WriteLine($"Sol2: {Sol2(path)}");
    }

    static int Sol1(string path) {
        var (movements, map) = ParseInput(path);
        
        int counter = 0;
        string lookingFor = "AAA";
        bool finished = false;
        int actual = 0;
        int max = movements.Length;

        while(!finished) {
            var mov = movements[actual];
            lookingFor = mov == 'L' ? map[lookingFor].Left : map[lookingFor].Right;
            actual = (actual+1)%max;
            counter++;
            if(lookingFor == "ZZZ") finished = true;
        }
        return counter;
    }

    static (string, Dictionary<string, (string Left, string Right)>) ParseInput(string path) {
        var inputText = File.ReadAllText(path);

        string deli = Environment.NewLine;

        var sections = inputText.Split(deli+deli);

        string directions = sections[0];

        Dictionary<string, (string, string)> map = [];

        foreach(var line in sections[1].Split(deli)) {
            var inputs = 
                line
                .Split(['=','(',',',')',' '])
                .Where(x=> !string.IsNullOrWhiteSpace(x))
                .ToArray();

            map[inputs[0]] = (inputs[1], inputs[2]);
        }

        return (directions, map);
    }

    static ulong Sol2(string path) {
        var (movements, map) = ParseInput(path);
        Console.WriteLine("Go");

        string[] startingA = 
            map.Keys
            .Where(x=> x.Last()=='A')
            .ToArray();
        int max = movements.Length;

        List<int> ints = [];
        foreach(var lfa in startingA) {
            int i = 0;
            int counter = 0;
            var actualPos = lfa;

            List<(string pos, int pasos)> finalsZ = [];
            int weno = 0;
            while(true) {
                if(actualPos[2] == 'Z') {
                    if(!finalsZ.Any(x=> x.pos == actualPos && x.pasos == i)) {
                        finalsZ.Add((actualPos, i));
                    } else {
                        weno = counter;
                        break;
                    }
                }
                actualPos = movements[i] == 'L' ? map[actualPos].Left : map[actualPos].Right;
                i = (i+1)%max;
                counter++;
            }
            ints.Add(weno);
        }

        foreach(var i in ints) {
            Console.WriteLine(i);
        }

        BigInteger res = new(ints[0]);
        foreach(var i in ints[1..]) {
            var x = new BigInteger(i);
            var mcd = BigInteger.GreatestCommonDivisor(res, x);
            res = res/mcd*x;
        }
        
        return (ulong)res;
    }
}
