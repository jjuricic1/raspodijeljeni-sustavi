using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Frontend.Models;
using Frontend.Actors;
using Frontend.DtoMappers;
using Newtonsoft.Json.Linq;
using Akka.Actor;
using Shared;
using System.Collections.Generic;

namespace Frontend.Controllers
{
    [Route("api/cards")]
    [ApiController]
    public class CardsApiController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<JArray>> Get()
        {
            var props = Props.Create(() => new ConnectionActor(AkkaService.CClientSettings));
            var actor = AkkaService.ActorSys.ActorOf(props);

            var result = await actor.Ask<JArray>(new GetAll());

            return result;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<JObject>> Get(int id)
        {
            /*        var props = Props.Create(() => new ConnectionActor(AkkaService.CClientSettings));
                    var actor = AkkaService.ActorSys.ActorOf(props);

                    var result = await actor.Ask<Card>(new Get(id));*/

            var props = Props.Create(() => new ConnectionActor(AkkaService.CClientSettings));
            var actor = AkkaService.ActorSys.ActorOf(props);

            var result = await actor.Ask<JObject>(new Get(id));

            return result;
        }

        [HttpPost("save")]
        public ActionResult Save([FromBody] JObject json)
        {
            //var card = CardDto.FromJson(json);
            return Ok();
        }
    }
}
