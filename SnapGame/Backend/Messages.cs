using System;


namespace Backend
{
    public class SaveCard
    {
        public DB.Card Card { get; set; }

        public SaveCard(DB.Card card)
        {
            Card = card;
        }
    }
}
