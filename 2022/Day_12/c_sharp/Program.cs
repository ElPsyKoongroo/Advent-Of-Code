using System.Diagnostics;

namespace AdventOfCode;
public static class Program
{
    public static void Main(string[] args)
    {
        Day_12 day = new();
        var st = Stopwatch.StartNew();
        //day.Answer1();
        day.Answer2();
        System.Console.WriteLine(st.ElapsedMilliseconds / 1000.0);
    }
}


public class Day_12
{
    string[] testLines;
    string[] inputLines;
    int[][] map;
    int max_x;
    int max_y;

    public Day_12()
    {
        testLines = File.ReadAllLines("../AOCtest");
        inputLines = File.ReadAllLines("../AOCinput");
    }

//340282366920938463463374607431768211456

    private (KeyValuePair<int, int>, KeyValuePair<int, int>) CreateTable()
    {
        map =
            inputLines
            .Select(ln => ln.Select(x=> 
            {
                return x switch
                {
                    'S' => 0,
                    'E' => 'z' - 'a',
                    _ => x - 'a'
                };
            }).ToArray())
            .ToArray();
        
        max_x = map.Length;
        max_y = map[0].Length;

        var posStart = 
            inputLines
            .SelectMany(
                (line, i) => line
                    .Select((x, j)=> new{X = i, Y = j, value = x})
            )
            .First(x=> x.value == 'S');

        var posEnd = 
            inputLines
            .SelectMany(
                (line, i) => line
                    .Select((x, j)=> new{X = i, Y = j, value = x})
            )
            .First(x=> x.value == 'E');

        KeyValuePair<int, int> start = new(posStart.X, posStart.Y);
        KeyValuePair<int, int> end = new(posEnd.X, posEnd.Y);

        return (start, end);
    }

    private (List<KeyValuePair<int, int>>, KeyValuePair<int, int>) CreateTable2()
    {
        var input = inputLines;

        map =
            inputLines
            .Select(ln => ln.Select(x=> 
            {
                return x switch
                {
                    'S' => 0,
                    'E' => 'z' - 'a',
                    _ => x - 'a'
                };
            }).ToArray())
            .ToArray();
        
        var starts = map
            .SelectMany(
                (line, i) => line
                    .Select((x, j)=> new{X = i, Y = j, value = x}))
            .Where(x=> x.value == 0)
            .Select(x=> new KeyValuePair<int, int>(x.X, x.Y)).ToList();

        max_x = map.Length;
        max_y = map[0].Length;

        var posStart = 
            input
            .SelectMany(
                (line, i) => line
                    .Select((x, j)=> new{X = i, Y = j, value = x})
            )
            .First(x=> x.value == 'S');

        var posEnd = 
            input
            .SelectMany(
                (line, i) => line
                    .Select((x, j)=> new{X = i, Y = j, value = x})
            )
            .First(x=> x.value == 'E');
        KeyValuePair<int, int> end = new(posEnd.X, posEnd.Y);

        return (starts, end);
    }

    private List<KeyValuePair<int, int>> GetNextCells(List<KeyValuePair<int, int>> path)
    {
        List<KeyValuePair<int, int>> nextCells = new();
        var actualCell = path.Last();

        
        foreach((int i, int j) in new (int,int)[]{(1, 0),(-1, 0),(0, 1),(0, -1)})
        {        
            var next = new KeyValuePair<int, int>(actualCell.Key+i, actualCell.Value+j);
            
            if(AcceptableNext(actualCell, i, j) && 
                !path.Contains(next))
            {
                nextCells.Add(next);
            }
        }
        
        return nextCells;
    }

     private List<KeyValuePair<int, int>> GetNextCells2(List<KeyValuePair<int, int>> path)
    {
        List<KeyValuePair<int, int>> nextCells = new();
        var actualCell = path.Last();

        
        foreach((int i, int j) in new (int,int)[]{(1, 0),(-1, 0),(0, 1),(0, -1)})
        {        
            var next = new KeyValuePair<int, int>(actualCell.Key+i, actualCell.Value+j);
            
            if(AcceptableNext2(actualCell, i, j) && 
                !path.Contains(next))
            {
                nextCells.Add(next);
            }
        }
        
        return nextCells;
    }

    private bool AcceptableNext(KeyValuePair<int, int> pos, int x_offset, int y_offset)
    {
        return (pos.Key+x_offset >= 0 &&
            pos.Key+x_offset < max_x &&
            pos.Value+y_offset >= 0 &&
            pos.Value+y_offset < max_y)
            &&
            map[pos.Key+x_offset][pos.Value+y_offset] <=  (map[pos.Key][pos.Value] + 1);
    }

    private bool AcceptableNext2(KeyValuePair<int, int> pos, int x_offset, int y_offset)
    {
        return (pos.Key+x_offset >= 0 &&
            pos.Key+x_offset < max_x &&
            pos.Value+y_offset >= 0 &&
            pos.Value+y_offset < max_y)
            && map[pos.Key+x_offset][pos.Value+y_offset] <=  (map[pos.Key][pos.Value] + 1)
            && map[pos.Key+x_offset][pos.Value+y_offset] != 0;
    }


    public void Answer1()
    {
        (var start, var end) = CreateTable();

        PriorityQueue<List<KeyValuePair<int, int>>, int> possiblesPath = new();
        int finalPathLength = -1;
        possiblesPath
            .Enqueue(
                new List<KeyValuePair<int, int>>(
                    new KeyValuePair<int, int>[]{start}
                ), 0
            );

        HashSet<KeyValuePair<int, int>> alreadyPassed = new();

        alreadyPassed.Add(start);
        
        while(finalPathLength == -1)
        {
            if(possiblesPath.Count == 0) throw new Exception("No quedan paths posibles");

            var actualPath = possiblesPath.Dequeue();

            var possiblesNextCell = GetNextCells(actualPath);

            foreach(var cell in possiblesNextCell)
            {
                if(cell.Equals(end))
                {
                    finalPathLength = actualPath.Count();
                    break;
                }
                if(!alreadyPassed.Contains(cell))
                {
                    alreadyPassed.Add(cell);
                    possiblesPath.Enqueue(
                        actualPath.Append(cell).ToList(),
                        (actualPath.Count+1)*100 + (100-map[cell.Key][cell.Value])
                    );
                }
            }
        }
        System.Console.WriteLine(finalPathLength);
    }

    public void Answer2()
    {
        (var starts, var end) = CreateTable2();

        PriorityQueue<List<KeyValuePair<int, int>>, int> possiblesPath = new();
        int finalPathLength = -1;
        possiblesPath
            .EnqueueRange(
                starts.Select(x=> (new List<KeyValuePair<int,int>>{x}, 0))
            );
        
        HashSet<KeyValuePair<int, int>> alreadyPassed = new();

        foreach (var item in starts)
        {
            alreadyPassed.Add(item);
        }

        while(finalPathLength == -1)
        {
            if(possiblesPath.Count == 0) throw new Exception("No quedan paths posibles");

            var actualPath = possiblesPath.Dequeue();

            var possiblesNextCell = GetNextCells2(actualPath);

            foreach(var cell in possiblesNextCell)
            {
                if(cell.Equals(end))
                {
                    finalPathLength = actualPath.Count();
                    break;
                }
                if(!alreadyPassed.Contains(cell))
                {
                    alreadyPassed.Add(cell);
                    possiblesPath.Enqueue(
                        actualPath.Append(cell).ToList(),
                        (actualPath.Count+1)*100 + (100-map[cell.Key][cell.Value])
                    );
                }
            }
        }
        System.Console.WriteLine(finalPathLength);
    }
}