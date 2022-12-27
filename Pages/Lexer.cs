namespace OurGoT.Pages
{
    public class Lexer
    {
        int ActionsCount = 0;
        int ConditionCount = 0;
        public string Text = "";
        public List<string> Asignations = new List<string>();
        public List<string> Errors = new List<string>();
        public string Title = "";
        public List<List<Token>> Tokens = new List<List<Token>>();
        public static bool Is_FieldS(string s)
        {
            if (s == "Name" || s == "Description" || s == "Picture")
                return true;
            return false;
        }
        public static bool Is_FieldE(string s)
        {
            if (s == "Life" || s == "Defense" || s == "Attack" || s == "Range" || s == "Cost")
                return true;
            return false;
        }
        public static bool Is_FieldC(string s)
        {
            if (s == "Powers" || s == "Conditions")
                return true;
            return false;
        }
        public static bool Is_Number(char c)
        {
            if ('0' <= c && c <= '9')
                return true;
            return false;
        }
        public static bool Is_Letter(char c)
        {
            if ('a' <= c && c <= 'z')
                return true;
            if ('A' <= c && c <= 'Z')
                return true;
            if (c == '.' || c == 'ñ' || c == '_')
                return true;
            return false;
        }
        public static bool Is_operation(char c)
        {
            if (c == '+' || c == '-' || c == '*' || c == '/')
                return true;
            return false;
        }
        public static bool Is_Var(string s, string Title)
        {
            if (s == "YourCamp" || s == "MyCamp" || s == "YourHand" || s == "MyHand" || s == "YourMoney" || s == "MyMoney")
                return true;
            bool flag = false;
            string left = "";
            string right = "";
            for (int i = 0; i < s.Count(); i++)
            {
                if (s[i] == '.')
                    flag = true;
                else if (!flag)
                    left += s[i];
                else
                    right += s[i];
            }
            bool Card = false;
            string[] paths = Directory.GetFiles("./Content", "*.*", SearchOption.AllDirectories);
            foreach (var x in paths)
            {
                string _title = x.Substring(10);
                if (_title == left)
                    Card = true;
            }
            if (left == Title)
                Card = true;
            if (Card && flag && Is_FieldE(right))
                return true;
            return false;
        }
        public void Read(string title)
        {
            System.Console.WriteLine("IEnter the features of the card : ");
            this.Title = title;
            while (true)
            {
                string line = Console.ReadLine()!;
                if (line == "end")
                    break;

                Text += line;
                Asignations.Add(line);
                Text += '\n';
            }
            Tokens = ToToken(Asignations, Title);
        }
        public static List<List<Token>> ToToken(List<string> Asignations, string Title)
        {
            List<List<Token>> sol = new List<List<Token>>();
            foreach (var x in Asignations)
            {
                string aux = "";
                aux += x[0];
                int ind = 1;
                List<Token> tokens = new List<Token>();
                while (ind < x.Length)
                {
                    char last = aux[aux.Length - 1];
                    char current = x[ind];
                    if ((Is_Number(last) && Is_Number(current)) || (Is_Letter(last) && Is_Letter(current)) || (last == '&' && current == '&') || (last == '|' && current == '|') || (last == '=' && current == '='))
                    {
                        aux += current;
                    }
                    else
                    {
                        if (aux != " " && aux[0] != '\n' && aux[0] != '\t' && aux != "")
                            tokens.Add(Token.Identify(aux, Title));
                        aux = "";
                        aux += current;
                    }
                    ind++;
                }
                if (aux != " ")
                    tokens.Add(Token.Identify(aux, Title));
                sol.Add(tokens);
            }
            return sol;
        }
        public static List<string> ToList(string s)
        {

            List<string> sol = new List<string>();
            string aux = "";
            int pos = 0;
            while (pos < s.Length)
            {
                if (s[pos] == '\n')
                {
                    sol.Add(aux);
                    aux = "";
                }
                else
                    aux += s[pos];
                pos++;
            }
            sol.Add(aux);
            return sol;
        }
        public List<Token> ToBalance(ref int i, List<Token> Value, int Line)
        {
            List<Token> ans = new List<Token>();
            if (Value[i].Value != "(")
            {
                Errors.Add("Missing parameter on line : " + Line);
                i = Value.Count();
                return ans;
            }
            int cur = 1;
            i++;
            while (i < Value.Count())
            {
                if (Value[i].Value == "(")
                    cur++;
                if (Value[i].Value == ")")
                    cur--;
                if (cur < 0)
                {
                    Errors.Add("Bad Balanced parenthesis line : " + Line);
                    i = Value.Count();
                    return new List<Token>();
                }
                if (cur == 0)
                {
                    i++;
                    return ans;
                }
                ans.Add(Value[i]);
                i++;
            }
            Errors.Add("Bad Balanced parenthesis line : " + Line);
            i = Value.Count();
            return new List<Token>();
        }
        public void CheckActions(List<Token> Value, int Line)
        {
            int i = 0;
            while (i < Value.Count())
            {
                Token current = Value[i];
                if (current.Type == "Sep")
                {
                    ActionsCount++;
                    i++;
                    continue;
                }
                if (current.Type != "Action0" && current.Type != "Action1" && current.Type != "Action2")
                {
                    i = Value.Count();
                    Errors.Add("There is no action like that line : " + Line);
                    continue;
                }
                if (current.Type == "Action0")
                {
                    i++;
                    continue;
                }
                if (i < Value.Count() - 1)
                    i++;
                List<Token> Balanced = ToBalance(ref i, Value, Line);
                if (current.Type == "Action1")
                {
                    CheckExpression(Balanced, Line);
                    continue;
                }
                bool div = false;
                List<Token> left = new List<Token>();
                List<Token> right = new List<Token>();
                foreach (var x in Balanced)
                {
                    if (x.Value == ",")
                        div = true;
                    else if (!div)
                        left.Add(x);
                    else
                        right.Add(x);
                }
                if (!div)
                {
                    Errors.Add("Missing paremeter on line " + Line);
                    continue;
                }
                CheckActions(left, Line);
                CheckExpression(right, Line);
            }
        }
        public void CheckExpression(List<Token> Value, int Line)
        {
            if (Value.Count() == 0)
            {
                Errors.Add("It was expected an expression on line : " + Line);
                return;
            }
            int curr = 0;
            bool flag = false;
            if (Value[0].Value != "(" && Value[0].Type != "Number" && Value[0].Type != "Var" && Value[0].Value != "-")
                Errors.Add("Syntax error on line : " + Line);
            for (int i = 0; i < Value.Count(); i++)
            {
                Token current = Value[i];
                if (Value[i].Value == "(")
                    curr++;
                if (Value[i].Value == ")")
                    curr--;
                if (curr < 0)
                    flag = true;
                if (i == 0)
                    continue;
                Token Last = Value[i - 1];
                if (current.Type == "Number" || current.Type == "Var")
                {
                    if (Last.Type != "Operation" && Last.Value != "(")
                        Errors.Add("Syntax error on line : " + Line);
                }
                else if (current.Type == "Operation")
                {
                    if (Last.Type != "Number" && Last.Value != ")" && Last.Type != "Var")
                        Errors.Add("Syntax error on line : " + Line);
                }
                else if (current.Type == "Parent")
                {
                    if (current.Value == "(" && Last.Type != "Operation" && Last.Value != "(")
                        Errors.Add("Syntax error on line : " + Line);
                    if (current.Value == ")" && Last.Type != "Number" && Last.Type != "Var" && Last.Value != ")")
                        Errors.Add("Syntax error on line : " + Line);
                }
                else
                {
                    Errors.Add("Syntax error on line : " + Line);
                }
            }
            if (flag)
            {
                Errors.Add("Bad Balanced Parenthesis on line : " + Line);
            }
        }
        public void CheckCondition(List<Token> Value, int Line)
        {
            int i = 0;
            List<Token> aux = new List<Token>();
            while (i < Value.Count())
            {
                Token curr = Value[i];
                if (curr.Type == "Sep")
                {
                    i++;
                    ConditionCount++;
                    CheckExpression(aux, Line);
                    aux = new List<Token>();
                    continue;
                }

                i++;
                aux.Add(curr);
            }
        }
        public void Check()
        {
            Dictionary<string, bool> Exist = new Dictionary<string, bool>();
            int Line = 0;
            foreach (var line in Tokens)
            {
                Line++;
                int i = 0;
                Token current = line[0];
                if (current.Type == "FieldS" || current.Type == "FieldE" || current.Type == "FieldC")
                {
                    if (Exist.ContainsKey(current.Value))
                    {
                        Errors.Add("Repetition of camp " + current.Value + " on line : " + Line);
                    }
                    else
                    {
                        if (line.Count() == 1)
                        {
                            Errors.Add("There is no assignament for camp " + current.Value + " on line : " + Line);
                            continue;
                        }
                        Exist.Add(current.Value, true);
                        i++;
                        current = line[i];
                        if (current.Type != "Asignation")
                        {
                            Errors.Add("It was expected = on line : " + Line);
                            continue;
                        }
                        i++;
                        List<Token> Value = new List<Token>();
                        while (i < line.Count())
                        {
                            Value.Add(line[i++]);
                        }
                        if (line[0].Type == "FieldS")
                            continue;
                        if (line[0].Type == "FieldE")
                            CheckExpression(Value, Line);
                        else if (line[0].Value == "Powers")
                            CheckActions(Value, Line);
                        else
                            CheckCondition(Value, Line);
                    }
                }
                else
                    Errors.Add("Error on line : " + Line);
            }
            if (ActionsCount != ConditionCount - 1)
                Errors.Add("There is not the same count of conditions that powers");
            if (Exist.Count() != 9)
                Errors.Add("Missing camps");
        }
    }
}
