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
        public async Task<ActionResult<List<Card>>> Get()
        {
            var props = Props.Create(() => new ConnectionActor(AkkaService.CClientSettings));
            var actor = AkkaService.ActorSys.ActorOf(props);

            var result = await actor.Ask<List<Card>>(new GetAll());

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> Get(int id)
        {
            var props = Props.Create(() => new ConnectionActor(AkkaService.CClientSettings));
            var actor = AkkaService.ActorSys.ActorOf(props);

            var result = await actor.Ask<Card>(new Get(id));

            return Ok(result);

            // return _students.Find(x => x.Id == id);
        }

        [HttpPost("save")]
        public ActionResult Save([FromBody] JObject json)
        {
            var student = CardDto.FromJson(json);
            return Ok();
        }
    }
}
