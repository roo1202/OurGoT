namespace OurGoT.Pages
{
    public class Deck
    {
        public static Card CreateCard()
        {
            System.Console.WriteLine("Enter the name of the Card");
            string Title;
            Title = Console.ReadLine()!;
            Lexer lexer = new Lexer();
            lexer.Read(Title);
            lexer.Check();
            if (lexer.Errors.Count() == 0)
            {
                Parser2 parser2 = new Parser2(Title, lexer.Tokens);
                Save(Title, lexer.Text);
                return parser2.card;
            }
            else
            {
                foreach (var x in lexer.Errors)
                    System.Console.WriteLine(x);
                System.Console.WriteLine("We sorry there is error on the creation of the card");
                return new Card();
            }
        }
        public static List<Card> ReadDeck()
        {
            List<Card> list = new List<Card>();
            string[] paths = Directory.GetFiles("./Content", "*.*", SearchOption.AllDirectories);
            foreach (var x in paths)
            {
                string body = File.ReadAllText(x, System.Text.Encoding.UTF8);
                string Title = x.Substring(10);

                Parser2 card = new Parser2(Title, Lexer.ToToken(Lexer.ToList(body), Title));
                card.Parsing();
                list.Add(card.card);
            }
            return list;
        }

        public static void Save(string Title, string Text)
        {
            File.WriteAllText("./Content/" + Title, Text);
        }

        public static List<Card> Shuffle(List<Card> mazo)
        {
            List<Card> aux = new List<Card>();
            Random r = new Random(Environment.TickCount);
            while (mazo.Count() > 0)
            {
                int pos = r.Next(0, mazo.Count() - 1);
                aux.Add(mazo[pos]);
                mazo.RemoveAt(pos);
            }
            return aux;
        }
    }
}
