namespace OurGoT.Pages
{
    public class Player
    {
        public string Name { get; set; }
        public int Money { get; set; }
        public int CardLim { get; set; }
        public List<Card> Hand = new List<Card>();
        public List<Card> CampCards = new List<Card>();
        public Player(string Name)
        {
            this.Name = Name;
            this.Money = 500;
            this.CardLim = 6;
        }

        public void Draw(List<Card> DrawDeck, int PosCard)
        {
            Hand.Add(DrawDeck[PosCard]);
            DrawDeck.RemoveAt(PosCard);
            Play.Context.Save(Play.CurrentPlayer.Name + ".Hand", Play.CurrentPlayer.Hand.Count());
            Play.Context.Save(Play.CurrentPlayer.Name + ".Hand", Play.CurrentPlayer.Hand.Count());
        }

        public List<int> AvailableCards()
        {
            List<int> Available = new List<int>();
            for (int i = 0; i < Hand.Count(); i++)
            {
                if (Hand[i].Conditions[0].Evaluate() == 0)
                    continue;
                Available.Add(i);
            }
            return Available;
        }

        public static Player Choose_Player()
        {
            System.Console.WriteLine("Choose 1 to human, 2 to virtual");
            int n = int.Parse(Console.ReadLine()!);
            if (n == 1)
            {
                System.Console.WriteLine("What is your Name");
                string name = Console.ReadLine()!;
                return new Player(name);
            }
            else
            {
                string[] Playeres = { "Robert_Barathyon", "Daenerys", "Cersei", "Joffrey", "Thom", "Jon_Snow" };
                return new VirtualPlayer(Playeres[Methods.GetRandom(0, Playeres.Length - 1)]);
            }
        }

        public virtual void Choose_Card()
        {
            System.Console.WriteLine("Choose a card else -1 to skip");
            int Selection;
            Selection = int.Parse(Console.ReadLine()!);
            Play.Selection = Selection;
        }

        public virtual void Choose_Position(bool flag, int turn)
        {
            if (flag)
            {
                if (turn % 2 == 0) System.Console.WriteLine("Choose a position between the rows 0 and 1");
                else System.Console.WriteLine("Choose a position between the rows 8 and 9");
            }
            else System.Console.WriteLine("Choose a position else -1 to skip");
            Play.x = int.Parse(Console.ReadLine()!);
            if (Play.x == -1) return;
            Play.y = int.Parse(Console.ReadLine()!);
        }

        public virtual void Choose_Power(Card C, bool[] used)
        {
            System.Console.WriteLine("Choose one of the next Available Powers or -1 to skip");
            for (int x = 1; x < C.Conditions.Count(); x++)
            {
                if (C.Conditions[x].Evaluate() > 0 && used[x] == false)
                    System.Console.WriteLine(x);
            }
            Play.Selection = int.Parse(Console.ReadLine()!);
        }

        public static bool Exist(int posx, int posy)
        {
            bool flag = false;
            foreach (var v in Play.CurrentPlayer.CampCards)
                if (v.Posx == posx && posy == v.Posy)
                    flag = true;
            foreach (var v in Play.NextPlayer.CampCards)
                if (v.Posx == posx && posy == v.Posy)
                    flag = true;
            return flag;
        }
        public virtual Card Choose_Card(Card A, int type)
        {
            int PX, PY;
            Card Return = new Card();
            do
            {
                System.Console.WriteLine("Choose a Card");
                PX = int.Parse(Console.ReadLine()!);
                PY = int.Parse(Console.ReadLine()!);
            } while (Methods.Distance(PX, PY, A.Posx, A.Posy) > A.Range || !Exist(PX, PY));
            Return = Play.Tab[PX, PY];
            return Return;
        }

        public virtual Card GetCard(List<Card> list)
        {
            int ind = 0;
            System.Console.WriteLine("Cards Available");
            for (int i = 0; i < list.Count(); i++)
                System.Console.WriteLine(i + "-" + list[i].Name);
            do
            {
                System.Console.WriteLine("Pick the index of the choosen Card");
                ind = int.Parse(Console.ReadLine()!);
            } while (ind < 0 || ind >= list.Count());
            return list[ind];
        }
        public virtual Player Choose_Player(int x)
        {
            int S = 0;
            do
            {
                System.Console.WriteLine("Choose the Player, 1-To current Player 2-To next Player");
                S = int.Parse(Console.ReadLine()!);
            } while (S <= 0 || S > 2);
            if (S == 1) return Play.CurrentPlayer;
            return Play.NextPlayer;
        }

    }
}  