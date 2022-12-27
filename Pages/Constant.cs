namespace OurGoT.Pages
{
    public class Constant : Expression
    {
        int expression;
        public Constant(int expression)
        {
            this.expression = expression;
        }
        public int Evaluate()
        {
            return expression;
        }
    }
}
