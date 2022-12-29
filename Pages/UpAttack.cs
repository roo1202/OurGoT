namespace OurGoT.Pages
{
    public class UpAttack : Action
    {
        protected Card A;
        protected int P;
        protected Card B = new Card();
        public UpAttack(Card A, int P)
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
            Play.MessegeAction = $"{A.Name}  modified the Attack of {B.Name} on {P}";
            //await Task.Delay(1000);
            if (B.Name == "*")
            {
                System.Console.WriteLine("Your spell was misused");
                return;
            }
            B.Attack += P;
            System.Console.WriteLine("{2} has modified the attack of {0} on {1}", B.Name, P, A.Name);
            System.Console.WriteLine("Now the attack of {0} is {1}", B.Name, B.Attack);
            Play.Context.Save(B.Name + ".Attack", B.Attack);
            Play.SelectedCardAction = new Card();
        }
    }
}
