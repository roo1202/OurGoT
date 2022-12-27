namespace OurGoT.Pages
{
    public class Minus : Binary
    {
        public Minus(Expression expression1, Expression expression2) : base(expression1, expression2)
        {
        }
        protected override int Evaluate(int result1, int result2)
        {
            if (result1 < result2)
                return 1;
            return 0;
        }
    }
}
