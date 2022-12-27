namespace OurGoT.Pages
{
    public abstract class Action : Expression
    {
        public int Evaluate()
        {
            Run();
            return 1;
        }
        public abstract void Run();
    }
}
