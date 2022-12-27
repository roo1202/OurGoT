namespace OurGoT.Pages
{
    public class Attack : Action
    {
        Card A;
        Card B = new Card();

        public Attack(Card A)
        {
            this.A = A;
        }
        public override void Run()
        {
            if (Methods.OnRange(A.Posx, A.Posy, Play.NextPlayer.CampCards, A.Range).Count() != 0)
            {
                B = Play.CurrentPlayer.Choose_Card(A, 2);
                if (B.Name == "*")
                {
                    System.Console.WriteLine("Your spell was misused");
                    return;
                }
                System.Console.WriteLine("{0} attack to {1}", A.Name, B.Name);
                if (A.Attack > B.Defense)
                {
                    System.Console.Write("Life of {0} down from {1} to ", B.Name, B.Life);
                    B.Life -= (A.Attack - B.Defense);
                    System.Console.WriteLine(B.Life);
                }
                else
                {
                    System.Console.WriteLine("Defense of {0} is bigger than attack of {1}", B.Name, A.Name);
                }
                B.Life = Math.Max(B.Life, 0);
                if (B.Life == 0)
                {
                    System.Console.WriteLine("{0} has died", B.Name);
                    Console.WriteLine("You earn {0}", B.Cost / 2);
                    Play.CurrentPlayer.Money += B.Cost / 2;
                    Play.NextPlayer.CampCards.Remove(B);
                    Play.Graveyard.Add(B);
                    Play.Tab[B.Posx, B.Posy] = new Card();
                    Play.Context.Save(Play.NextPlayer.Name + ".CampCards", Play.NextPlayer.CampCards.Count());
                }
                Play.Context.Save(B.Name + ".Life", B.Life);
            }
            else System.Console.WriteLine("You cannot Attack anymore, because there is not enemies around");
            return;
        }
    }
}
