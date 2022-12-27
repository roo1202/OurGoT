namespace OurGoT.Pages
{
    public class Context
    {
        public Dictionary<string, int> context = new Dictionary<string, int>();
        public Context()
            {
            context.Add(".Money", 0);
            context.Add(".Hand", 0);
            context.Add(".CampCards",0);
        }

        public virtual void Save(string id, int value)
        {
            if (context.ContainsKey(id))
            {
                context[id] = value;
            }
            else
            {
                context.Add(id, value);
            }
        }
        public int Get(string id)
        {
            return context[id];
        }

    }
}
