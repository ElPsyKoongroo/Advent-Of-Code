using System.Linq.Expressions;

namespace AdventOfCode;

public static class Program
{
    public static void Main()
    {
        Day_11 day = new();
        day.Answer1();
    }
}

public class Day_11
{
    string testText;
    string inputText;
    public Day_11()
    {
        testText = File.ReadAllText("../AOCtest");
        inputText = File.ReadAllText("../AOCinput");
    }

    public void Answer1()
    {
        string[] monkeyData = inputText.Split("\r\n\r\n");

        List<Monkey> monkeys = new();

        for(int i = 0; i < monkeyData.Length; ++i)
        {
            monkeys.Add(new Monkey(monkeyData[i], i));
        }

        for(int i = 0; i < 20; ++i)
        {
            foreach(var monke in monkeys)
            {
                while(monke.items.Count != 0)
                {
                    (int newItem, int monkeyToAdd) = monke.InspectNext();
                    monkeys[monkeyToAdd].items.Enqueue(newItem);
                }
            }
        }

        monkeys = monkeys.OrderByDescending(x=> x.total).ToList();

        System.Console.WriteLine(monkeys[0].total);
        System.Console.WriteLine(monkeys[1].total);

        int total = monkeys[0].total * monkeys[1].total;

        System.Console.WriteLine(total);
    }

    public void Answer2()
    {
        
    }
}

public class Monkey
{
    int monkey;
    public Queue<int> items;
    private Func<int, int> operation;
    private Func<int, int> test;

    public int total;

    public Monkey(string monkeyInfo, int _monkey)
    {
        monkey = _monkey;
        string[] infoAsLines = monkeyInfo.Split("\n");

        GenerateItems(infoAsLines[1]);
        GenerateOperation(infoAsLines[2]);
        GenerateTest(infoAsLines[3..6]);

        total = 0;
    }

    private void GenerateItems(string itemsLine)
    {
        items = new Queue<int>(itemsLine.Split(":")[1].Split(",").Select(x=> int.Parse(x.Trim())));
    }

    private void GenerateOperation(string opLine)
    {
        string[] opData = opLine.Split("=")[1].Trim().Split(" ");
        int constant;

        int oldPosition = -1; //If 0, is first argument, if 1, second argument, if 2, both arguments

        ConstantExpression constParam = Expression.Constant(1, typeof(int));

        if(opData[0] == "old") oldPosition = 0;
        if(opData[2] == "old")
        {
            if(oldPosition == 0) oldPosition = 2;
            else oldPosition = 1;
        }

        if(oldPosition != 2)
        {
            if(int.TryParse(opData[0], out constant));
            else if(int.TryParse(opData[2], out constant));

            constParam = Expression.Constant(constant, typeof(int));
        }

        ParameterExpression oldParam = Expression.Parameter(typeof(int), "old");
        
        BinaryExpression operationExp = (opData[1], oldPosition) switch
        {
            ("+", 0) => Expression.Add(constParam, oldParam),
            ("+", 1) => Expression.Add(oldParam, constParam),
            ("+", 2) => Expression.Add(oldParam, oldParam),
            
            ("*", 0) => Expression.Multiply(constParam, oldParam),
            ("*", 1) => Expression.Multiply(oldParam, constParam),
            ("*", 2) => Expression.Multiply(oldParam, oldParam)
        };

        operation = Expression.Lambda<Func<int, int>>(operationExp, new ParameterExpression[]{oldParam}).Compile();        
    }

    private void GenerateTest(string[] testLines)
    {
        int numberToDivide = int.Parse(testLines[0].Split("by")[1].Trim());

        int whenTrue = int.Parse(testLines[1].Split("monkey")[1].Trim());
        int whenFalse = int.Parse(testLines[2].Split("monkey")[1].Trim());

        test = (int toTest) => toTest%numberToDivide == 0 ? whenTrue : whenFalse;
    }

    public int Test(int num)
    {
        return test(num);
    }

    public int ChangeWorryLevel(int worryLevel)
    {
        return operation(worryLevel);
    }
    
    public (int, int) InspectNext()
    {
        total++;
        int actualItem = items.Dequeue();

        actualItem = ChangeWorryLevel(actualItem);
        actualItem /=3;
        return (actualItem, Test(actualItem));
    }
}