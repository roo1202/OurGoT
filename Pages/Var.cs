namespace OurGoT.Pages
{
    public class Var : Expression
    {
        string expression;
        public Var(string expression)
        {
            this.expression = expression;
        }
        public int Evaluate()
        {
            switch (expression)
            {
                case "MyMoney":
                    expression = Play.CurrentPlayer.Name + ".Money";
                    break;
                case "YourMoney":
                    expression = Play.NextPlayer.Name + ".Money";
                    break;
                case "MyHand":
                    expression = Play.CurrentPlayer.Name + ".Hand";
                    break;
                case "YourHand":
                    expression = Play.NextPlayer.Name + ".Hand";
                    break;
                case "MyCamp":
                    expression = Play.CurrentPlayer.Name + ".CampCards";
                    break;
                case "YourCamp":
                    expression = Play.NextPlayer.Name + ".CampCards";
                    break;
            }
            return Play.Context.Get(expression);
        }
    }
}
