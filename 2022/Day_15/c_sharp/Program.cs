using System.Diagnostics;

namespace AdventOfCode;

public class Program
{
    public static void Main(string[] args)
    {
        Day_15 day = new();
        day.Answer2();
    }
}

public class Day_15
{
    string[] testLines;
    string[] inputLines;

    public Day_15()
    {
        testLines = File.ReadAllLines("../AOCtest");
        inputLines = File.ReadAllLines("../AOCinput");
    }

    /// <param name="test"> True for sample input, false for real input </param>

    private (List<KeyValuePair<int,int>>, List<KeyValuePair<int,int>>) ParseInput(bool test = false)
    {
        var input = test ? testLines : inputLines;

        List<KeyValuePair<int, int>> sensors = new();
        List<KeyValuePair<int, int>> beacons = new();

        foreach (var line in input)
        {
            var words = line.Split("=");
            int s_x, s_y, b_x, b_y;

            s_x = int.Parse(string.Concat(words[1].TakeWhile(x=> x != ',')));
            s_y = int.Parse(string.Concat(words[2].TakeWhile(x=> x != ':')));
            b_x = int.Parse(string.Concat(words[3].TakeWhile(x=> x != ',')));
            b_y = int.Parse(words[4]);

            sensors.Add(new KeyValuePair<int, int>(s_x, s_y));
            beacons.Add(new KeyValuePair<int, int>(b_x, b_y));
        }

        return (sensors, beacons);
    }

    private int GetDistance(KeyValuePair<int, int> first, KeyValuePair<int, int> second)
    {
        return Math.Abs(first.Key - second.Key) + Math.Abs(first.Value - second.Value);
    }

    private int GetDistance(KeyValuePair<int, int> first, int x, int y)
    {
        return Math.Abs(first.Key - x) + Math.Abs(first.Value - y);
    }

    public void Answer1()
    {
        bool test = false;
        (var sensors, var beacons) = ParseInput(test);

        HashSet<KeyValuePair<int, int>> checkedCells = new();

        foreach ((var sensor, var beacon) in sensors.Zip(beacons))
        {
            int distance = GetDistance(sensor, beacon);

            for(int i = distance; i >= Math.CopySign(distance, -1); --i)
            {
                if(sensor.Value + i != (test ? 10 : 2000000))
                    continue;
                if(i != 0)
                    checkedCells.Add(new KeyValuePair<int, int>(sensor.Key, sensor.Value+i));
                for(int j = 1; j <= distance - Math.Abs(i); ++j)
                {
                    checkedCells.Add(new KeyValuePair<int, int>(sensor.Key+j, sensor.Value+i));
                    checkedCells.Add(new KeyValuePair<int, int>(sensor.Key-j, sensor.Value+i));
                }
            }
        }
        checkedCells = checkedCells.Except<KeyValuePair<int, int>>(beacons).ToHashSet();
        
        int totalInLine = checkedCells.Count();

        System.Console.WriteLine(totalInLine);
    }

    public void Answer2()
    {
        bool test = false;
        int limit = (test ? 20 : 4000000);

        // bool InRange(KeyValuePair<int, int> pos)
        // {
        //     return (pos.Key >= 0 && pos.Value >= 0 && pos.Key <= limit && pos.Value <= limit);
        // }

        bool InRange(int x, int y)
        {
            return (x >= 0 && y >= 0 && x <= limit && y <= limit);
        }

        (var sensors, var beacons) = ParseInput(test);


        var distances = sensors.Zip(beacons).Select(x=> (x.First, GetDistance(x.First, x.Second))).ToArray();

        KeyValuePair<int, int> result = new(0,0);

        Stopwatch sp = Stopwatch.StartNew();

        foreach ((var sensor, var dis) in distances)
        {
            int border = dis + 1;

            int x = sensor.Key + 1;
            int y = sensor.Value - border + 1;
            int dir_x = 1;
            int dir_y = 1;

            while(true)
            {
                
                if(InRange(x, y))
                {
                    for(int i = 0; i < distances.Length; ++i)
                    {
                        if(GetDistance(distances[i].First, x, y) <= distances[i].Item2)
                            goto next;
                        
                    }
                    result = new KeyValuePair<int, int>(x, y);
                    goto exit;
                }
                next:
                
                if(x == sensor.Key && y == sensor.Value - border)
                {
                    break;
                }
                if(x == sensor.Key && y == sensor.Value+border)
                {
                    dir_y = -1;
                }
                else if(x == sensor.Key + border && y == sensor.Value)
                {
                    dir_x = -1;
                }
                else if(x == sensor.Key - border && y == sensor.Value)
                {
                    dir_x = +1;
                }
                x+=dir_x;
                y+=dir_y;
            }
        }
        exit:

        System.Console.WriteLine(sp.ElapsedMilliseconds/1000.0);
        const long x_mult = 4000000;

        long finalValue = result.Key * x_mult + result.Value;

        System.Console.WriteLine(finalValue);
    }
}