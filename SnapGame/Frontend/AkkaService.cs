using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Immutable;
using Shared;
using Akka.Actor;
using Akka.Cluster.Tools.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Akka.Configuration;
using AkkaConfigProvider;

namespace Frontend
{
    public class AkkaService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private ImmutableHashSet<ActorPath> _initialContacts;
                
        public static ActorSystem ActorSys { get; private set; }
        public static ClusterClientSettings CClientSettings { get; set; }

        public AkkaService(IConfiguration configuration)
        {
            _configuration = configuration;
            
            var clusterName = configuration.GetSection("cluster:name").Get<string>();
            var clusterAddress = configuration.GetSection("cluster:domain").Get<string>();
            var ports = configuration.GetSection("cluster:ports").Get<int[]>();

            _initialContacts = ImmutableHashSet<ActorPath>.Empty
                .Add
                (
                    ActorPath.Parse($"akka.tcp://Cluster@localhost:12000/system/receptionist")
                )
                .Add
                (
                    ActorPath.Parse($"akka.tcp://Cluster@localhost:12001/system/receptionist")
                );
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //var config = _configuration.GetSection("akka").Get<AkkaConfig>();
            //var fullConfig = new { akka = config };
            //var akkaConfig = ConfigurationFactory.FromObject(fullConfig);

            var provider = new ConfigProvider();
            var config = provider.GetAkkaConfig<AkkaConfig>();

            ActorSys = ActorSystem.Create("Cluster", config);

            CClientSettings = ClusterClientSettings.Create(ActorSys)
                .WithInitialContacts(_initialContacts);

            Console.WriteLine($"[{DateTime.Now}] *** ActorSys started! ***");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return ActorSys.Terminate().ContinueWith(_ =>
                Console.WriteLine($"[{DateTime.Now}] *** ActorSys terminated! ***"));
            // return CoordinatedShutdown.Get(ActorSys).Run(CoordinatedShutdown.ActorSystemTerminateReason.Instance);
        }
    }
}
