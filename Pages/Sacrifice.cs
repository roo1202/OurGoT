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
        public override async Task Run()
        {
            if (Play.Graveyard.Count() == 0)
            {
                System.Console.WriteLine("Graveyard is empty,there is no Cards");
                return;
            }
            Card card = new Card();
            if (Play.CurrentPlayer is VirtualPlayer)
            {
                card= Play.CurrentPlayer.GetCard(Play.Graveyard);
      
            }
            else
            {
                Play.OnSacrifice = true;
                await Play.Waitting();
                foreach(var x in Graveyard)
                    if(x.Name == Play.ReturnalCard)
                        card = x;
                Play.OnSacrifice = false;
                Play.ReturnalCard = "*";
            }
            
            card.Life = card.TotalLife;
            Play.CurrentPlayer.Hand.Add(card);
            Play.Context.Save(Play.CurrentPlayer.Name + ".Hand", Play.CurrentPlayer.Hand.Count());
            System.Console.WriteLine("Now you have the card choosed");
            Play.Graveyard.Add(A);
            Play.Tab[A.Posx, A.Posy] = new Card();
            Play.CurrentPlayer.CampCards.Remove(A);
            Play.Context.Save(Play.CurrentPlayer.Name + ".CampCards", Play.CurrentPlayer.CampCards.Count());

            
            Play.Tab[A.Posx, A.Posy].Posx = A.Posx;
            Play.Tab[A.Posx, A.Posy].Posy = A.Posy;
            Play.MessegeAction = $"{A.Name} has died now you have {card.Name}";
        }
    }
}
/*@using OurGoT.Pages
@using System.Timers
@implements IDisposable

@page "/Play"

<h3>Player: @CurrentPlayer.Name</h3>
<h3>Money: @CurrentPlayer.Money</h3>
<p>@Message</p>

 <h3 class="Right-Up">Turn: @turn</h3>
<p>Your field invocation is on the row 1 or 2</p>


@if(Winner != "")
{
    <text-center class = "Head Up">Winner is @Winner</text-center>
}

<div class="Hand" style="height=(@height)px">
    @foreach(var _card in CurrentPlayer.Hand)
    {
        CardModel cardmodel = new CardModel {card = _card, Opacity = _card.Conditions[0].Evaluate() > 0 };
        height = CurrentPlayer.Hand.Count() * 100;
        <Card CardModel = "cardmodel" Resize = "true" OnMouseOver="(()=>MouseOver(_card))" OnMouseClick="(()=>SelectCardHand(_card))"/>
    }
</div>

<div class="Tab Tab_Picture">
    @foreach(var x in gridModels)
    {
        string aux = "";
        
        x.Card = Tab[x.Idx, x.Idy];

        foreach(var y in NextPlayer.CampCards)
        {
            if(y == x.Card)
            {
                aux = "Enemy";
            }
        }
        foreach(var y in CurrentPlayer.CampCards)
        {
            if(y == x.Card)
            {
                aux = "Ally";
            }
        }
        x.Site = aux;
        @if(Tab[x.Idx,x.Idy].Name == "*")
        {
            x.IsInvisible = true;
        }
        else
        {
            x.IsInvisible = false;
        }
      
         <Grid GridModel="x"  OnMouseOver="(()=>MouseOver(x.Card))" OnMouseDClick="(()=>Invoke(x.Card))" OnMouseClick="(()=>SelectCardCamp(x.Card))" OnMouseUp="(()=>Move(x.Card))"/>   
      
     }
</div>

 <div class="Showing">
 <Card CardModel = "@Show" Resize = "false" STYLE = "Bigger" />
 @if(SelectedCardCamp.Name != "" && itPower == 0)
{
        <div class="Powers">
            <h2>Powers</h2>
            <select style="height=50 width=50" @bind = "Option">
            @for(int i=1;i<SelectedCardCamp.Powers.Count();i++)
            {
                     @if(SelectedCardCamp.Conditions[i].Evaluate() > 0 && SelectedCardCamp.Used[i] == false)
                {
                    <option value = "@i"> @i </option>
                }
            }
            </select> 
        </div>
}
 </div>

 <button @onclick="Change" type="button" class="UP btn btn-primary">End Phase</button>


@code
{
    public static int turn = -1;
    public static Player CurrentPlayer = new Player("");
    public static Player Player1 = new Player("");
    public static Player Player2 = new Player("");
    public static Player NextPlayer = new Player("");
    public static Card[,] Tab = new Card[10, 10];
    public static List<Card> Graveyard = new List<Card>();
    public static Dictionary<Expression, int> TimeActions = new Dictionary<Expression, int>();
    public static Context Context = new Context();
    public static int x;
    public static int y;
    public static int Selection;
    public static List<int> Available = new List<int>();
    public static Card Aux = new Card();
    public List<Card> deck = new List<Card>();
    public static string Winner = "";
    public static List<GridModel> gridModels { get; set; } = new List<GridModel>();
    public static List<CardModel> cardModels { get; set; } = new List<CardModel>();
    public static int height;
    public static CardModel Show = new CardModel{id = 69,Opacity = true,card = new Card()};
    public static Card SelectedCardHand = new Card();
    public static int InvokeCant = 0;
    public static Card SelectedCardCamp = new Card();
    public static bool OnPhase = false;

    public static Timer timer;
    public static int Option = -1;
    public static int itPower = 0;
    public static string Message = "";

    public void Dispose()
    {
        if (timer != null)
            timer.Dispose();
    }

    private void MouseOver(Card card)
    {
        Show = new CardModel {id=100, Opacity = true, card = card };

    }
    private void SelectCardHand(Card card)
    {       
        if(InvokeCant == 0 && card.Conditions[0].Evaluate() > 0)
            SelectedCardHand = card;
    }
    private void SelectCardCamp(Card card)
    {
        if(card.Name == "*")
            return;
        foreach (var x in NextPlayer.CampCards)
            if (x == card)
                return;
        System.Console.WriteLine(card.Name);

        SelectedCardCamp = card;
        Option = -1;
        itPower = 0;
    }
    private void Invoke(Card position)
    {
        if(position.Name == "*" && SelectedCardHand.Name != "*")
        {
            if(Methods.Validate_Position(position.Posx, position.Posy, Tab, turn, true))
            {
                CurrentPlayer.Hand.Remove(SelectedCardHand);

                Tab[position.Posx,position.Posy] = SelectedCardHand;
                Tab[position.Posx, position.Posy].Posx = position.Posx;
                Tab[position.Posx, position.Posy].Posy = position.Posy;
                InvokeCant = 1;
                CurrentPlayer.CampCards.Add(SelectedCardHand);

                CurrentPlayer.Money -= SelectedCardHand.Cost;
                Context.Save(CurrentPlayer.Name + ".Money", CurrentPlayer.Money);
                Context.Save(CurrentPlayer.Name + ".Hand", CurrentPlayer.Hand.Count());
                Context.Save(CurrentPlayer.Name + ".CampCards", CurrentPlayer.CampCards.Count());
                Context.Save(SelectedCardHand.Name + ".Posx", SelectedCardHand.Posx);
                Context.Save(SelectedCardHand.Name + ".Posy", SelectedCardHand.Posy);

                StateHasChanged();
            }
        }
        SelectedCardHand = new Card();
    }
    public void Move(Card NewPosition)
    {

        if (Methods.Distance(NewPosition.Posx,NewPosition.Posy,SelectedCardCamp.Posx,SelectedCardCamp.Posy) > SelectedCardCamp.Range)
            return;
        if (NewPosition.Name != "*")
            return;
        if (SelectedCardCamp.Moved)
            return;

        int newx = NewPosition.Posx;
        int newy = NewPosition.Posy;
        int posx = SelectedCardCamp.Posx;
        int posy = SelectedCardCamp.Posy;

        Context.Save(SelectedCardCamp.Name + ".Posx", newx);
        Context.Save(SelectedCardCamp.Name + ".Posy", newy);

        Tab[newx, newy] = SelectedCardCamp;
        Tab[posx, posy] = new Card();

        Tab[posx,posy].Posx = posx;
        Tab[posx,posy].Posy = posy;

        Tab[newx, newy].Posx = newx;
        Tab[newx, newy].Posy = newy;
        Tab[newx, newy].Moved = true;
        StateHasChanged();
    }

    private void Change()
    {
        OnPhase = false;
        StateHasChanged();
    }

    private async Task FirstPhase()
    {
        OnPhase = true;
        while(OnPhase)
        {
            await Task.Delay(1);
        }
    }
    private async Task ActionTime()
    {
        StateHasChanged();
        OnPhase = true;
        foreach (var x in CurrentPlayer.CampCards)
            for (int i = 0; i < x.Used.Count(); i++)
                x.Used[i] = false;
        while(OnPhase)
        {
            Console.WriteLine(Option);
            if (Option != -1)
            {
                SelectedCardCamp.Powers[Option].Evaluate();
                SelectedCardCamp.Used[Option] = true;
                StateHasChanged();
            }
            await Task.Delay(1);
        }
    }

    private async Task GameLoop()
    {
        Context.Save(Player1.Name + ".Money", Player1.Money);
        Context.Save(Player2.Name + ".Money", Player2.Money);
        Context.Save(Player1.Name + ".Hand", Player1.Hand.Count());
        Context.Save(Player2.Name + ".Hand", Player2.Hand.Count());
        Context.Save(Player1.Name + ".CampCards", Player1.CampCards.Count());
        Context.Save(Player2.Name + ".CampCards", Player2.CampCards.Count());
        Console.WriteLine("CACACACACACCCACACCACRFRFDFAFEAFADFFDAF");
        // deck.Create_Card();
        // return;


        deck = Deck.ReadDeck();

        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
            {
                Tab[i, j] = new Card();
                Tab[i, j].Posx = i;
                Tab[i, j].Posy = j;
                gridModels.Add(new GridModel { Card = Tab[i,j] ,Idx = j, Idy = i});
            }

        for (int i = 0; i < 3; i++)
        {
            Player1.Draw(deck, Methods.GetRandom(0, deck.Count() - 1));
            Player2.Draw(deck, Methods.GetRandom(0, deck.Count() - 1));
        }


        while (Methods.Continue(Player1) && Methods.Continue(Player2))
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    Tab[i, j].Moved = false;
                }

            gridModels = Enumerable.Reverse(gridModels).ToList();

            InvokeCant = 0;

            Methods.ReadTab(Tab);
            if (turn != -1)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Change to Player : " + NextPlayer.Name);
            }
            else System.Console.WriteLine("Start game, First " + Player1.Name);
            turn++;
            if (turn % 2 == 1)
            {
                CurrentPlayer = Player2;
                NextPlayer = Player1;
            }
            else
            {
                CurrentPlayer = Player1;
                NextPlayer = Player2;
            }

            Context.Save(CurrentPlayer.Name + ".Money", CurrentPlayer.Money);
            Context.Save(NextPlayer.Name + ".Money", NextPlayer.Money);
            Context.Save(CurrentPlayer.Name + ".Hand", CurrentPlayer.Hand.Count());
            Context.Save(NextPlayer.Name + ".Hand", NextPlayer.Hand.Count());
            Context.Save(CurrentPlayer.Name + ".CampCards", CurrentPlayer.CampCards.Count());
            Context.Save(NextPlayer.Name + ".CampCards", NextPlayer.CampCards.Count());

            if (deck.Count() == 0)
            {
                Methods.EndGame();
                break;
            }
            System.Console.WriteLine("Your Money is  : " + CurrentPlayer.Money);
            CurrentPlayer.Draw(deck, Methods.GetRandom(0, deck.Count() - 1));

            if (TimeActions.Count() > 0)
            {
                System.Console.WriteLine("Doing time actions");
                Methods.DoTimeActions();
            }

            while (CurrentPlayer.CardLim < CurrentPlayer.Hand.Count())
            {
                Graveyard.Add(CurrentPlayer.Hand[0]);
                CurrentPlayer.Hand.RemoveAt(0);
            }
            Available = CurrentPlayer.AvailableCards();
            if (Available.Count() != 0)
            {
                foreach (var c in Available)
                {
                    System.Console.WriteLine("Card : " + c);
                    CurrentPlayer.Hand[c].ReadCard();
                }
                x = y = -1;

                if (CurrentPlayer is VirtualPlayer)
                {
                    CurrentPlayer.Choose_Card();
                    await Task.Delay(1000);
                }
                else
                {
                    Message = "Waitting for invocation";
                    await FirstPhase();
                }

                Methods.ReadTab(Tab);

                if (CurrentPlayer is VirtualPlayer && Selection != -1)
                {

                    while (!Methods.Validate_Position(x, y, Tab, turn, true) || CurrentPlayer.Hand[Selection].Conditions[0].Evaluate() == 0)
                    {
                        CurrentPlayer.Choose_Position(true, turn);
                    }

                    CurrentPlayer.Money -= CurrentPlayer.Hand[Selection].Cost;
                    Context.Save(CurrentPlayer.Name + ".Money", CurrentPlayer.Money);
                    Tab[x, y] = CurrentPlayer.Hand[Selection];
                    CurrentPlayer.CampCards.Add(CurrentPlayer.Hand[Selection]);
                    CurrentPlayer.Hand.RemoveAt(Selection);
                    CurrentPlayer.CampCards[CurrentPlayer.CampCards.Count() - 1].Posx = x;
                    CurrentPlayer.CampCards[CurrentPlayer.CampCards.Count() - 1].Posy = y;
                    Aux = CurrentPlayer.CampCards[CurrentPlayer.CampCards.Count() - 1];
                    Context.Save(CurrentPlayer.Name + ".Camp", CurrentPlayer.CampCards.Count());
                    Context.Save(CurrentPlayer.Name + ".Hand", CurrentPlayer.Hand.Count());
                    Context.Save(Aux.Name + ".Posx", Aux.Posx);
                    Context.Save(Aux.Name + ".Posy", Aux.Posy);
                    await Task.Delay(1000);
                }
            }

            System.Console.WriteLine();
            System.Console.WriteLine("Moving Cards Camp");
            Message = "Moving Cards";
            StateHasChanged();
            for (int i = 0; i < CurrentPlayer.CampCards.Count(); i++)
            {
                Methods.ReadTab(Tab);
                Aux = CurrentPlayer.CampCards[i];
                System.Console.WriteLine("You have to pay {0} by {1}", (Aux.Cost / 10), Aux.Name);
                CurrentPlayer.Money -= Aux.Cost / 10;
                System.Console.WriteLine("Actual money {0}", CurrentPlayer.Money);
                Context.Save(CurrentPlayer.Name + ".Money", CurrentPlayer.Money);
                Console.WriteLine(143413515);
                if (CurrentPlayer is VirtualPlayer)
                {
                    System.Console.WriteLine("Moving Card : ");
                    Aux.ReadCard();
                    do
                    {
                        if (Aux.Range == 0)
                        {
                            System.Console.WriteLine("This card cannot move");
                            x = -1;
                            break;
                        }
                        CurrentPlayer.Choose_Position(false, turn);
                        if (x == -1) break;

                        if (x == Aux.Posx && y == Aux.Posy)
                        {
                            x = -1;
                            break;
                        }
                    }
                    while (!Methods.Validate_Position(x, y, Tab, turn, false)
                       || Methods.Distance(x, y, Aux.Posx, Aux.Posy) > Aux.Range);

                    if (x != -1)
                    {
                        Tab[x, y] = Aux;
                        Tab[Aux.Posx, Aux.Posy] = new Card();
                        Tab[Aux.Posx, Aux.Posy].Posx = Aux.Posx;
                        Tab[Aux.Posx, Aux.Posy].Posy = Aux.Posy;

                        Aux.Posx = x;
                        Aux.Posy = y;
                        System.Console.WriteLine("Card moved to " + CurrentPlayer.CampCards[i].Posx + " " + CurrentPlayer.CampCards[i].Posy);
                        Context.Save(Aux.Name + ".Posx", Aux.Posx);
                        Context.Save(Aux.Name + ".Posy", Aux.Posy);
                    }
                    await Task.Delay(500);
                }
                Console.WriteLine(1443);
                if(CurrentPlayer is VirtualPlayer)
                {
                    Console.WriteLine("afa");
                    bool[] used = new bool[Aux.Powers.Count()];
                    Selection = -1;
                    do
                    {
                        System.Console.WriteLine(Aux.Description);
                        CurrentPlayer.Choose_Power(Aux, used);
                        System.Console.WriteLine(Selection + "134");
                        if (Selection == -1)
                            break;

                     
                        used[Selection] = true;
                        if (Aux.Conditions[Selection].Evaluate() > 0)
                            Aux.Powers[Selection].Evaluate();
                        else System.Console.WriteLine("Power no available");
                        

                    } while (Selection != -1);
                    await Task.Delay(500);
                }
                
            }
            if(!(CurrentPlayer is VirtualPlayer))
                {
                    Console.WriteLine("papa");
                    await FirstPhase();
                    Message = "Waitting for actions";
                    
                    await ActionTime();
                    StateHasChanged();
                }
        }
    }
    protected override async void OnInitialized()
    {
        await GameLoop();
    }
    
}
*/