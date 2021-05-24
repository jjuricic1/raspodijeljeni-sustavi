 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;
using Akka.Actor;
using Akka.Cluster.Tools.Client;
using Newtonsoft.Json.Linq;
using Frontend.Models;

namespace Frontend.Actors
{
    public class ConnectionActor : ReceiveActor
    {
        private IActorRef _clusterClient;

        public ConnectionActor(ClusterClientSettings clusterClientSettings)
        {
            var props = ClusterClient.Props(clusterClientSettings);
            _clusterClient = Context.ActorOf(props);

            Receive<GetAll>(all => HandleGetAll(all));
            Receive<Get>(one => HandleGetOne(one));
            Receive<GetAllResult>(res =>
            {
                Sender.Tell(res.JArray);
                Self.Tell(PoisonPill.Instance);
            });
            Receive<GetResult>(res =>
            {
                Sender.Tell(res.Json);
                Self.Tell(PoisonPill.Instance);
            });
        }

        private void HandleJson(JObject json)
        {
            Sender.Tell(json);
            Self.Tell(PoisonPill.Instance);
        }
        private void HandleGetOne(Get msg)
        {
            _clusterClient.Ask<GetResult>(new ClusterClient.Send("/user/manager", msg)).PipeTo(Self, Sender);
        }

        private void HandleGetAll(GetAll msg)
        {
            _clusterClient.Ask<GetAllResult>(new ClusterClient.Send("/user/manager", msg)).PipeTo(Self, Sender);
        }
    }
}
