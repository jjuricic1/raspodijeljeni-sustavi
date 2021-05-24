
namespace Backend
{
    public enum Color { Clubs, Spades, Diamonds, Hearts }
    public enum Rank { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

    public class Card
    {
        public int Id { get; }
        public string Name { get; }
        public Rank Rank { get; }
        public Color Color { get; }

        private string SetName(Rank rank, Color color)
        {
            return rank + " of " + color;
        }

        public Card(int id, Rank rank, Color color)
        {
            Id = id;
            Rank = rank;
            Color = color;
            Name = SetName(rank, color);
        }

    }

}
