namespace Advent_Of_Code;

public class EJ9 : EJ
{
    int _x, _y;
    int[][] tabla;
    List<KeyValuePair<int, int>> checkeds;

    private bool exists(int x, int y)
    {
        return (x >= 0 && x < _x) && (y >= 0 && y < _y);
    }

    public void EJ()
    {
        _x = input.Count;
        _y = input[0].Length;

        tabla = new int[_x][];

        for (int i = 0; i < _x; i++)
        {
            tabla[i] = new int[_y];

            for (int j = 0; j < _y; j++)
                tabla[i][j] = int.Parse(input[i][j].ToString());
        }

        int total = 0;

        for (int i = 0; i < _x; i++)
        {
            for (int j = 0; j < _y; j++)
            {
                if (exists(i - 1, j) && tabla[i - 1][j] <= tabla[i][j])
                    continue;
                if (exists(i + 1, j) && tabla[i + 1][j] <= tabla[i][j])
                    continue;
                if (exists(i, j - 1) && tabla[i][j - 1] <= tabla[i][j])
                    continue;
                if (exists(i, j + 1) && tabla[i][j + 1] <= tabla[i][j])
                    continue;

                total += tabla[i][j] + 1;
            }
        }
        Console.WriteLine(total);
    }
    public void EJ2()
    {
        _x = input.Count;
        _y = input[0].Length;

        tabla = new int[_x][];

        for (int i = 0; i < _x; i++)
        {
            tabla[i] = new int[_y];

            for (int j = 0; j < _y; j++)
                tabla[i][j] = int.Parse(input[i][j].ToString());
        }

        List<KeyValuePair<int, int>> minimos = new();

        for (int i = 0; i < _x; i++)
        {
            for (int j = 0; j < _y; j++)
            {
                if (exists(i - 1, j) && tabla[i - 1][j] <= tabla[i][j])
                    continue;
                if (exists(i + 1, j) && tabla[i + 1][j] <= tabla[i][j])
                    continue;
                if (exists(i, j - 1) && tabla[i][j - 1] <= tabla[i][j])
                    continue;
                if (exists(i, j + 1) && tabla[i][j + 1] <= tabla[i][j])
                    continue;

                minimos.Add(new KeyValuePair<int, int>(i, j));
            }
        }

        List<int> top3 = new() { 1, 1, 1};
        checkeds = new();

        foreach (var min in minimos)
        {
            //checkeds.Clear();
            int aux = size(min.Key, min.Value);

            //Console.WriteLine($"---{aux}");

            if(top3.ElementAt(0) < aux)
            {
                top3.Add(aux);
                top3.Sort();
                top3.RemoveAt(0);
            }
        }
        int total = 1;
        top3.ForEach(x => total *= x);
        Console.WriteLine(total);
    }

    public int size(int i, int j)
    {
        int _size = 1;

        if (exists(i - 1, j) 
            && tabla[i - 1][j] > tabla[i][j] 
            && 9 != tabla[i - 1][j] 
            && !checkeds.Contains(new KeyValuePair<int, int>(i-1,j)))
        {
            checkeds.Add(new KeyValuePair<int, int>(i-1, j));
            _size += size(i - 1, j);
        }
        if (exists(i + 1, j) 
            && tabla[i + 1][j] > tabla[i][j] 
            && 9 != tabla[i + 1][j] 
            && !checkeds.Contains(new KeyValuePair<int, int>(i + 1, j)))
        {
            checkeds.Add(new KeyValuePair<int, int>(i + 1, j));
            _size += size(i + 1, j);
        }
        if (exists(i , j - 1) 
            && tabla[i ][j - 1] > tabla[i][j] 
            && 9 != tabla[i][j - 1] 
            && !checkeds.Contains(new KeyValuePair<int, int>(i, j - 1)))
        {
            checkeds.Add(new KeyValuePair<int, int>(i, j - 1));
            _size += size(i, j - 1);
        }
        if (exists(i, j + 1) 
            && tabla[i ][j + 1] > tabla[i][j] 
            && 9 != tabla[i][j + 1] 
            && !checkeds.Contains(new KeyValuePair<int, int>(i, j + 1)))
        {
            checkeds.Add(new KeyValuePair<int, int>(i, j + 1));
            _size += size(i, j + 1);
        }

        return _size;
    }

}