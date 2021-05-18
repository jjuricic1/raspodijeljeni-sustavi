using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.AkkaExtensions
{
    public static class Extensions
    {
        public static void AddServiceScopeFactory(this ActorSystem actorSystem,
            IServiceScopeFactory iServiceScopeFactory)
        {
            var instance = ServiceScopeExtensionIdProvider.Instance;
            actorSystem.RegisterExtension(instance);
            instance.Get(actorSystem).Initialize(iServiceScopeFactory);
        }

        public static IServiceScope CreateScope(this IActorContext context)
        {
            return ServiceScopeExtensionIdProvider.Instance.Get(context.System).CreateScope();
        }
    }
}
