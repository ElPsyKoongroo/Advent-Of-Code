namespace Advent_Of_Code;

public class EJ8 : EJ
{
    /* num  - seg
     *  2      5 ///////
     *  3      5 ///////
     *  5      5 ///////
     *  0      6 +++++++
     *  6      6 +++++++
     *  9      6 +++++++
     *  1      2 -------
     *  4      4 -------
     *  7      3 -------
     *  8      7 -------
     */



    public void EJ()
    {
        List<List<string>> outputs = input
            .Select(x => x.Split("|")[1]
            .Trim()
            .Split(" ")
            .ToList()
            )
            .ToList();

        int total = 0;

        outputs.ForEach(x =>
        {
            x.ForEach(y =>
            {
                if (new int[] { 2, 4, 3, 7 }.Contains(y.Length))
                    total++;
            });
        });

        Console.WriteLine(total);
    }

    public void EJ2()
    {
        Dictionary<string, char> positions = new();

        List<List<string>> inputs = input
            .Select(x => x.Split("|")[0]
                .Trim()
                .Split(" ")
                .Select(y=> string.Concat(y.OrderBy(ch => ch)))
                .ToList()
            )
            .ToList();

        List<List<string>> outputs = input
            .Select(x => x.Split("|")[1]
                .Trim()
                .Split(" ")
                .Select(y => string.Concat(y.OrderBy(ch => ch)))
                .ToList()
            )
            .ToList();

        ulong total = 0;

        for (int i = 0; i < input.Count; i++)
        {
            var input = inputs[i];

            string n1 = input.Where(x => x.Length == 2).First();
            string n4 = input.Where(x => x.Length == 4).First();
            string n7 = input.Where(x => x.Length == 3).First();
            string n8 = input.Where(x => x.Length == 7).First();

            positions["T"] = n7.First(x => !n1.Contains(x));

            var size5 = input.Where(x => x.Length == 5).ToList(); //2 3 5
            var size6 = input.Where(x => x.Length == 6).ToList(); //0 6 9

            string n6 = size6.First(x =>
            {
                bool res = false;
                foreach (char ch in n1)
                {
                    if (!x.Contains(ch))
                    {
                        positions["TR"] = ch;

                        positions["BR"] = n1.First(y => y != ch);

                        return true;
                    }
                }
                return false;
            });

            string n5 = size5.First(x => !x.Contains(positions["TR"]));
            string n2 = size5.First(x => !x.Contains(positions["BR"]));
            string n3 = size5.First(x => x != n5 && x != n2);

            string n0 = size6.First(x =>
            {
                if (x == n6) return false;

                foreach (var ch in x)
                {
                    if (!n5.Contains(ch) && ch != positions["TR"]) return true;
                }
                return false;
            });

            string n9 = size6.First(x => x != n0 && x != n6);

            List<string> numeros = new()
            {
                n0,
                n1,
                n2,
                n3,
                n4,
                n5,
                n6,
                n7,
                n8,
                n9
            };

            int[] outs = new int[4];

            for (int j = 0; j < 4; j++)
            {
                outs[j] = numeros.IndexOf(numeros.First(x => x == outputs[i][j]));
            }
            total += (ulong)int.Parse(String.Concat(outs));
        }


        Console.WriteLine(total);
    }

}