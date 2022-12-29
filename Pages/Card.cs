namespace OurGoT.Pages
{
    public class Card
    {
        public string Name;
        public int Life;
        public int TotalLife;
        public int Cost;
        public int Defense;
        public int Attack;
        public string Description;
        public string Picture;
        public int Range;
        public int Posx;
        public int Posy;
        public List<Expression> Powers = new List<Expression>();
        public List<Expression> Conditions = new List<Expression>();
        public bool Moved = false;
        public List<bool> Used = new List<bool>(); 
        public Card()
        {
            Name = Description = Picture = "*";
            Posx = Posy = -1;

            //Life = TotalLife = Cost = Defense = Attack =Range = Posx = Posy = 0;

            Powers = new List<Expression>();
            Powers.Add(new Constant(0));
            Conditions = new List<Expression>();
            Conditions.Add(new Constant(0));
            Used.Add(false);
            Picture = "../Img/Empty.jpg";
        }

        public Card(string n, int v, int cos, int def, int a, string d, string f, int al, List<Expression> p, List<Expression> c)
        {
            Name = n;
            Life = TotalLife = v;
            Cost = cos;
            Defense = def;
            Attack = a;
            Description = d;
            Picture = f;
            Range = al;
            Powers = p;
            Conditions = c;
            Posx = Posy = -1;
            Used = new List<bool>();
            for (int i = 0; i < p.Count; i++)
                Used.Add(false);
        }

        public void ReadCard()
        {
            System.Console.WriteLine("Name : " + this.Name);
            System.Console.WriteLine("Life : " + this.Life);
            System.Console.WriteLine("Attack : " + this.Attack);
            System.Console.WriteLine("Defense : " + this.Defense);
            System.Console.WriteLine("Range : " + this.Range);
            System.Console.WriteLine("Cost : " + this.Cost);
            System.Console.WriteLine("Description : " + this.Description);
            System.Console.WriteLine("Position : " + this.Posx + " " + this.Posy);
            System.Console.WriteLine();
        }
    }
}
