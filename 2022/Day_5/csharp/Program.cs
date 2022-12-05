namespace AdventOfCode;

using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;

public static class Program
{
    public static void Main()
    {
        Day_5 day = new();

        System.Console.WriteLine(day.Answer2());
    }
}

public class Day_5
{
    string inputText;

    public Day_5()
    {
        inputText = File.ReadAllText("../AOCinput");
    }

    private List<Stack<char>> GetInput()
    {
        List<Stack<char>> inputList = new();

        for(int i = 0; i < 9; ++i)
        {
            inputList.Add(new Stack<char>());
        }

        var stacks = inputText.Split("\n\n")[0].Split("\n")[0..^1];
        
        for(int i = stacks.Length-1; i >= 0; --i)
        {
            for (int j = 0; j < 9; ++j)
            {
                if(stacks[i][1 + j*4] != ' ')
                {
                    inputList[j].Push(stacks[i][1 + j*4]);
                }
            }
        }
        return inputList;
    }

    public string Answer1()
    {
        var input = GetInput();

        var moves = 
            inputText
            .Split("\n\n")[1]
            .Split("\n")
            .Select(line => 
                line
                .Split(" ")
                .Where(x=> int.TryParse(x, out int a))
                .Select(x=> int.Parse(x))
                .ToArray()
            )
            .ToArray();

        foreach(var move in moves)
        {
            for(int i = 0; i < move[0]; ++i)
            {
                var element = input[move[1]-1].Pop();
                input[move[2]-1].Push(element);
            }
        }

        StringBuilder response = new();

        foreach(var stack in input)
        {
            response.Append(stack.Pop());
        }

        return response.ToString();
    }

    public string Answer2()
    {
        var input = GetInput();

        var moves = 
            inputText
            .Split("\n\n")[1]
            .Split("\n")
            .Select(line => 
                line
                .Split(" ")
                .Where(x=> int.TryParse(x, out int a))
                .Select(x=> int.Parse(x))
                .ToArray()
            )
            .ToArray();

        foreach(var move in moves)
        {
            Stack<char> aux = new();
            for(int i = 0; i < move[0]; ++i)
            {
                var element = input[move[1]-1].Pop();
                aux.Push(element);
            }

            for(int i = 0; i < move[0]; ++i)
            {
                var element = aux.Pop();
                input[move[2]-1].Push(element);
            }
        }

        StringBuilder response = new();

        foreach(var stack in input)
        {
            response.Append(stack.Pop());
        }

        return response.ToString();
    }
}