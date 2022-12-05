namespace AdventOfCode;

public static class Program
{
    public static void Main()
    {
        Day_4 day = new();

        System.Console.WriteLine(day.Answer2());
    }
}

public class Day_4
{
    string[] inputLines;
    public Day_4()
    {
        inputLines = File.ReadAllLines("../AOCinput");
    }

    public int Answer1()
    {
        int total = 0;
        foreach(var line in inputLines)
        {
            string[] pairStr = line.Split(",");

            int[][] pair = new int[][] 
            {
                pairStr[0].Split("-").Select(x=> int.Parse(x)).ToArray(),
                pairStr[1].Split("-").Select(x=> int.Parse(x)).ToArray()
            };
            
        if(pair[0][0] < pair[1][0])             //Maybe 1 in 0
            {
                if(pair[0][1] >= pair[1][1]) total++;
            }
            else if(pair[0][0] >  pair[1][0])   //Maybe 0 in 1
            {
                if(pair[0][1] <= pair[1][1]) total++;
            }
            else total++; 
            //If two pairs starts in the same section,
            //  or one of them contains the other, 
            //  or they are the same
        }
        return total;
    }

    public int Answer2()
    {
        int total = 0;
        foreach(var line in inputLines)
        {
            string[] pairStr = line.Split(",");

            int[][] pair = new int[][] 
            {
                pairStr[0].Split("-").Select(x=> int.Parse(x)).ToArray(),
                pairStr[1].Split("-").Select(x=> int.Parse(x)).ToArray()
            };
            
            if(pair[0][0] < pair[1][0])   
            {
                if(pair[0][1] >= pair[1][0]) total++;
            }
            else if(pair[0][0] >  pair[1][0])
            {
                if(pair[0][0] <= pair[1][1]) total++;
            }
            else total++;
        }
        return total;
    }
}