using System.Diagnostics;
using System.Linq;

namespace AdventOfCode;
public class Program
{
    public static void Main(string[] args)
    {
        Day_16 day = new();

        int ans = args.Length != 0 ? int.Parse(args[0]) : 2;

        var init = Stopwatch.GetTimestamp();
        switch(ans)
        { 
            case 1:
                day.Answer1();
                break;
            case 2:
                day.Answer2();
                break;
            default:
                System.Console.WriteLine("No valid input");
                break;
        }

        var finish = Stopwatch.GetTimestamp();

        System.Console.WriteLine($"{Stopwatch.GetElapsedTime(init, finish).Milliseconds}ms");
    }
}

public class Day_16
{
    string[] testLines;
    string[] inputLines;
    public Day_16()
    {
        testLines = File.ReadAllLines("../AOCtest");
        inputLines = File.ReadAllLines("../AOCinput");
    }

    private Graph ParseInput(bool test = false)
    {
        Dictionary<string, int> nodes = new();
        Dictionary<string, HashSet<KeyValuePair<string, int>>> edges = new();

        var input = test ? testLines : inputLines;

        foreach (var line in input)
        {
            string valve = line.Split(" ")[1];
            int flowRate = int.Parse(string.Concat(line.Split("=")[1].TakeWhile(x=> char.IsDigit(x))));
            string[] actualEdges = line.Split(" ")[9..].Select(x=> x.Substring(0, 2)).ToArray();

            nodes.Add(valve, flowRate);
            edges.Add(valve, new());

            foreach(var v in actualEdges)
            {
                edges[valve].Add(new(v, 0));
            }
        }

        return new Graph(nodes, edges);
    }

    private Graph MakeMinimumGraph(Graph g)
    {
        Dictionary<string, int> nodes = new();
        Dictionary<string, HashSet<KeyValuePair<string, int>>> edges = new();

        foreach (var actNode in g.nodes)
        {
            if(actNode.Key != "AA" && actNode.Value == 0) continue;
            nodes.Add(actNode.Key, actNode.Value);
            edges.Add(actNode.Key, new());
        }

        foreach (var actNode in nodes)
        {
            var minimumPath = BFS_MinimumPath(g, actNode.Key);
            foreach (var node in nodes)
            {
                if(node.Key == actNode.Key) continue;
                int min = minimumPath(node.Key);
                edges[actNode.Key].Add(new KeyValuePair<string, int>(node.Key, min));
            }
        }
        return new Graph(nodes, edges);
    }

    private Func<string, int> BFS_MinimumPath(Graph g, string start) {
        var previous = new Dictionary<string, string>();
            
        var queue = new Queue<string>();
        queue.Enqueue(start);

        while (queue.Count > 0) {
            var vertex = queue.Dequeue();
            foreach(var neighbor in g.edges[vertex]) {
                if (previous.ContainsKey(neighbor.Key))
                    continue;
                
                previous[neighbor.Key] = vertex;
                queue.Enqueue(neighbor.Key);
            }
        }

        Func<string, int> shortestPath = v => {
            var path = new List<string>{};

            var current = v;
            while (!current.Equals(start)) {
                path.Add(current);
                current = previous[current];
            };

            path.Add(start);
            path.Reverse();

            return path.Count - 1;
        };

        return shortestPath;
    }

    private int MaxPressure(Graph g, string actualNode, HashSet<string> visited, int timeLeft, int released)
    {
        if(timeLeft <= 1) return released;
        
        if(actualNode != "AA")
        {
            timeLeft--;
            released = released + g.nodes[actualNode] * timeLeft;
            if(timeLeft <= 2) return released;
        }

        int max = released;

        var visitedNow = visited.Append(actualNode).ToHashSet();

        foreach (var node in g.nodes.Where(x=> !visitedNow.Contains(x.Key)))
        {
            int timeToNext = g.edges[actualNode].First(x=> x.Key == node.Key).Value;
            max = Math.Max(
                max,
                MaxPressure(g, node.Key, visitedNow, timeLeft-timeToNext, released)
            );
        }

        return max;
    }

    private int MaxPressure2(Graph g, string[] nodes, int[] times, HashSet<string> visited, int timeLeft, int released/*, bool first = false*/)
    {
        if(timeLeft <= 2) return released;

        int max = released;

        if(times.Any(x=> x == timeLeft))
        {
            var gotoNodes = g.nodes.Where(x=> !visited.Contains(x.Key)).ToList();
            for (int i = 0; i < nodes.Length; ++i)
            {
                if(times[i] != timeLeft) continue;

                var actualNode = nodes[i]; //El que ya ha terminado de moverse

                bool hasNext = false;

                foreach (var node in gotoNodes)
                {
                    //if(first) System.Console.WriteLine(node.Key);
                    int timeToNext = g.edges[actualNode].First(x=> x.Key == node.Key).Value;

                    if(timeLeft - timeToNext <= 1) continue;

                    hasNext = true;

                    var visitedNow = visited.Append(node.Key).ToHashSet();
                    var nextNodes = nodes.ToArray();
                    var nextTimes = times.ToArray();

                    nextNodes[i] = node.Key;
                    nextTimes[i] -= (timeToNext + 1);

                    int newReleased = released + node.Value * (timeLeft - timeToNext - 1);

                    max = Math.Max(
                        max,
                        MaxPressure2(g, nextNodes, nextTimes, visitedNow, timeLeft, newReleased)
                    );
                }

                if (!hasNext)
                {
                    times[i] = 0;
                    max = MaxPressure2(g, nodes, times, visited, timeLeft, released);
                }

                if(nodes.All(x=> x == "AA"))
                    break;
            }
        }
        else
        {
            max = MaxPressure2(g, nodes, times, visited, times.Max(), released);
        }
        return max;
    }

    public void Answer1()
    {
        bool test = false;
        Graph g = ParseInput(test);

        g = MakeMinimumGraph(g);

        System.Console.WriteLine("Inicio");
        
        int total = MaxPressure(g, "AA", new(), 30, 0);

        System.Console.WriteLine(total);
    }

    public void Answer2()
    {
        bool test = false;
        Graph g = ParseInput(test);

        g = MakeMinimumGraph(g);

        System.Console.WriteLine(g.nodes.Count);

        System.Console.WriteLine("Inicio");

        int total = MaxPressure2(g, new string[]{"AA", "AA"}, new int[]{26,26}, new(){"AA"}, 26, 0/*, true*/);

        System.Console.WriteLine(total);
    }
}

public record Graph(Dictionary<string, int> nodes, Dictionary<string, HashSet<KeyValuePair<string, int>>> edges);