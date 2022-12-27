namespace OurGoT.Models
{
    using OurGoT.Pages;
    public class CardModel
    {
        public Card card { get; set; }
        public int id { get; set; }
        public string Style { get; set; }
        public bool opacity { get; set; }
        public bool Opacity 
        { get => opacity;

            set
            {
                opacity = value;
                if(!value)
                {
                    Style = "Opacity";
                }
                else
                {
                    Style = "";
                }

            }
        }
    }
}
