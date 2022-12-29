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
        public override async Task Run()
        {
            if (Play.CurrentPlayer is VirtualPlayer)
                B = Play.CurrentPlayer.Choose_Card(A, 1);
            else
            {

                while ((Methods.Distance(A.Posx, A.Posy, B.Posx, B.Posy) > A.Range))
                {
                    await Play.Waitting();
                    B = Play.SelectedCardAction;
                }


            }
            Play.MessegeAction = $"{A.Name}  Heal to {B.Name} on {P}";
            //await Task.Delay(1000);
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
                Play.MessegeAction = $"You cannot heal anymore this unit";
                //await Task.Delay(1000);
            }
            if (B.Life <= 0)
            {
                System.Console.WriteLine("{0} has died", B.Name);
                Console.WriteLine("You earn {0}", B.Cost / 2);
                Play.CurrentPlayer.Money += B.Cost / 2;
                Play.NextPlayer.CampCards.Remove(B);
                Play.CurrentPlayer.CampCards.Remove(B);
                Play.Graveyard.Add(B);
                Play.Tab[B.Posx, B.Posy] = new Card();
                Play.Tab[B.Posx, B.Posy].Posx = B.Posx;
                Play.Tab[B.Posx, B.Posy].Posy = B.Posy;
                Play.Context.Save(Play.NextPlayer.Name + ".CampCards", Play.NextPlayer.CampCards.Count());
            }
            System.Console.WriteLine("Life: " + B.Life);
            Play.Context.Save(B.Name + ".Life", B.Life);
            Play.SelectedCardAction = new Card();
        }
    }
}
