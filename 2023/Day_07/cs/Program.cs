namespace cs;

class Program
{
    public enum Card : uint {
        Joker = 0,
        _2,
        _3,
        _4,
        _5,
        _6,
        _7,
        _8,
        _9,
        T,
        J,
        Q,
        K,
        A,
    };

    enum HandType: uint {
        FiveKind = 6,
        FourKind = 5,
        FullHouse = 4,
        ThreeKind = 3,
        TwoPair = 2,
        OnePair = 1,
        HighCard = 0,
    };

    static Card ParseCard(char card, bool joker = false) => (card, joker) switch {
        ('A',_) => Card.A,
        ('K',_) => Card.K,
        ('Q',_) => Card.Q,
        ('J',false) => Card.J,
        ('J',true) => Card.Joker,
        ('T',_) => Card.T,
        ('9',_) => Card._9,
        ('8',_) => Card._8,
        ('7',_) => Card._7,
        ('6',_) => Card._6,
        ('5',_) => Card._5,
        ('4',_) => Card._4,
        ('3',_) => Card._3,
        ('2',_) => Card._2,
        _ => throw new ArgumentException($"Caracter no valido: {card}"),
    };

    record Hand(Card[] Cards, Int64 Bid) : IComparable<Hand>
    {
        public int CompareTo(Hand? other)
        {
            for(int i = 0; i < Cards.Length; ++i) {
                var compare = Cards[i].CompareTo(other!.Cards[i]);
                if(compare != 0) return compare;
            }
            return 0;
        }

        public string CardsToString() {
            return string.Join(",", Cards);
        }
    }

    static HandType GetHandType(Hand hand) {
        var groups = 
            hand.Cards
            .GroupBy(x=>x)
            .OrderByDescending(x=> x.Count())
            .ToArray();
        
        var major = groups[0];
        switch (major.Count()) {
            case 5:
            case 4 when groups.Any(x=> x.Key == Card.Joker):
                return HandType.FiveKind;
            case 4:
                return HandType.FourKind;
            case 3: {
                // AAA BB
                if(groups.Length == 2) {
                    if(major.Key == Card.Joker) return HandType.FiveKind;
                    if(groups[1].Key == Card.Joker) return HandType.FiveKind;
                    return HandType.FullHouse;
                }
                //AAA B C
                if(groups.Any(x => x.Key == Card.Joker)) return HandType.FourKind;
                return HandType.ThreeKind;
            }
            case 2: {
                // AA BB C
                if(groups[1].Count() == 2) {
                    if(major.Key == Card.Joker) return HandType.FourKind;
                    if(groups[1].Key == Card.Joker) return HandType.FourKind;
                    if(groups[2].Key == Card.Joker) return HandType.FullHouse;
                    return HandType.TwoPair;
                }
                // AA B C D
                if(major.Key == Card.Joker) return HandType.ThreeKind;
                if(groups.Skip(1).Any(x=> x.Key == Card.Joker)) return HandType.ThreeKind;
                return HandType.OnePair;
            }
            default: { //Case 1
                if(groups.Any(x=> x.Key == Card.Joker)) return HandType.OnePair;
                return HandType.HighCard;
            }
            
        }
    }

    public static long Sol1(string path)
    {
        var input = ParseInput(path);

        var grouped = 
            input
            .GroupBy(x=> GetHandType(x))
            .OrderByDescending(x=> x.Key)
            .ToArray();

        var ordered =
            grouped
            .SelectMany(x=> x.OrderDescending())
            .ToArray();
        return 
            Enumerable.Range(1, input.Length)
            .Reverse()
            .Zip(ordered)
            .Select(x=> x.First * x.Second.Bid)
            .Sum();
    }

    public static long Sol2(string path)
    {
        var input = ParseInput(path, true);

        var grouped = 
            input
            .GroupBy(x=> GetHandType(x))
            .OrderByDescending(x=> x.Key)
            .ToArray();

        var ordered =
            grouped
            .SelectMany(x=> x.OrderDescending())
            .ToArray();
        
        foreach (var hand in ordered)
        {
            Console.WriteLine(hand.CardsToString());
        }

        return 
            Enumerable.Range(1, input.Length)
            .Reverse()
            .Zip(ordered)
            .Select(x=> x.First * x.Second.Bid)
            .Sum();
    }

    private static Hand[] ParseInput(string path, bool joker = false)
    {
        var inputLines = File.ReadAllLines(path);
        return 
            inputLines
            .Select( l => {
                var aux = l.Split(" ");
                return new Hand(
                    aux[0].Select(x=> ParseCard(x, joker)).ToArray(), 
                    long.Parse(aux[1]));
            }).ToArray();
    }

    static void Main(string[] args)
    {
        var path = args.FirstOrDefault("../AOCinput");

        // Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }
}

