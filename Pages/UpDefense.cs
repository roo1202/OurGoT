namespace OurGoT.Pages
{
    public class UpDefense : UpAttack
    {
        public UpDefense(Card A, int P) : base(A, P)
        {
        }
        public override void Run()
        {
            B = Play.CurrentPlayer.Choose_Card(A, 1);
            if (B.Name == "*")
            {
                System.Console.WriteLine("Your spell was misused");
                return;
            }
            B.Defense += P;
            System.Console.WriteLine("{2} has modified the Defense of {0} on {1}", B.Name, P, A.Name);
            System.Console.WriteLine("Now the Defense of {0} is {1}", B.Name, B.Defense);
            Play.Context.Save(B.Name + ".Defense", B.Defense);
        }
    }
}
