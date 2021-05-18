using Akka.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ConfigHelpers
    {
        public static Config ToActorSystemConfig(AkkaConfig config)
        {
            var fullConfig = new { akka = config };
            return ConfigurationFactory.FromObject(fullConfig);
        }
    }
}
