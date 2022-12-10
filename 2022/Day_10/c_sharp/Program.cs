namespace AdventOfCode;

public static class Program
{
    public static void Main()
    {
        Day_10 day = new();
        //day.Answer1();
        day.Answer2();
    }
}


public class Day_10
{
    string[] inputLines;
    string[] testLines;
    public Day_10()
    {
        inputLines = File.ReadAllLines("../AOCinput");
        testLines = File.ReadAllLines("../AOCtest");
    }

    public void Answer1()
    {
        int regX = 1;
        int cycle = 0;
        int actualInstruction = 0;
        int totalSum = 0;
        int cyclesAdding = 0;

        int totalInstructions = inputLines.Length-1;

        while(true)
        {
            if(actualInstruction == totalInstructions) break;

            ++cycle;

            if((cycle-20)%40 == 0 && cycle <= 220) totalSum += regX * cycle;

            if(cyclesAdding != 0)
            {
                regX += int.Parse(inputLines[actualInstruction].Split(" ")[1]);
                ++actualInstruction;
                cyclesAdding = 0;
                continue;
            }

            string[] input = inputLines[actualInstruction].Split(" ");

            if(input[0] == "noop")
            {
                ++actualInstruction;
                continue;
            }

            cyclesAdding = 1;
        }

        System.Console.WriteLine(totalSum);
    }

    public void Answer2()
    {
        int regX = 1;
        int cycle = 0;
        int actualInstruction = 0;
        int cyclesAdding = 0;

        int totalInstructions = inputLines.Length-1;
        const int totalCycles = 240;

        string[] message = new string[6];

        while(true)
        {
            if(actualInstruction > totalInstructions) break;

            message[cycle/40] += (cycle%40 >= (regX-1) && cycle%40 <= (regX+1)) ? '#' : '.';
            
            ++cycle;

            if(cycle > totalCycles) break;


            if(cyclesAdding != 0)
            {
                regX += int.Parse(inputLines[actualInstruction].Split(" ")[1]);
                ++actualInstruction;
                cyclesAdding = 0;
                continue;
            }

            string[] input = inputLines[actualInstruction].Split(" ");

            if(input[0] == "noop")
            {
                ++actualInstruction;
                continue;
            }

            cyclesAdding = 1;
        }

        for(int i = 0; i < 6; ++i)
        {
            for(int j = 0; j < 40; ++j)
            {
                if(j%5 == 0) System.Console.Write("\t");
                System.Console.Write(message[i][j]);
            }
            System.Console.WriteLine();
        }

        /*foreach (var line in message)
        {
            System.Console.WriteLine(line);
        }*/
    }
}