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
        public override async Task Run()
        {
            if (Methods.OnRange(A.Posx, A.Posy, Play.NextPlayer.CampCards, A.Range).Count() != 0)
            {

                if (Play.CurrentPlayer is VirtualPlayer)
                    B = Play.CurrentPlayer.Choose_Card(A, 2);
                else
                {
                    while ((Methods.Distance(A.Posx, A.Posy, B.Posx, B.Posy) > A.Range))
                    {
                        await Play.Waitting();
                        B = Play.SelectedCardAction;
                    }
                    
                }
                Play.MessegeAction = $"{A.Name}  Attack to {B.Name}";
                //await Task.Delay(1000);
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
                    Play.MessegeAction = $"{B.Name} Defense's is bigger than attack of {A.Name}";
                    //await Task.Delay(1000);
                }
                B.Life = Math.Max(B.Life, 0);
                if (B.Life <= 0)
                {
                    System.Console.WriteLine("{0} has died", B.Name);
                    Console.WriteLine("You earn {0}", B.Cost / 2);
                    Play.CurrentPlayer.Money += B.Cost / 2;
                    Play.NextPlayer.CampCards.Remove(B);
                    Play.Graveyard.Add(B);
                    Play.Tab[B.Posx, B.Posy] = new Card();
                    Play.Tab[B.Posx, B.Posy].Posx = B.Posx;
                    Play.Tab[B.Posx, B.Posy].Posy = B.Posy;
                    Play.Context.Save(Play.NextPlayer.Name + ".CampCards", Play.NextPlayer.CampCards.Count());
                    Play.MessegeAction = $"{B.Name} has died";
                    //await Task.Delay(1000);
                }

                Play.Context.Save(B.Name + ".Life", B.Life);
            }
            else
            {
                Play.MessegeAction = $"You cannot Attack anymore";
                //await Task.Delay(1000);
                System.Console.WriteLine("You cannot Attack anymore, because there is not enemies around");
            }
            Play.SelectedCardAction = new Card();
                return;
        }
    }
}
