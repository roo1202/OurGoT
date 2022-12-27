namespace OurGoT.Pages
{
    public class Heal : Action
    {
        Card A;
        Card B = new Card();
        int P;
        public Heal(Card A, int P)
        {
            this.A = A;
            this.P = P;
        }
        public override void Run()
        {
            B = Play.CurrentPlayer.Choose_Card(A, 1);
            if (B.Name == "*")
            {
                System.Console.WriteLine("Your spell was misused");
                return;
            }
            B.Life += P;
            System.Console.WriteLine("{0} has been healed on {1}", B.Name, P);
            if (B.Life > B.TotalLife)
            {
                B.Life = B.TotalLife;
                System.Console.WriteLine("You cannot heal anymore this unit");
            }
            if (B.Life == 0)
            {
                System.Console.WriteLine("{0} has died", B.Name);
                Console.WriteLine("You earn {0}", B.Cost / 2);
                Play.CurrentPlayer.Money += B.Cost / 2;
                Play.NextPlayer.CampCards.Remove(B);
                Play.CurrentPlayer.CampCards.Remove(B);
                Play.Graveyard.Add(B);
                Play.Tab[B.Posx, B.Posy] = new Card();
                Play.Context.Save(Play.NextPlayer.Name + ".CampCards", Play.NextPlayer.CampCards.Count());
            }
            System.Console.WriteLine("Life: " + B.Life);
            Play.Context.Save(B.Name + ".Life", B.Life);
        }
    }
}
