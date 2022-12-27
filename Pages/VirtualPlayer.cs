namespace OurGoT.Pages
{
    public class VirtualPlayer : Player
    {
        public VirtualPlayer(string Name) : base(Name)
        {
            this.Name = Name;
            System.Console.WriteLine("Your opponet is " + Name);
            this.Money = 1000;
            this.CardLim = 6;
        }

        public override void Choose_Card()
        {
            int index = 0;
            int Attack = 0;
            for (int i = 0; i < Play.Available.Count(); i++)
            {
                if (Attack <= Play.CurrentPlayer.Hand[Play.Available[i]].Attack)
                {
                    Attack = Play.CurrentPlayer.Hand[Play.Available[i]].Attack;
                    index = Play.Available[i];
                }
            }
            Play.Selection = index;
        }

        public override void Choose_Position(bool flag, int turno)
        {
            if (flag)
            {
                if (turno % 2 == 1)
                {
                    Play.y = Methods.GetRandom(0, 9);
                    Play.x = Methods.GetRandom(8, 9);
                }
                else
                {
                    Play.y = Methods.GetRandom(0, 9);
                    Play.x = Methods.GetRandom(0, 1);
                }
            }
            else
            {
                int al = Play.Aux.Range;
                int r = Methods.GetRandom(0, al);
                al -= r;
                int op1 = Methods.GetRandom(0, 2);
                int op2 = Methods.GetRandom(0, 2);
                if (op1 == 2)
                    op1 = -1;
                if (op2 == 2)
                    op2 = -1;
                r *= op1;
                al *= op2;
                Play.y = Play.Aux.Posy + al;
                Play.x = Play.Aux.Posx + r;
                Play.y = Math.Max(0, Play.y);
                Play.y = Math.Min(9, Play.y);
                Play.x = Math.Max(0, Play.x);
                Play.x = Math.Min(9, Play.x);
            }
        }

        public override Card Choose_Card(Card A, int type)
        {
            Card Return = new Card();
            int Life = 1000000;
            int index = 0;
            List<Card> Nearly = new List<Card>();
            if (type == 1) Nearly = Methods.OnRange(A.Posx, A.Posy, Play.CurrentPlayer.CampCards, A.Range);
            else Nearly = Methods.OnRange(A.Posx, A.Posy, Play.NextPlayer.CampCards, A.Range);
            for (int i = 0; i < Nearly.Count(); i++)
            {
                if (Life > Nearly[i].Life)
                {
                    Life = Nearly[i].Life;
                    index = i;
                }
            }
            if (Nearly.Count() == 0)
            {
                return new Card();
            }
            Return = Nearly[index];
            return Return;
        }
        public override void Choose_Power(Card C, bool[] used)
        {
            List<int> Available = new List<int>();
            for (int x = 1; x < C.Conditions.Count(); x++)
            {
                if (C.Conditions[x].Evaluate() > 0 && used[x] == false)
                {
                    Play.Selection = x;
                    return;
                }
            }
            Play.Selection = -1;
        }

        public override Card GetCard(List<Card> list)
        {
            int ind = 0;
            int Attack = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].Attack > Attack)
                {
                    ind = i;
                    Attack = list[i].Attack;
                }
            }
            return list[ind];
        }

        public override Player Choose_Player(int x)
        {
            if (x == 1) return Play.CurrentPlayer;
            return Play.NextPlayer;
        }
    }
}
