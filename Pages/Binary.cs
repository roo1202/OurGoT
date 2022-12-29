namespace OurGoT.Pages
{
    public abstract class Binary : Expression
    {
        protected Expression expression1;
        protected Expression expression2;
        public Binary(Expression expression1, Expression expression2)
        {
            this.expression1 = expression1;
            this.expression2 = expression2;
        }

        public int Evaluate()
        {
           
            int result1 = expression1.Evaluate();
           
            int result2 = expression2.Evaluate();
            return this.Evaluate(result1, result2);
        }
        protected abstract int Evaluate(int result1, int result2);
    }
}
