namespace OurGoT.Pages
{
    public class And : Binary
    {
        public And(Expression expression1, Expression expression2) : base(expression1, expression2)
        {

        }
        protected override int Evaluate(int result1, int result2)
        {
            if (result1 != 0 && result2 != 0)
                return 1;
            return 0;
        }
    }
}
