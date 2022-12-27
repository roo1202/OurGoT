namespace OurGoT.Pages
{
    public class Asignation : Expression
    {
        protected string a;
        protected Expression b;

        public Asignation(string a, Expression b)
        {
            this.a = a;
            this.b = b;
        }

        public int Evaluate()
        {
            int result = b.Evaluate();
            //   if(If.Is_If() == false)
            Play.Context.Save(a, result);
            //else
            // If.Contextnuevo.Save(a,result);
            return 1;
        }
    }
}
