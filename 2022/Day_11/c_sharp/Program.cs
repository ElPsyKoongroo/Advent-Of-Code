using System.Linq.Expressions;

namespace AdventOfCode;

public static class Program
{
    public static void Main()
    {
        Day_11 day = new();
        //day.Answer1();
        day.Answer2();
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
        string[] monkeyData = testText.Split("\r\n\r\n");

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
                    (ulong newItem, int monkeyToAdd) = monke.InspectNext1();
                    monkeys[monkeyToAdd].items.Enqueue(newItem);
                }
            }
        }

        monkeys = monkeys.OrderByDescending(x=> x.total).ToList();

        System.Console.WriteLine(monkeys[0].total);
        System.Console.WriteLine(monkeys[1].total);

        ulong total = monkeys[0].total * monkeys[1].total;

        System.Console.WriteLine(total);
    }

    public void Answer2()
    {
        string[] monkeyData = inputText.Split("\r\n\r\n");

        List<Monkey> monkeys = new();

        for(int i = 0; i < monkeyData.Length; ++i)
        {
            monkeys.Add(new Monkey(monkeyData[i], i));
        }

        for(int i = 0; i < 10_000; ++i)
        {
            foreach(var monke in monkeys)
            {
                while(monke.items.Count != 0)
                {
                    (ulong newItem, int monkeyToAdd) = monke.InspectNext2();
                    monkeys[monkeyToAdd].items.Enqueue(newItem);
                }
            }
        }

        monkeys = monkeys.OrderByDescending(x=> x.total).ToList();

        System.Console.WriteLine(monkeys[0].total);
        System.Console.WriteLine(monkeys[1].total);

        UInt64 total = (UInt64)monkeys[0].total * (UInt64)monkeys[1].total;

        System.Console.WriteLine(total);
    }
}

public class Monkey
{
    static ulong Module2 = 1;
    int monkey;
    public Queue<ulong> items;
    private Func<ulong, ulong> operation;
    private Func<ulong, int> test;

    public ulong total;

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
        items = new Queue<ulong>(itemsLine.Split(":")[1].Split(",").Select(x=> ulong.Parse(x.Trim())));
    }

    private void GenerateOperation(string opLine)
    {
        string[] opData = opLine.Split("=")[1].Trim().Split(" ");
        ulong constant;

        int oldPosition = -1; //If 0, is first argument, if 1, second argument, if 2, both arguments

        ConstantExpression constParam = Expression.Constant(1UL, typeof(ulong));

        if(opData[0] == "old") oldPosition = 0;
        if(opData[2] == "old")
        {
            if(oldPosition == 0) oldPosition = 2;
            else oldPosition = 1;
        }

        if(oldPosition != 2)
        {
            if(ulong.TryParse(opData[0], out constant));
            else if(ulong.TryParse(opData[2], out constant));

            constParam = Expression.Constant(constant, typeof(ulong));
        }

        ParameterExpression oldParam = Expression.Parameter(typeof(ulong), "old");
        
        BinaryExpression operationExp = (opData[1], oldPosition) switch
        {
            ("+", 0) => Expression.Add(constParam, oldParam),
            ("+", 1) => Expression.Add(oldParam, constParam),
            ("+", 2) => Expression.Add(oldParam, oldParam),
            
            ("*", 0) => Expression.Multiply(constParam, oldParam),
            ("*", 1) => Expression.Multiply(oldParam, constParam),
            ("*", 2) => Expression.Multiply(oldParam, oldParam)
        };

        operation = Expression.Lambda<Func<ulong, ulong>>(operationExp, new ParameterExpression[]{oldParam}).Compile();        
    }

    private void GenerateTest(string[] testLines)
    {
        ulong numberToDivide = ulong.Parse(testLines[0].Split("by")[1].Trim());

        Module2 *= numberToDivide;

        int whenTrue = int.Parse(testLines[1].Split("monkey")[1].Trim());
        int whenFalse = int.Parse(testLines[2].Split("monkey")[1].Trim());

        test = (ulong toTest) => toTest%numberToDivide == 0 ? whenTrue : whenFalse;
    }

    public int Test(ulong num)
    {
        return test(num);
    }

    public ulong ChangeWorryLevel(ulong worryLevel)
    {
        return operation(worryLevel);
    }
    
    public (ulong, int) InspectNext1()
    {
        total++;
        ulong actualItem = items.Dequeue();

        actualItem = ChangeWorryLevel(actualItem);
        actualItem /=3;
        return (actualItem, Test(actualItem));
    }

    public (ulong, int) InspectNext2()
    {
        total++;
        ulong actualItem = items.Dequeue();

        actualItem = ChangeWorryLevel(actualItem);
        actualItem = actualItem % Module2;
        return (actualItem, Test(actualItem));
    }
}