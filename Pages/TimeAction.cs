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

        public override async Task Run()
        {
            System.Console.WriteLine("A time action has been actived, it will be reactived {0} times", time);
            Play.MessegeAction = $"Is goin to Run an action {time} times";
            //await Task.Delay(1000);
            if (time <= 0)
            {
                System.Console.WriteLine("The power is finish");
                Play.MessegeAction = $"The power is finish";
              //  await Task.Delay(1000);
                return;
            }
            action.Evaluate();
            time--;
            if (Play.TimeActions.ContainsKey(action))
                Play.TimeActions[action] += time;
            else
            Play.TimeActions.Add(action, time);
        }
    }
}
