namespace OurGoT.Models
{
    using OurGoT.Pages;
    public class GridModel
    {
        public int Idx { get; set; } 
        public int Idy { get; set; }
        public Card Card { get; set; }
        private bool isInvisible { get; set; }
        public string Style { get; set; }
        public bool IsInvisible 
        { 
            get => isInvisible; 
            set
            {
                isInvisible = value;
                if(value)
                {
                    Style = "Invisible";
                }
                else
                {
                    Style = "";
                }
            } 
        }
    }
}
