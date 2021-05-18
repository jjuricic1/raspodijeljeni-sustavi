using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Immutable;
using Shared;
using Akka.Actor;
using AkkaConfigProvider;
using Akka.Cluster.Tools.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Akka.Configuration;

namespace Frontend
{
    public class AkkaService : IHostedService
    {
        public static ActorSystem ActorSys { get; private set; }
        public static ImmutableHashSet<ActorPath> InitialContacts { get; private set; }
        public static ClusterClientSettings CClientSettings { get; private set; }

        private readonly IConfiguration _configuration;

        public AkkaService(IConfiguration configuration)
        {
            _configuration = configuration;
            var clusterName = configuration.GetSection("cluster:name").Get<string>();
            var clusterAddress = configuration.GetSection("cluster:domain").Get<string>();
            var ports = configuration.GetSection("cluster:ports").Get<int[]>();

            InitialContacts = ImmutableHashSet<ActorPath>.Empty
                .Add
                (
                    ActorPath.Parse($"akka.tcp://{clusterName}@{clusterAddress}:{ports[0]}/system/receptionist")
                )
                .Add
                (
                    ActorPath.Parse($"akka.tcp://{clusterName}@{clusterAddress}:{ports[1]}/system/receptionist")
                );
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = _configuration.GetSection("akka").Get<AkkaConfig>();
            var fullConfig = new { akka = config };
            var akkaConfig = ConfigurationFactory.FromObject(fullConfig);

            ActorSys = ActorSystem.Create("webapi", akkaConfig);

            CClientSettings = ClusterClientSettings.Create(ActorSys)
                .WithInitialContacts(InitialContacts);

            Console.WriteLine($"[{DateTime.Now}] ActorSys started!");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return ActorSys.Terminate().ContinueWith(_ =>
                Console.WriteLine($"[{DateTime.Now}] ActorSys terminated!"));
            // return CoordinatedShutdown.Get(ActorSys).Run(CoordinatedShutdown.ActorSystemTerminateReason.Instance);
        }
    }
}
