namespace OurGoT.Pages
{
    public class UpDefense : UpAttack
    {
        public UpDefense(Card A, int P) : base(A, P)
        {
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
            Play.MessegeAction = $"{A.Name}  modified the Defense of {B.Name} on {P}";
            //await Task.Delay(1000);
            if (B.Name == "*")
            {
                System.Console.WriteLine("Your spell was misused");
                return;
            }
            B.Defense += P;
            System.Console.WriteLine("{2} has modified the Defense of {0} on {1}", B.Name, P, A.Name);
            System.Console.WriteLine("Now the Defense of {0} is {1}", B.Name, B.Defense);
            Play.Context.Save(B.Name + ".Defense", B.Defense);
            Play.SelectedCardAction = new Card();
        }
    }
}
