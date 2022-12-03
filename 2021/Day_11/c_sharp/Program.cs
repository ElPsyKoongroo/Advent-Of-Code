namespace Program;

public static class Program
{
    public static void Main()
    {
        Day_11 day = new();
        day.Answer2();
    }
}

public class Day_11
{
    string[] inputLines;

    public enum State
    {
        NOT_FLASHED = 0,
        FLASHING = 1,
        FLASHED = 2
    }

    public struct Octopus
    {
        public State state;
        public int Energy;

        public Octopus(int energy)
        {
            Energy = energy;
            state = State.NOT_FLASHED;
        }
    } 

    Octopus[][] table;
    public Day_11()
    {
        inputLines  = File.ReadAllLines("../AOCinput");
        CreateTable();
    }

    public void CreateTable()
    {
         table = new Octopus[10][];
        for (int i = 0; i < 10; ++i)
        {
            table[i] = new Octopus[10];
            for (int j = 0;  j < 10; ++j)
            {
                table[i][j] = new Octopus(inputLines[i][j] - '0');
            }
        }
    }
    public Queue<KeyValuePair<int, int>> AddToAdjacent(int x, int y)
    {
        Queue<KeyValuePair<int, int>> changed = new();

        for (int i = -1; i < 2; ++i)
        {
            for (int j = -1; j < 2; ++j)
            {
                if(i == 0 && j == 0) continue;
                if(Exists(x+i, y+j))
                {
                    if(table[x+i][y+j].Energy >= 9 &&
                        table[x+i][y+j].state == State.NOT_FLASHED)
                        {
                            table[x+i][y+j].state = State.FLASHING;
                            changed.Enqueue(new KeyValuePair<int, int>(x+i, y+j));
                        }
                    table[x+i][y+j].Energy++;
                }
            }
        }
        return changed;
    }
    private bool Exists(int i, int j) => (i, j) switch
        {
            (<0 or > 9, _) => false,
            (_, <0 or > 9) => false,
            _ => true
        };

    private Queue<KeyValuePair<int, int>> AddToAll()
    {
        Queue<KeyValuePair<int,int>> changed = new();

        for(int i = 0; i < 10; i ++)
        {
            for (int j = 0; j < 10; j++)
            {
                if(table[i][j].Energy == 9)
                {
                    table[i][j].state = State.FLASHING;
                    changed.Enqueue(new KeyValuePair<int, int>(i,j));
                }

                table[i][j].Energy++;
            }
        }
        return changed;
    }

    private void ResetStateAndEnergy()
    {
        for(int i = 0;i < 10; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                table[i][j].state = State.NOT_FLASHED;

                if(table[i][j].Energy > 9)
                    table[i][j].Energy = 0;
            }
        }
    }
    public void Answer1()
    {
        ulong totalFlashes = 0;
        for(int i = 0; i < 100; ++i)
        {
            var changed = AddToAll();

            while(changed.Count != 0)
            {
                (int x, int y) = changed.Dequeue();

                totalFlashes++;

                table[x][y].state = State.FLASHED;

                var adjacent = AddToAdjacent(x, y);
                changed = new Queue<KeyValuePair<int, int>>(changed.Concat(adjacent));

            }
            ResetStateAndEnergy();
        }

        System.Console.WriteLine(totalFlashes);
    }

    public void Answer2()
    {
        int resultIteration = -1;

        for(int i = 0; resultIteration == -1; ++i)
        {
            var changed = AddToAll();
            int totalFlashes = 0;
            while(changed.Count != 0)
            {
                (int x, int y) = changed.Dequeue();

                totalFlashes++;

                table[x][y].state = State.FLASHED;

                var adjacent = AddToAdjacent(x, y);
                changed = new Queue<KeyValuePair<int, int>>(changed.Concat(adjacent));

            }
            if(totalFlashes == 100 && resultIteration == -1) resultIteration = i+1;
            ResetStateAndEnergy();
        }

        System.Console.WriteLine(resultIteration);
    }
}