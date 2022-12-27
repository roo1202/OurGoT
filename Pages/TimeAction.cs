namespace OurGoT.Pages
{
    public class TimeAction : Action
    {
        Expression action;
        int time;

        public TimeAction(Expression action, int time)
        {
            this.action = action;
            this.time = time;
        }

        public override void Run()
        {
            System.Console.WriteLine("A time action has been actived, it will be reactived {0} times", time);
            if (time <= 0)
            {
                System.Console.WriteLine("The power is finish");
                return;
            }
            action.Evaluate();
            time--;
            Play.TimeActions.Add(action, time);
        }
    }
}
