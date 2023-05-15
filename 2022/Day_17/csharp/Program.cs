using System.Diagnostics;

namespace AdventOfCode;
public class Program
{
    public static void Main(string[] args)
    {
        Day_17 day = new();

        int ans = args.Length != 0 ? int.Parse(args[0]) : 1;
        bool test = args.Length == 2 ? int.Parse(args[1]) == 1 : true;

        string answer = "";

        var init = Stopwatch.GetTimestamp();

        answer = Day_17.Answer(ans, test);

        var finish = Stopwatch.GetTimestamp();

        System.Console.WriteLine($"Response: {answer}");

        System.Console.WriteLine($"Time: {Stopwatch.GetElapsedTime(init, finish)}");
    }
}
public class Day_17
{
    private static KeyValuePair<int, long>[][] BaseRocks = new KeyValuePair<int, long>[][]
    {
        new KeyValuePair<int, long>[]
        {
            new(0,0),
            new(1,0),
            new(2,0),
            new(3,0),
        },

        new KeyValuePair<int, long>[]
        {
            new(1,0),
            new(0,1),
            new(1,1),
            new(2,1),
            new(1,2),
        },

        new KeyValuePair<int, long>[]
        {
            new(0,0),
            new(1,0),
            new(2,0),
            new(2,1),
            new(2,2),
        },

        new KeyValuePair<int, long>[]
        {
            new(0,0),
            new(0,1),
            new(0,2),
            new(0,3),
        },

        new KeyValuePair<int, long>[]
        {
            new(0,0),
            new(1,0),
            new(0,1),
            new(1,1),
        },
    };
    private static string GetInput(bool test)
    {
        return test 
            ?File.ReadAllText("../AOCtest")
            :File.ReadAllText("../AOCinput");
    }

    public static string Answer(int answer, bool test)
    {
        return answer switch
        {
            1 => Answer1(test),
            2 => Answer2(test),
            _ => throw new ArgumentOutOfRangeException("No valid answer required")
        };
    }

    private static string Answer1(bool test)
    {
        var input = GetInput(test);

        const int width = 7;
        const int totalPieces = 5;

        var towers = Enumerable.Range(0, width).Select(_ => new HashSet<KeyValuePair<int, int>>()).ToArray();

        int maxHeight = -1;
        int totalWinds = input.Length;
        int actualPiecePos = 0;
        int actualWindPos = 0;

        int totalRocksFallen = 0;

        int Y = 3;
        int X = 2;

        int pipo = 0;

        KeyValuePair<int, int>[] actualPiece = BaseRocks[actualPiecePos].Select(
                block => new KeyValuePair<int, int>(block.Key + X, (int)block.Value + Y)).ToArray();

        while(true)
        {
            int actualWindDir = input[actualWindPos] == '>' ? 1 : -1;
            actualWindPos = (actualWindPos+1) % totalWinds;
            bool hit = false;

            var movedRock = actualPiece.Select(
                block => new KeyValuePair<int, int>(block.Key + actualWindDir, block.Value)).ToArray();

            foreach(var block in movedRock)
            {
                if(block.Key < 0 || block.Key >= width || towers[block.Key].Contains(block))
                {
                    hit = true;
                    break;
                }
            }
            if(!hit)
            {
                X += actualWindDir;
                actualPiece = movedRock;
            }

            hit = false;
            movedRock = actualPiece.Select(
                block => new KeyValuePair<int, int>(block.Key, block.Value - 1)).ToArray();

            foreach(var block in movedRock)
            {
                if(block.Value < 0 || towers[block.Key].Contains(block))
                {
                    hit = true;
                    break;
                }
            }
            if(hit)
            {
                foreach(var block in actualPiece)
                {
                    if(block.Value > maxHeight) maxHeight = block.Value;
                    towers[block.Key].Add(block);
                }
                Y = maxHeight + 4;
                X = 2;

                actualPiecePos = (actualPiecePos+1) % totalPieces;
                actualPiece = BaseRocks[actualPiecePos].Select(
                    block => new KeyValuePair<int, int>(block.Key + X, (int)block.Value + Y)).ToArray();

                totalRocksFallen++;
                if(totalRocksFallen == 2022) break;
            }
            else
            {
                actualPiece = movedRock;
            }
        } 

        return (maxHeight+1).ToString();
    }

    private static void PrintState(HashSet<KeyValuePair<int, int>>[] towers, int height)
    {
        for (int i = height; i >= 0; --i)
        {
            for(int j = 0; j < 7; ++j)
            {
                System.Console.Write((towers[j].Contains(new KeyValuePair<int, int>(j, i)) ? "#" : "."));
            }
            System.Console.WriteLine();
        }
        System.Console.WriteLine();
        System.Console.WriteLine();
    }

    private static string Answer2(bool test)
    {
        var input = GetInput(test);

        const int width = 7;
        const int totalPieces = 5;

        var towers = Enumerable.Range(0, width).Select(_ => new HashSet<long>()).ToArray();

        long maxHeight = -1;
        int totalWinds = input.Length;
        int actualPiecePos = 0;
        int actualWindPos = 0;

        long totalRocksFallen = 0;

        long Y = 3;
        int X = 2;

        KeyValuePair<int, long>[] actualPiece = BaseRocks[actualPiecePos];

        while(true)
        {
            int actualWindDir = input[actualWindPos] == '>' ? 1 : -1;
            actualWindPos = (actualWindPos+1) % totalWinds;
            bool hit = false;

            foreach(var block in actualPiece)
            {
                int xValue = block.Key+X+actualWindDir;
                if(xValue < 0 || xValue >= width || towers[xValue].Contains(block.Value+Y))
                {
                    hit = true;
                    break;
                }
            }
            if(!hit)
            {
                X += actualWindDir;
            }

            hit = false;

            foreach(var block in actualPiece)
            {
                int xValue = block.Key+X;
                long yValue = block.Value+Y-1;
                if(yValue < 0 || towers[xValue].Contains(yValue))
                {
                    hit = true;
                    break;
                }
            }
            if(!hit)
            {
                Y--;
            }
            else
            {
                foreach(var block in actualPiece)
                {
                    int xValue = block.Key+X;
                    long yValue = block.Value + Y;
                    if(yValue > maxHeight) maxHeight = yValue;
                    towers[xValue].Add(yValue);
                }
                Y = maxHeight + 4;
                X = 2;

                actualPiecePos = (actualPiecePos+1) % totalPieces;
                actualPiece = BaseRocks[actualPiecePos];

                totalRocksFallen++;
                if(totalRocksFallen == 1000000000000) break;
            }
        } 

        return (maxHeight+1).ToString();
    }
}