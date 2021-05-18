using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.DB
{
    public enum Color { Clubs, Spades, Diamonds, Hearts }
    public enum Rank { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Rank Rank { get; set; }
        public Color Color { get; set; }
        
    }
}
