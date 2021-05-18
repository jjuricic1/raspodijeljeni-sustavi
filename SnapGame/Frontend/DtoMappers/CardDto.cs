using Frontend.Models;
using Newtonsoft.Json.Linq;


namespace Frontend.DtoMappers
{
    public class CardDto
    {
        public static Card FromJson(JObject json)
        {
            var id = json["id"].ToObject<int>();
            var rank = json["rank"].ToObject<Rank>();
            var color = json["color"].ToObject<Color>();
            var name = json["name"].ToObject<string>();
            

            return new Card(id, rank, color, name);
        }
    }
}
