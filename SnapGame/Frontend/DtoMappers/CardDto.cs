using Frontend.Models;
using Newtonsoft.Json.Linq;
using System;

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


        private static int UnwrapInt(JToken token)
        {
            var a = token?["$"]?.ToObject<string>();
            return int.Parse(a?.Substring(1) ?? throw new ArgumentException());
        }


        public static Card FromAkkaJson(JObject json)
        {
            var id = UnwrapInt(json["id"]);

            var rank = json["rank"].ToObject<Rank>();
            var color = json["color"].ToObject<Color>();
            var name = json["name"].ToObject<string>();


            return new Card(id, rank, color, name);
        }
    }
}
