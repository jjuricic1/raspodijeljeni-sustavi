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

            Receive<GetAll>(all => GetAll());
            Receive<Get>(one => GetOne(one));
            Receive<JObject>(json => HandleJson(json));
        }

        private void HandleJson(JObject json)
        {
            Sender.Tell(json);
            Self.Tell(PoisonPill.Instance);
        }
        private void GetOne(Get one)
        {
            _clusterClient.Ask<JObject>(one)
                .PipeTo(Self, Sender);
            // var list = new List<Student>();
            // list.Add(new Student(
            //     1, "ante", "mate", "1234", "email@email.email", -1, true
            // ));
            //
            // var s = list.Find(student => student.Id == one.Id);
            // Sender.Tell(s);
            // Self.Tell(PoisonPill.Instance);
        }

        private void GetAll()
        {
            var list = new List<Card>();
            list.Add(new Card( 1, Rank.Ace, Color.Hearts, "Ace of ♥"));

            Sender.Tell(list);
            Self.Tell(PoisonPill.Instance);
        }
    }
}
