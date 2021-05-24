using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Routing;
using Shared;

namespace Backend
{
    public class ManagerActor :ReceiveActor
    {
        private IActorRef _router;
         public ManagerActor()
        {
            var props = Props.Create(() => new StorageActor()).WithRouter(FromConfig.Instance);
            _router = Context.ActorOf(props, "router");

            Receive<Get>(get => _router.Forward(get));

            Receive<string>(c => Console.WriteLine(c));
        }
    }
}
