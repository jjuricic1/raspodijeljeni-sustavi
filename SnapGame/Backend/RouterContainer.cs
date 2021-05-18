using Akka.Actor;
using Akka.Cluster.Tools.Client;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class RouterContainer : ReceiveActor
    {
        private readonly Props _props;
        private readonly IActorRef _router;

        public RouterContainer()
        {
            _props = Props.Create(() => new StorageActor())
                .WithRouter(FromConfig.Instance);
            _router = Context.ActorOf(_props, "router");
            RegisterService(_router);
        }

        private void RegisterService(IActorRef router)
        {
            var receptionist = ClusterClientReceptionist.Get(Context.System);
            receptionist.RegisterService(router);
        }
    }
}
