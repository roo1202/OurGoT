namespace OurGoT.Pages
{
    public class UpRange : UpAttack
    {
        public UpRange(Card A, int P) : base(A, P)
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
            B.Range += P;
            System.Console.WriteLine("{2} has modified the range of {0} on {1}", B.Name, P, A.Name);
            System.Console.WriteLine("Now the Range of {0} is {1}", B.Name, B.Range);
            Play.Context.Save(B.Name + ".Range", B.Range);
        }
    }
}
