namespace OurGoT.Pages
{
    public class Token
    {
        public string Type = "";
        public string Value = "";
        public Token(string Type, string Value)
        {
            this.Type = Type;
            this.Value = Value;
        }
        public static Token Identify(string s, string Title)
        {
            Token ans;
            if (Lexer.Is_Number(s[0]))
                ans = new Token("Number", s);
            else if (Lexer.Is_Letter(s[0]))
            {
                if (Lexer.Is_FieldS(s))
                    ans = new Token("FieldS", s);
                else if (Lexer.Is_FieldE(s))
                    ans = new Token("FieldE", s);
                else if (Lexer.Is_FieldC(s))
                    ans = new Token("FieldC", s);
                else if (s == "ToAttack" || s == "Sacrifice")
                    ans = new Token("Action0", s);
                else if (s == "UpDefense" || s == "UpRange" || s == "Trade" || s == "Heal" || s == "UpAttack")
                    ans = new Token("Action1", s);
                else if (s == "During" || s == "Repeat")
                    ans = new Token("Action2", s);
                else if (Lexer.Is_Var(s, Title))
                    ans = new Token("Var", s);
                else
                    ans = new Token("Error", s);
            }
            else if (Lexer.Is_operation(s[0]))
                ans = new Token("Operation", s);
            else if (s.Length == 2 || s == ">" || s == "<")
                ans = new Token("Operation", s);
            else if (s == "(" || s == ")")
                ans = new Token("Parent", s);
            else if (s == "|")
                ans = new Token("Sep", s);
            else if (s == "=")
                ans = new Token("Asignation", s);
            else
                ans = new Token("Error", s);
            return ans;
        }
    }
}
