namespace Advent_Of_Code;

public class EJ10 : EJ
{
    private Stack<int> order;

    private List<char> opening;
    private List<char> closing;
    private int[] values;

    public EJ10()
    {
        values = new[] { 3, 57, 1197, 25137 };
        opening = new() { '(', '[', '{', '<' };
        closing = new() { ')', ']', '}', '>' };
    }

    public void EJ10_1()
    {
        var count = 0;

        foreach (var str in input)
        {
            order = new();
            
            foreach (var chr in str)
            {
                if (opening.Contains(chr))
                {
                    order.Push(opening.IndexOf(chr));
                    continue;
                }

                int index = closing.IndexOf(chr);

                if (order.Peek() != index)
                {
                    count += values[index];
                    break;
                }

                order.Pop();
            }
        }
        Console.WriteLine(count);
    }
    
    public void EJ10_2()
    {
        long count;
        bool corrupted;
        List<long> incomplete = new();

        foreach (var str in input)
        {
            count = 0;
            order = new();
            corrupted = false;
            
            foreach (var chr in str)
            {
                if (opening.Contains(chr))
                {
                    order.Push(opening.IndexOf(chr));
                    continue;
                }

                int index = closing.IndexOf(chr);

                if (order.Peek() != index)
                {
                    corrupted = true;
                    break;
                }
                order.Pop();
            }
            if (corrupted) continue;

            while (order.Count > 0)
            {
                count *= 5;
                count += order.Pop()+1;
            }
            incomplete.Add(count);
        }

        long medio;
        incomplete.Sort();
        
        medio = incomplete[incomplete.Count / 2];
        Console.WriteLine(medio);
        
    }
}