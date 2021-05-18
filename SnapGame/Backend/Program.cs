using Akka.Actor;
using System;
using Akka.Configuration;
using Backend.DB;
using System.IO;
using System.Threading.Tasks;
using Backend.AkkaExtensions;
using System.Collections.Generic;
using Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
    class Program
    {
        static void FillDB(ActorSelection actor)
        {
            var cards = new List<Card>();
            #region Clubs
            cards.Add(new Card { Rank = Rank.Ace, Color = Color.Clubs });
            cards.Add(new Card { Rank = Rank.Two, Color = Color.Clubs });
            cards.Add(new Card { Rank = Rank.Three, Color = Color.Clubs });
            cards.Add(new Card { Rank = Rank.Four, Color = Color.Clubs });
            cards.Add(new Card { Rank = Rank.Five, Color = Color.Clubs });
            cards.Add(new Card { Rank = Rank.Six, Color = Color.Clubs });
            cards.Add(new Card { Rank = Rank.Seven, Color = Color.Clubs });
            cards.Add(new Card { Rank = Rank.Eight, Color = Color.Clubs });
            cards.Add(new Card { Rank = Rank.Nine, Color = Color.Clubs });
            cards.Add(new Card { Rank = Rank.Ten, Color = Color.Clubs });
            cards.Add(new Card { Rank = Rank.Jack, Color = Color.Clubs });
            cards.Add(new Card { Rank = Rank.Queen, Color = Color.Clubs });
            cards.Add(new Card { Rank = Rank.King, Color = Color.Clubs });
            #endregion

            #region Diamonds
            cards.Add(new Card { Rank = Rank.Ace, Color = Color.Diamonds });
            cards.Add(new Card { Rank = Rank.Two, Color = Color.Diamonds });
            cards.Add(new Card { Rank = Rank.Three, Color = Color.Diamonds });
            cards.Add(new Card { Rank = Rank.Four, Color = Color.Diamonds });
            cards.Add(new Card { Rank = Rank.Five, Color = Color.Diamonds });
            cards.Add(new Card { Rank = Rank.Six, Color = Color.Diamonds });
            cards.Add(new Card { Rank = Rank.Seven, Color = Color.Diamonds });
            cards.Add(new Card { Rank = Rank.Eight, Color = Color.Diamonds });
            cards.Add(new Card { Rank = Rank.Nine, Color = Color.Diamonds });
            cards.Add(new Card { Rank = Rank.Ten, Color = Color.Diamonds });
            cards.Add(new Card { Rank = Rank.Jack, Color = Color.Diamonds });
            cards.Add(new Card { Rank = Rank.Queen, Color = Color.Diamonds });
            cards.Add(new Card { Rank = Rank.King, Color = Color.Diamonds });
            #endregion

            #region Spades
            cards.Add(new Card { Rank = Rank.Ace, Color = Color.Spades });
            cards.Add(new Card { Rank = Rank.Two, Color = Color.Spades });
            cards.Add(new Card { Rank = Rank.Three, Color = Color.Spades });
            cards.Add(new Card { Rank = Rank.Four, Color = Color.Spades });
            cards.Add(new Card { Rank = Rank.Five, Color = Color.Spades });
            cards.Add(new Card { Rank = Rank.Six, Color = Color.Spades });
            cards.Add(new Card { Rank = Rank.Seven, Color = Color.Spades });
            cards.Add(new Card { Rank = Rank.Eight, Color = Color.Spades });
            cards.Add(new Card { Rank = Rank.Nine, Color = Color.Spades });
            cards.Add(new Card { Rank = Rank.Ten, Color = Color.Spades });
            cards.Add(new Card { Rank = Rank.Jack, Color = Color.Spades });
            cards.Add(new Card { Rank = Rank.Queen, Color = Color.Spades });
            cards.Add(new Card { Rank = Rank.King, Color = Color.Spades });
            #endregion

            #region Hearts
            cards.Add(new Card { Rank = Rank.Ace, Color = Color.Hearts });
            cards.Add(new Card { Rank = Rank.Two, Color = Color.Hearts });
            cards.Add(new Card { Rank = Rank.Three, Color = Color.Hearts });
            cards.Add(new Card { Rank = Rank.Four, Color = Color.Hearts });
            cards.Add(new Card { Rank = Rank.Five, Color = Color.Hearts });
            cards.Add(new Card { Rank = Rank.Six, Color = Color.Hearts });
            cards.Add(new Card { Rank = Rank.Seven, Color = Color.Hearts });
            cards.Add(new Card { Rank = Rank.Eight, Color = Color.Hearts });
            cards.Add(new Card { Rank = Rank.Nine, Color = Color.Hearts });
            cards.Add(new Card { Rank = Rank.Ten, Color = Color.Hearts });
            cards.Add(new Card { Rank = Rank.Jack, Color = Color.Hearts });
            cards.Add(new Card { Rank = Rank.Queen, Color = Color.Hearts });
            cards.Add(new Card { Rank = Rank.King, Color = Color.Hearts });
            #endregion 

            cards.ForEach(c =>
                actor.Ask<SaveSuccess>(
                    new SaveCard(c)).ContinueWith(c => Console.WriteLine(c.Id)));
        }
        static void SetupDb(IServiceCollection services)
        {
            services.AddDbContext<CardContext>(opt => opt.UseInMemoryDatabase("Cards"));
        }

        private static Config GetAkkaConfig(IConfiguration configuration)
        {
            var port = configuration.GetValue<int?>("port") ?? 0;

            var config = configuration.GetSection("akka").Get<AkkaConfig>();

            var fullConfig = new { akka = config };
            var akkaConfig = ConfigurationFactory.FromObject(fullConfig);
            return ConfigurationFactory.ParseString($"akka.remote.dot-netty.tcp.port={port}")
                .WithFallback(akkaConfig);
        }
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            var currDir = Directory.GetCurrentDirectory();
            var lastIndex = currDir.LastIndexOf("bin", StringComparison.InvariantCultureIgnoreCase);
            var projectDir =
                lastIndex < 0
                    ? currDir
                    : currDir.Substring(0, currDir.LastIndexOf("bin", StringComparison.InvariantCultureIgnoreCase));

            var configuration = new ConfigurationBuilder()
                .SetBasePath(projectDir)
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .Build();

            SetupDb(services);

            var akkaConfig = GetAkkaConfig(configuration);

            Console.WriteLine(akkaConfig.GetInt("akka.remote.dot-netty.tcp.port"));

            using (var system = ActorSystem.Create("cluster", akkaConfig))
            {
                var serviceProvider = services.BuildServiceProvider();
                var scopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
                system.AddServiceScopeFactory(scopeFactory);

                var props = Props.Create(() => new RouterContainer());
                system.ActorOf(props, "routerContainer");

                Task.Delay(1000).Wait();

                var selection = system.ActorSelection("/user/routerContainer/router");
                FillDB(selection);

                Console.ReadLine();
                CoordinatedShutdown.Get(system).Run(CoordinatedShutdown.ActorSystemTerminateReason.Instance);
            }

        }  
       


    }
}
