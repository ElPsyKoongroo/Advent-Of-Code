namespace AdventOfCode;

public static class Program
{
    public static void Main()
    {
        Day_12 day = new();
        day.Answer2();
    }
}

public class Day_12
{
    string[] inputLines;
    string[] inputLinesTest;

    private int TotalPaths;
    private Graph graph;
    public Day_12()
    {
        inputLines = File.ReadAllLines("../AOCinput");
        inputLinesTest = File.ReadAllLines("../AOCtest");
    }

    #region Answer1
    public void Answer1()
    {
        TotalPaths = 0;
        graph = new(inputLines);

        Node start = graph.GetNodeByName("start");

        var a = new List<Node>();

        RecursiveSearch1(start, a);

        System.Console.WriteLine(TotalPaths);
    }

    private void RecursiveSearch1(Node actual, List<Node> alreadyPass)
    {
        List<Node> nextPassed = actual.IsBig switch
        {
            true => alreadyPass.ToList(),
            false => alreadyPass.Append(actual).ToList()
        };
        
        var adjacents = graph.Adjacents(actual);

        if(adjacents.Length == 0) return;

        foreach(Node node in adjacents)
        {
            if(node.Name == "end")
            {
                TotalPaths++;

                System.Console.WriteLine(String.Join(",", nextPassed));
                continue;
            }

            if(nextPassed.Contains(node)) continue;

            RecursiveSearch1(node, nextPassed);
        }
    }
    #endregion

    #region Answer2

    public void Answer2()
    {
        TotalPaths = 0;
        graph = new(inputLines);

        Node start = graph.GetNodeByName("start");

        var a = new List<Node>();

        RecursiveSearch2(start, a, false);

        System.Console.WriteLine(TotalPaths);
    }

    private void RecursiveSearch2(Node actual, List<Node> alreadyPass, bool onlyOnce)
    {
        List<Node> nextPassed = actual.IsBig switch
        {
            true => alreadyPass.ToList(),
            false => alreadyPass.Append(actual).ToList()
        };
        
        var adjacents = graph.Adjacents(actual);

        if(adjacents.Length == 0) return;

        foreach(Node node in adjacents)
        {
            if(node.Name == "end")
            {
                TotalPaths++;

                //System.Console.WriteLine(String.Join(",", nextPassed) + ",end");
                continue;
            }

            if(nextPassed.Contains(node))
            {
                if(!onlyOnce && !node.IsBig && node.Name != "start")
                    RecursiveSearch2(node, nextPassed, true);
                continue;
            }

            RecursiveSearch2(node, nextPassed, onlyOnce);
        }
    }

    #endregion
}

#region Classes

public class Graph
{
    public List<Node> _nodes;
    public  List<Link> _links;

    public Node GetNodeByName(string name)
    {
        return _nodes.First(x=> x.Name == name);
    }

    public Node[] Adjacents(Node node)
    {
        return 
            _links
            .Where(l=> l.Has(node) != -1)
            .Select(l=> l.GetNext(node))
            .ToArray();
    }

    public Graph(string[] input)
    {
        _nodes = new();
        _links = new();
        foreach(var line in input)
        {
            var nodes = line.Split("-").Select(x=> new Node(x)).ToArray();

            if(!_nodes.Contains(nodes[0])) _nodes.Add(nodes[0]);
            if(!_nodes.Contains(nodes[1])) _nodes.Add(nodes[1]);

            Link link = new(nodes[0], nodes[1]);
            if(!_links.Contains(link)) _links.Add(link);
        }
    }

    
}
public class Node
{
    public readonly string Name;
    public readonly bool IsBig;
    public Node(string name)
    {
        Name = name;
        IsBig = char.IsUpper(name[0]);
    }

    public override bool Equals(object? o)
    {
        if(o is Node n)
        {
            if(n.Name == Name) return true;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public static bool operator == (Node c1, Node c2)  
    {  
        return c1.Equals(c2);  
    }
    public static bool operator != (Node c1, Node c2)  
    {  
        return !c1.Equals(c2);  
    }

    public override string ToString()
    {
        return Name;
    }

}
public class Link
{
    public Node[] nodes;
    public Link(Node a, Node b) => nodes = new Node[]{a,b};

    public Node GetNext(Node a) => (a == nodes[0] ? nodes[1] : nodes[0]);

    public int Has(Node a) => nodes[0] == a ? 0 : (nodes[1] == a ? 1 : -1);

    public override bool Equals(object? obj)
    {
        if(obj is Link l)
        {
            if(l.nodes[0] == nodes[0] && l.nodes[1] == nodes[1] || 
                l.nodes[0] == nodes[1] && l.nodes[1] == nodes[0]) return true;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public static bool operator == (Link c1, Link c2)  
    {  
        return c1.Equals(c2);  
    }
    public static bool operator != (Link c1, Link c2)  
    {  
        return !c1.Equals(c2);  
    }  

}

#endregion