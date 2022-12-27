namespace OurGoT.Pages
{
    public class Methods
    {

        public static void Move(Card[,] Tab, Card A, int newX, int newY)
        {
            Tab[newX, newY] = A;
            Tab[A.Posx, A.Posy] = new Card();
            A.Posx = newX;
            A.Posy = newY;
            Play.Context.Save(A.Name + ".Posx", A.Posx);
            Play.Context.Save(A.Name + ".Posy", A.Posy);
        }

        public static bool Validate_Position(int x, int y, Card[,] Tab, int t, bool flag)
        {
            if (x < 0 || x > 9 || y < 0 || y > 9 || Tab[x, y].Name != "*") return false;
            if (flag && ((t % 2 == 0 && x > 1) || (t % 2 == 1 && x < 8))) return false;
            return true;
        }

        public static bool Continue(Player A)
        {
            if (A.Money > 0)
                return true;
            if (A != Play.CurrentPlayer)
                Play.Winner = Play.CurrentPlayer.Name;
            else
                Play.Winner = Play.NextPlayer.Name;
            return false;
        }

        public static List<Card> OnRange(int x, int y, List<Card> list, int ratio)
        {
            List<Card> Cards = new List<Card>();
            for (int i = 0; i < list.Count(); i++)
            {
                int dist = Distance(list[i].Posx, list[i].Posy, x, y);
                if (dist <= ratio)
                    Cards.Add(list[i]);
            }
            return Cards;
        }
        /*
        public static void AccionDeRango(int x,int y,List<Card>list,int ratio, Action accion)
        {
            List<Card> enZona = new List<Card>();

            enZona = OnRange(x,y,list,ratio);
            {
                foreach(var A in enZona)
                {
                    accion.Run();
                }
            }
        }*/


        public static void DoTimeActions()
        {
            foreach (var action in Play.TimeActions)
            {
                action.Key.Evaluate();
                if (action.Value <= 1)
                    Play.TimeActions.Remove(action.Key);
                else
                    Play.TimeActions[action.Key]--;
            }
        }

        public static int Distance(int x, int y, int newx, int newy)
        {
            return Math.Abs(newx - x) + Math.Abs(newy - y);
        }

        public static Card Closet(int x, int y, List<Card> list)
        {
            List<Card> Exist = new List<Card>();
            for (int i = 0; i <= 18; i++)
            {
                Exist = OnRange(x, y, list, i);
                if (Exist.Count() != 0) return Exist[0];
            }
            return new Card();
        }

        public static int GetRandom(int star, int end)
        {
            Random num = new Random();
            int n = num.Next(star, end);
            return n;
        }
        public static void ReadTab(Card[,] Tab)
        {
            System.Console.Write("   ");
            for (int z = 0; z < Tab.GetLength(0); z++)
            {
                System.Console.Write(z + "  ");
            }
            System.Console.WriteLine();
            System.Console.Write("   ");
            for (int z = 0; z < Tab.GetLength(0); z++)
            {
                System.Console.Write("_" + "  ");
            }

            System.Console.WriteLine();
            for (int i = 0; i < Tab.GetLength(0); i++)
            {
                System.Console.Write(i + " ");
                for (int j = 0; j < Tab.GetLength(1); j++)
                {
                    if (Tab[i, j].Name != "*")
                    {
                        string name = Tab[i, j].Name;
                        bool flag = false;
                        foreach (var x in Play.Player1.CampCards)
                            if (name == x.Name)
                                flag = true;
                        char Beggining = name[0];
                        System.Console.Write("|");
                        if (flag)
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                        else
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                        System.Console.Write(Beggining);
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Console.Write("|");
                    }
                    else System.Console.Write("|_|");
                }
                System.Console.WriteLine();
            }
        }
        public static void EndGame()
        {
            System.Console.WriteLine("Game is over");
            int p1 = Play.CurrentPlayer.Money, p2 = Play.NextPlayer.Money;
            foreach (var x in Play.CurrentPlayer.CampCards)
                p1 += x.Cost;
            foreach (var x in Play.NextPlayer.CampCards)
                p2 += x.Cost;

            if (p1 > p2)
            { System.Console.WriteLine("Won " + Play.CurrentPlayer.Name); Play.Winner = Play.CurrentPlayer.Name; }
            else if (p1 < p2)
            { System.Console.WriteLine("Won " + Play.NextPlayer.Name); Play.Winner = Play.NextPlayer.Name; }
            else
            { System.Console.WriteLine("tie"); Play.Winner = "Tie" ; }
        }

    }
}
