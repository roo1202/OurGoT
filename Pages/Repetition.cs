namespace OurGoT.Pages
{
    public class Repetition : Action
    {
        Expression action;
        int n;

        public Repetition(Expression action, int n)
        {
            this.action = action;
            this.n = n;
        }
        public override async   Task Run()
        {
            System.Console.WriteLine("Is goin to Run an action {0} times", n);
            Play.MessegeAction = $"Is goin to Run an action {n} times";
            //await Task.Delay(1000);
            for (int i = 0; i < n; i++)
            {
                action.Evaluate();
            }
        }
    }
}
