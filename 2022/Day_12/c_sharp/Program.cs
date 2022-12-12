namespace AdventOfCode;
public static class Program
{
    public static void Main(string[] args)
    {
        Day_12 day = new();
        day.Answer1();
        //day.Answer2();
    }
}


public class Day_12
{
    string[] testLines;
    string[] inputLines;
    int[][] map;

    public Day_12()
    {
        testLines = File.ReadAllLines("../AOCtest");
    }

    private (KeyValuePair<int, int>, KeyValuePair<int, int>) CreateTable()
    {
        map =
            testLines
            .Select(ln => ln.Select(x=> 
            {
                return x switch
                {
                    'S' => 'a',
                    'E' => 'z',
                    _ => x - 'a'
                };
            }).ToArray())
            .ToArray();
        
        var posStart = 
            testLines
            .SelectMany(
                (line, i) => line
                    .Select((x, j)=> new{X = i, Y = j, value = x})
            )
            .First(x=> x.value == 'S');

        var posEnd = 
            testLines
            .SelectMany(
                (line, i) => line
                    .Select((x, j)=> new{X = i, Y = j, value = x})
            )
            .First(x=> x.value == 'E');

        KeyValuePair<int, int> start = new(posStart.X, posStart.Y);
        KeyValuePair<int, int> end = new(posEnd.X, posEnd.Y);

        return (start, end);
    }

    private List<KeyValuePair<int, int>> GetNextCells(List<KeyValuePair<int, int>> path)
    {
        var actualCell = path.Last();


    }

    public void Answer1()
    {
        (var map, var start, var end) = CreateTable();

        PriorityQueue<List<KeyValuePair<int, int>>, int> possiblesPath = new();
        int finalPathLength = -1;
        possiblesPath
            .Enqueue(
                new List<KeyValuePair<int, int>>(
                    new KeyValuePair<int, int>[]{start}
                ), 0
            );
        
        while(true)
        {
            if(possiblesPath.Count == 0) throw new Exception("No quedan paths posibles");

            var actualPath = possiblesPath.Dequeue();

            var possiblesNextCell = GetNextCells(actualPath);

        }
    }

    public void Answer2()
    {
        
    }
}