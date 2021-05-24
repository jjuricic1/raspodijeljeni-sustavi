using Akka.Configuration;

namespace Shared
{
    public class ConfigHelpers
    {
        public static Config ToActorSystemConfig(AkkaConfig config)
        {
            var fullConfig = new {akka = config};
            return ConfigurationFactory.FromObject(fullConfig);
        }
    }
}