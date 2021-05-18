using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.AkkaExtensions
{
    public class ServiceScopeExtensionIdProvider : ExtensionIdProvider<ServiceScopeExtension>
    {
        public static ServiceScopeExtensionIdProvider Instance = new ServiceScopeExtensionIdProvider();

        public override ServiceScopeExtension CreateExtension(ExtendedActorSystem system)
        {
            return new ServiceScopeExtension();
        }
    }
}
