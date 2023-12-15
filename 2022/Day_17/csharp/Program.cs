using System.Diagnostics;
using System.Xml;

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

    private static void PrintState(HashSet<(int, int)>[] towers, int height)
    {
        for (int i = height; i >= 0; --i)
        {
            for(int j = 0; j < 7; ++j)
            {
                System.Console.Write((towers[j].Contains((j, i)) ? "#" : "."));
            }
            System.Console.WriteLine();
        }
        System.Console.WriteLine();
        System.Console.WriteLine();
    }

    private static (int, int)[] SaveState(HashSet<(int X, int Y)>[] towers, int saveSize, int height) {
        return 
            towers
            .SelectMany(t => 
                t.Where(x => 
                    x.Y > height-saveSize
                )
                .Select(x=> (x.X, x.Y-(height-saveSize)))
            ).ToArray();
    }
    
    private static bool CheckAlreadySeen(
        (int X, int Y)[] actual, 
        List<((int X, int Y)[], int wind, int rock, int Y, long totalRocksFallen)> seen, 
        int actualWindPos, int actualPiecePos,
        out int last)
    {
        last = -1;
        foreach(var ((towers, wind, rock, _, _), i) in seen.Zip(Enumerable.Range(0, seen.Count))) {
            if(towers.Length != actual.Length) continue;
            if(actualWindPos != wind) continue;
            if (actualPiecePos != rock) continue;

            bool ok = true;
            foreach(var pos in towers) {
                if(!actual.Contains(pos))  {
                    ok = false;
                    break; 
                }
            }
            if(!ok) continue;
            last = i;
            return true;
        }
        return false;
    }


    private static string Answer2(bool test)
    {
        var input = GetInput(test);

        const int width = 7;
        const int totalPieces = 5;

        var towers = Enumerable.Range(0, width).Select(_ => new HashSet<(int X, int Y)>()).ToArray();

        int maxHeight = -1;
        int totalWinds = input.Length;
        int actualPiecePos = 0;
        int actualWindPos = 0;

        long totalRocksFallen = 0;
        int saveSize = 30;

        long totalIters = 1_000_000_000_000;

        int Y = 3;
        int X = 2;
        bool skiped = false;
        long add = 0;
        List<((int x, int y)[], int wind, int rock, int y, long totalRocks)> seen = [];


        (int X, int Y)[] actualPiece = BaseRocks[actualPiecePos].Select(
                block => (block.Key + X, (int)block.Value + Y)).ToArray();

        while(true)
        {
            if(totalRocksFallen == totalIters) break;
            if(!skiped && maxHeight > saveSize) {
                var actual = SaveState(towers, saveSize, maxHeight);

                if(CheckAlreadySeen(actual, seen, actualWindPos, actualPiecePos, out int last) 
                    /*&& totalRocksFallen > 2022*/) {
                    skiped = true;
                    // last++;
                    var dy = (Y-4) - seen[last].y;
                    var dr = totalRocksFallen - seen[last].totalRocks;
                    long amt = (totalIters-totalRocksFallen)/dr;
                    add = amt*dy;
                    totalRocksFallen += amt*dr;
                    // actualPiecePos = (int)(totalRocksFallen%5);
                    continue;
                }
                seen.Add((actual, actualWindPos, actualPiecePos, maxHeight, totalRocksFallen));
            }
            int actualWindDir = input[actualWindPos] == '>' ? 1 : -1;
            actualWindPos = (actualWindPos+1) % totalWinds;
            bool hit = false;

            (int Key, int Value)[] movedRock = actualPiece.Select(
                block => (block.X + actualWindDir, block.Y)).ToArray();

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
                block => (block.X, block.Y - 1)).ToArray();

            foreach(var block in movedRock)
            {
                if(block.Value < 0 || towers[block.Key].Contains(block))
                {
                    hit = true;
                    break;
                }
            }
            if(!hit)
            {
                actualPiece = movedRock;
                continue;
            }
            foreach(var block in actualPiece)
            {
                if(block.Y > maxHeight) maxHeight = block.Y;
                towers[block.X].Add(block);
            }
            Y = maxHeight + 4;
            X = 2;

            actualPiecePos = (actualPiecePos+1) % totalPieces;
            actualPiece = BaseRocks[actualPiecePos].Select(
                block => (block.Key + X, (int)block.Value + Y)).ToArray();

            totalRocksFallen++;
        } 

        // System.Console.WriteLine(
        //     "Se fini"
        // );

        // PrintState(towers, maxHeight);
        // System.Console.WriteLine(add);
        return (maxHeight+1+add).ToString();
    }

}