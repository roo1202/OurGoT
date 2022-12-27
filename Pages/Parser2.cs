namespace OurGoT.Pages
{
    public class Parser2
    {
        public string Title = "";
        public Card card = new Card();
        List<List<Token>> Tokens;

        public Parser2(string Title, List<List<Token>> Tokens)
        {
            this.Title = Title;
            this.Tokens = Tokens;
        }
        public static Expression Calculate(Token value)
        {
            List<Token> aux = new List<Token>();
            aux.Add(value);
            return Calculate(aux);
        }
        public static Expression Calculate(List<Token> value)
        {
            if (value.Count() == 1)
            {
                if (value[0].Type == "Number")
                    return new Constant(int.Parse(value[0].Value));
                return new Var(value[0].Value);
            }
            if (value.Count() == 2)
                return new Mult(new Constant(-1), Calculate(value[1]));
            int O1 = -1;
            int O2 = -1;
            int O3 = -1;
            int O4 = -1;
            int cur = 0;
            int pos = 0;
            foreach (var x in value)
            {
                string op = x.Value;
                if (op == "(")
                    cur++;
                else if (op == ")")
                    cur--;
                if (cur == 0)
                {
                    if (op == "&&" || op == "||")
                        O1 = pos;
                    if (op == ">" || op == "<" || op == "==")
                        O2 = pos;
                    if (op == "+" || op == "-")
                        O3 = pos;
                    if (op == "*" || op == "/")
                        O4 = pos;
                }
                pos++;
            }
            pos = -1;
            if (O1 != -1)
                pos = O1;
            else if (O2 != -1)
                pos = O2;
            else if (O3 != -1)
                pos = O3;
            else if (O4 != -1)
                pos = O4;
            if (pos == -1)
            {
                List<Token> sol = new List<Token>();
                for (int i = 1; i < value.Count() - 1; i++)
                    sol.Add(value[i]);
                return Calculate(sol);
            }

            List<Token> left = new List<Token>();
            List<Token> right = new List<Token>();
            for (int i = 0; i < pos; i++)
                left.Add(value[i]);
            for (int i = pos + 1; i < value.Count(); i++)
                right.Add(value[i]);
            Expression leftResult = Calculate(left);
            Expression rightResult = Calculate(right);
            string operation = value[pos].Value;
            if (operation == "&&")
                return new And(leftResult, rightResult);
            else if (operation == "||")
                return new Or(leftResult, rightResult);
            else if (operation == "<")
                return new Minus(leftResult, rightResult);
            else if (operation == ">")
                return new Higher(leftResult, rightResult);
            else if (operation == "==")
                return new Iqual(leftResult, rightResult);
            else if (operation == "+")
                return new Sum(leftResult, rightResult);
            else if (operation == "-")
                return new Sub(leftResult, rightResult);
            else if (operation == "*")
                return new Mult(leftResult, rightResult);
            else
                return new Div(leftResult, rightResult);
        }

        public List<Expression> CutPowers(List<Token> value)
        {
            int pos = 0;
            Expression Aux = new Constant(0);
            List<Expression> Sol = new List<Expression>();
            while (pos < value.Count())
            {
                if (value[pos].Type == "Sep")
                {

                    Sol.Add(Aux);
                    Aux = new Constant(0);
                }
                else
                {
                    Expression power = new Constant(0);
                    int POS = pos;
                    if (value[pos].Type == "Action0")
                    {
                        if (value[pos].Value == "ToAttack")
                            power = new Attack(card);
                        else
                            power = new Sacrifice(card, Play.Graveyard, Play.Tab);
                    }
                    else
                    {
                        List<Token> Balanced = new List<Token>();
                        int cur = 1;
                        pos += 2;
                        while (pos < value.Count())
                        {
                            if (value[pos].Value == "(")
                                cur++;
                            else if (value[pos].Value == ")")
                                cur--;
                            if (cur == 0)
                                break;
                            Balanced.Add(value[pos]);
                            pos++;
                        }
                        if (value[POS].Type == "Action1")
                        {
                            switch (value[POS].Value)
                            {
                                case "Heal":
                                    {
                                        power = new Heal(card, Calculate(Balanced).Evaluate());
                                    }
                                    break;
                                case "UpAttack":
                                    {
                                        power = new UpAttack(card, Calculate(Balanced).Evaluate());
                                    }
                                    break;
                                case "UpRange":
                                    {
                                        power = new UpRange(card, Calculate(Balanced).Evaluate());
                                    }
                                    break;
                                case "UpDefense":
                                    {
                                        power = new UpDefense(card, Calculate(Balanced).Evaluate());
                                    }
                                    break;
                                case "Trade":
                                    {
                                        power = new Trade(Calculate(Balanced).Evaluate());
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            List<Token> First = new List<Token>();
                            List<Token> Second = new List<Token>();
                            bool flag = false;
                            foreach (var x in Balanced)
                            {
                                if (x.Value == ",")
                                    flag = true;
                                else if (flag)
                                    Second.Add(x);
                                else
                                    First.Add(x);

                            }
                            First.Add(new Token("Sep", "|"));
                            List<Expression> aux = CutPowers(First);
                            //System.Console.WriteLine(aux.Count());
                            if (value[POS].Value == "During")
                                power = new TimeAction(aux[0], Calculate(Second).Evaluate());
                            else
                                power = new Repetition(aux[0], Calculate(Second).Evaluate());
                        }
                    }
                    Aux = new And(Aux, power);
                }
                pos++;
            }
            return Sol;
        }

        public List<Expression> CutConditions(List<Token> value)
        {
            List<Expression> sol = new List<Expression>();
            List<Token> aux = new List<Token>();
            int pos = 0;
            while (pos < value.Count())
            {
                if (value[pos].Type == "Sep")
                {
                    sol.Add(Calculate(aux));
                    aux = new List<Token>();
                }
                else
                    aux.Add(value[pos]);
                pos++;
            }
            return sol;
        }
        public void Parsing()
        {
            foreach (var x in Tokens)
            {
                string Field = x[0].Value;
                if (x[0].Type == "FieldS")
                {
                    string value = "";
                    for (int i = 2; i < x.Count(); i++)
                    {
                        value += x[i].Value;
                        if (i < x.Count() - 1 && x[0].Value != "Picture")
                            value += " ";
                    }
                    if (Field == "Name")
                        card.Name = value;
                    else if (Field == "Description")
                        card.Description = value;
                    else
                        card.Picture = value;
                }
                else
                {
                    List<Token> value = new List<Token>();
                    for (int i = 2; i < x.Count(); i++)
                        value.Add(x[i]);
                    if (x[0].Type == "FieldE")
                    {

                        int result = Calculate(value).Evaluate();
                        if (Field == "Life")
                        {
                            card.TotalLife = result;
                            card.Life = result;
                            Play.Context.Save(card.Name + ".Life", card.Life);
                        }
                        else if (Field == "Attack")
                        {
                            card.Attack = result;
                            Play.Context.Save(card.Name + ".Attack", card.Attack);
                        }
                        else if (Field == "Defense")
                        {
                            card.Defense = result;
                            Play.Context.Save(card.Name + ".Defense", card.Defense);
                        }
                        else if (Field == "Cost")
                        {
                            card.Cost = result;
                            Play.Context.Save(card.Name + ".Cost", card.Cost);
                        }
                        else
                        {
                            card.Range = result;
                            Play.Context.Save(card.Name + ".Range", card.Range);
                        }
                        Play.Context.Save(card.Name + ".Posx", -1);
                        Play.Context.Save(card.Name + ".Posy", -1);
                    }
                    else
                    {
                        if (x[0].Value == "Powers")
                        {
                            List<Expression> Powers = CutPowers(value);
                            Powers.Insert(0, new Constant(0));
                            card.Powers = Powers;
                        }
                        else
                        {
                            card.Conditions = CutConditions(value);
                        }
                    }
                }
            }
        }
    }
}
