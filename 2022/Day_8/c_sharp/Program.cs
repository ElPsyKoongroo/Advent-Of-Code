using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode;

public static class Program
{
    public static void Main()
    {
        Day_8 day = new();
        day.Answer1();
        day.Answer2();
    }
}

public class Day_8
{
    string[] inputLines;
    //string[] testLines;

    public Day_8()
    {
        inputLines = File.ReadAllLines("../AOCinput");
        //testLines = File.ReadAllLines("../AOCtest");
    }

    public void Answer1()
    {
        int[][] table = 
            inputLines
            .Select(line => line.Select(chr => chr - '0').ToArray())
            .ToArray();
            
        int size = table.Length;

        var visibles = (size*4-4) +
            table
            .SelectMany( (x,i) => x.Select((y, j) => new {x = i, y = j}))
            .Where(x=> x.x > 0 && x.x < size-1 && x.y > 0 && x.y < size-1 )
            .Where(x => 
                table[(x.x+1)..size].Select(line => line[x.y])  .All(y=> y < table[x.x][x.y]) ||
                table[0..x.x].Select(line => line[x.y])         .All(y=> y < table[x.x][x.y]) ||
                table[x.x][(x.y+1)..size]                       .All(y=> y < table[x.x][x.y]) ||
                table[x.x][0..x.y]                              .All(y=> y < table[x.x][x.y])
                )
            .Count();
        
        System.Console.WriteLine(visibles);
    }

    public void Answer2()
    {
        int[][] table = 
            inputLines
            .Select(line => line.Select(chr => chr - '0').ToArray())
            .ToArray();
            
        int size = table.Length;

        var visibles =
            table
            .SelectMany( (x,i) => x.Select((y, j) => new {x = i, y = j}))
            .Where(x=> x.x > 0 && x.x < size-1 && x.y > 0 && x.y < size-1 )
            .Select(x => 
                (table[(x.x+1)..size].Select((line, index) => new {index = index, value = line[x.y]})
                    .FirstOrDefault(y=> y.value >= table[x.x][x.y], new {index = (size - x.x - 2), value = 0}).index + 1) *

                (table[0..x.x].Select((line, index) => new {index = x.x - 1 - index, value = line[x.y]}).OrderBy(item => item.index)
                    .FirstOrDefault(y=> y.value >= table[x.x][x.y], new {index = x.x - 1, value = 0}).index + 1) *

                (table[x.x][(x.y+1)..size].Select((value, index) => new {index= index, value = value})
                    .FirstOrDefault(y=> y.value >= table[x.x][x.y], new {index = (size - x.y - 2), value = 0}).index + 1) *

                (table[x.x][0..x.y].Select((value, index) => new {index= x.y - 1 - index, value = value}).OrderBy(item => item.index)
                    .FirstOrDefault(y=> y.value >= table[x.x][x.y], new {index = x.y - 1, value = 0}).index + 1) 

                )
            .Max(); // +1 for each pair of axis, 4 in total

        System.Console.WriteLine(visibles);
    }
}