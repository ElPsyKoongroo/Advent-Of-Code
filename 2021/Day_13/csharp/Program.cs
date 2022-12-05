namespace AdventOfCode;


public static class Program
{
    public static void Main()
    {
        Day_13 day = new();
        day.Answer1();
    }
}

public class Day_13
{
    string input;
    public Day_13()
    {
        input = File.ReadAllText("../AOCinput");
    }

    public void Answer1()
    {
        var table = GetTable();

        var folds = 
            input
            .Split("\n\n")[1]
            .Split("\n")
            .Select(line => 
                line
                .Split(" ")[^1]
                .Split("=")
            )
            .ToArray();
        
        for(int fold = 0; fold < 1; ++fold)
        {
            var actualFold = folds[fold];

            int foldNum = int.Parse(actualFold[1]);

            bool[][] newTable;

            if(actualFold[0] == "x")        //FOLD X
            {
                int tableY = table.Length;
                newTable  = CreateNewTable(foldNum, tableY);
                for(int i = 0; i < tableY; ++i)
                {
                    for(int j = 1; j <= foldNum; ++j)
                    {
                        newTable[i][foldNum-j] = (table[i][foldNum-j] || table[i][foldNum+j]);
                    }
                }
            }
            else                            //FOLD Y
            {
                int tableX = table[0].Length;
                newTable  = CreateNewTable(tableX, foldNum);
                for(int i = 1; i <= foldNum; ++i)
                {
                    for(int j = 0; j < tableX; ++j)
                    {
                        newTable[foldNum-i][j] = (table[foldNum-i][j] || table[foldNum+i][j]);
                    }
                }
            }
            table = newTable;
        }

        int visible = CountVisibles(table);

        System.Console.WriteLine(visible);
    }

    public void Answer2()
    {
        var table = GetTable();

        var folds = 
            input
            .Split("\n\n")[1]
            .Split("\n")
            .Select(line => 
                line
                .Split(" ")[^1]
                .Split("=")
            )
            .ToArray();
        
        for(int fold = 0; fold < folds.Length; ++fold)
        {
            var actualFold = folds[fold];

            int foldNum = int.Parse(actualFold[1]);

            bool[][] newTable;

            if(actualFold[0] == "x")        //FOLD X
            {
                int tableY = table.Length;
                newTable  = CreateNewTable(foldNum, tableY);
                for(int i = 0; i < tableY; ++i)
                {
                    for(int j = 1; j <= foldNum; ++j)
                    {
                        newTable[i][foldNum-j] = (table[i][foldNum-j] || table[i][foldNum+j]);
                    }
                }
            }
            else                            //FOLD Y
            {
                int tableX = table[0].Length;
                newTable  = CreateNewTable(tableX, foldNum);
                for(int i = 1; i <= foldNum; ++i)
                {
                    for(int j = 0; j < tableX; ++j)
                    {
                        newTable[foldNum-i][j] = (table[foldNum-i][j] || table[foldNum+i][j]);
                    }
                }
            }
            table = newTable;
        }

        int fontSize = table[0].Length/8;

        System.Console.WriteLine("\n");

        for(int i = 0; i < table.Length; ++i)
        {
            for(int j = 0; j < table[0].Length; ++j)
            {
                if(j != 0 && j%fontSize == 0) System.Console.Write("\t\t");
                System.Console.Write(table[i][j] ? '@' : ' ');
            }
            System.Console.WriteLine(Environment.NewLine);
        }
        System.Console.WriteLine();
    }

    private bool[][] CreateNewTable(int x, int y)
    {
        bool[][] table = new bool[y][];
        for (int i = 0; i < y; ++i)
        {
            table[i] = new bool[x];
        }
        return table;
    }

    private int CountVisibles(bool[][] table)
    {
        int total = 0;

        foreach(var line in table)
        {
            foreach (var dot in line)
            {
                if(dot) ++total;
            }
        }
        return total;
    }

    private bool[][] GetTable()
    {
        var data = 
            input
            .Split("\n\n")[0]
            .Split("\n")
            .Select(_x=>
            {
                (var x, var y) = _x.Split(",");
                return (x,y);
            })
            .ToList();
        
        int maxX = data.Max(dot=> dot.x) + 1;
        int maxY = data.Max(dot=> dot.y) + 1;

        bool[][] table = new bool[maxY][];

        for(int i = 0; i < maxY; ++i)
        {
            table[i] = new bool[maxX];

            for(int j = 0; j < maxX; ++j)
            {
                if(data.Any(dot => dot.y == i && dot.x == j))
                    table[i][j] = true;
                
                else table[i][j] = false;
            }
        }
        return table;
    }
}
public static class Extension
{
    public static void Deconstruct(this string[] array, out int first, out int second)
    {
        first = int.Parse(array[0]);
        second = int.Parse(array[1]);
    }
}