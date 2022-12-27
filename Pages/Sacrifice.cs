namespace OurGoT.Pages
{
    public class Sacrifice : Action
    {
        Card A;
        List<Card> Graveyard;
        Card[,] Tab;
        public Sacrifice(Card A, List<Card> Graveyard, Card[,] Tab)
        {
            this.A = A;
            this.Graveyard = Graveyard;
            this.Tab = Tab;
        }
        public override void Run()
        {
            if (Play.Graveyard.Count() == 0)
            {
                System.Console.WriteLine("Graveyard is empty,there is no Cards");
                return;
            }
            Card card = Play.CurrentPlayer.GetCard(Play.Graveyard);
            Player Player = Play.CurrentPlayer.Choose_Player(1);
            card.Life = card.TotalLife;
            Player.Hand.Add(card);
            Play.Context.Save(Player.Name + ".Hand", Player.Hand.Count());
            System.Console.WriteLine("Now you have the card choosed");
            A.Life = A.TotalLife;
            Graveyard.Add(A);
            Tab[A.Posx, A.Posy] = new Card();
            Player.CampCards.Remove(A);
            Play.Context.Save(Player.Name + ".Camp", Player.CampCards.Count());
        }
    }
}
