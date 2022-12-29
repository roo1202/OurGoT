namespace OurGoT.Pages
{
    public class UpRange : UpAttack
    {
        public UpRange(Card A, int P) : base(A, P)
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
            Play.MessegeAction = $"{A.Name}  modified the Range of {B.Name} on {P}";
            //await Task.Delay(1000);
            if (B.Name == "*")
            {
                System.Console.WriteLine("Your spell was misused");
                return;
            }
            B.Range += P;
            System.Console.WriteLine("{2} has modified the range of {0} on {1}", B.Name, P, A.Name);
            System.Console.WriteLine("Now the Range of {0} is {1}", B.Name, B.Range);
            Play.Context.Save(B.Name + ".Range", B.Range);
            Play.SelectedCardAction = new Card();
        }
    }
}
