namespace OurGoT.Pages
{
    public class Trade : Action
    {
        Player Player = Play.CurrentPlayer;
        int P;
        public Trade(int P)
        {
            this.P = P;
        }
        public override void Run()
        {
            if (P < 0) Player = Play.CurrentPlayer.Choose_Player(2);
            else Player = Play.CurrentPlayer.Choose_Player(1);
            Player.Money += P;
            System.Console.WriteLine("The Money of {0} has been modified on {1}", Player.Name, P);
            System.Console.WriteLine("Now is {0}", Player.Money);
            Play.Context.Save(Player.Name + ".Money", Player.Money);
        }
    }
}
