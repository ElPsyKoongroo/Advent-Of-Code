using System.Diagnostics;
using System.Linq;

namespace AdventOfCode;
public class Program
{
    public static void Main(string[] args)
    {
        Day_16 day = new();

        int ans = args.Length != 0 ? int.Parse(args[0]) : 1;

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
            nodes.Add(actNode.Key, actNode.Value);
            edges.Add(actNode.Key, new());
        }

        foreach (var actNode in g.nodes)
        {
            var minimumPath = BFS_MinimumPath(g, actNode.Key);
            foreach (var node in g.nodes)
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
            if(g.nodes[node.Key] == 0) continue;
            int timeToNext = g.edges[actualNode].First(x=> x.Key == node.Key).Value;
            max = Math.Max(
                max,
                MaxPressure(g, node.Key, visitedNow, timeLeft-timeToNext, released)
            );
        }

        return max;
    }

    public void Answer1()
    {
        bool test = false;
        Graph g = ParseInput(test);

        g = MakeMinimumGraph(g);

        System.Console.WriteLine("Inicio");
        var init = Stopwatch.GetTimestamp();

        int total = MaxPressure(g, "AA", new(), 30, 0);

        var finish = Stopwatch.GetTimestamp();

        System.Console.WriteLine(Stopwatch.GetElapsedTime(init, finish));

        System.Console.WriteLine(total);
    }

    public void Answer2()
    {
        bool test = false;
        Graph g = ParseInput(test);

        g = MakeMinimumGraph(g);

        System.Console.WriteLine("Inicio");
        var init = Stopwatch.GetTimestamp();

        int total = MaxPressure(g, "AA", new(), 26, 0);

        var finish = Stopwatch.GetTimestamp();

        System.Console.WriteLine(Stopwatch.GetElapsedTime(init, finish));

        System.Console.WriteLine(total);
    }
}

public record Graph(Dictionary<string, int> nodes, Dictionary<string, HashSet<KeyValuePair<string, int>>> edges);