using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Shared;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Backend.DB;
using Backend.AkkaExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
   
    public class StorageActor : ReceiveActor
    {
        
        /*private Stack<Card> _deck;

        private void SetDeck()
        {
            #region Clubs
            _deck.Push(new Card(1, Rank.Ace, Color.Clubs));
            _deck.Push(new Card(2, Rank.Two, Color.Clubs));
            _deck.Push(new Card(3, Rank.Three, Color.Clubs));
            _deck.Push(new Card(4, Rank.Four, Color.Clubs));
            _deck.Push(new Card(5, Rank.Five, Color.Clubs));
            _deck.Push(new Card(6, Rank.Six, Color.Clubs));
            _deck.Push(new Card(7, Rank.Seven, Color.Clubs));
            _deck.Push(new Card(8, Rank.Eight, Color.Clubs));
            _deck.Push(new Card(9, Rank.Nine, Color.Clubs));
            _deck.Push(new Card(10, Rank.Ten, Color.Clubs));
            _deck.Push(new Card(11, Rank.Jack, Color.Clubs));
            _deck.Push(new Card(12, Rank.Queen, Color.Clubs));
            _deck.Push(new Card(13, Rank.King, Color.Clubs));
            #endregion

            #region Diamonds
            _deck.Push(new Card(1, Rank.Ace, Color.Diamonds));
            _deck.Push(new Card(2, Rank.Two, Color.Diamonds));
            _deck.Push(new Card(3, Rank.Three, Color.Diamonds));
            _deck.Push(new Card(4, Rank.Four, Color.Diamonds));
            _deck.Push(new Card(5, Rank.Five, Color.Diamonds));
            _deck.Push(new Card(6, Rank.Six, Color.Diamonds));
            _deck.Push(new Card(7, Rank.Seven, Color.Diamonds));
            _deck.Push(new Card(8, Rank.Eight, Color.Diamonds));
            _deck.Push(new Card(9, Rank.Nine, Color.Diamonds));
            _deck.Push(new Card(10, Rank.Ten, Color.Diamonds));
            _deck.Push(new Card(11, Rank.Jack, Color.Diamonds));
            _deck.Push(new Card(12, Rank.Queen, Color.Diamonds));
            _deck.Push(new Card(13, Rank.King, Color.Diamonds));
            #endregion

            #region Spades
            _deck.Push(new Card(1, Rank.Ace, Color.Spades));
            _deck.Push(new Card(2, Rank.Two, Color.Spades));
            _deck.Push(new Card(3, Rank.Three, Color.Spades));
            _deck.Push(new Card(4, Rank.Four, Color.Spades));
            _deck.Push(new Card(5, Rank.Five, Color.Spades));
            _deck.Push(new Card(6, Rank.Six, Color.Spades));
            _deck.Push(new Card(7, Rank.Seven, Color.Spades));
            _deck.Push(new Card(8, Rank.Eight, Color.Spades));
            _deck.Push(new Card(9, Rank.Nine, Color.Spades));
            _deck.Push(new Card(10, Rank.Ten, Color.Spades));
            _deck.Push(new Card(11, Rank.Jack, Color.Spades));
            _deck.Push(new Card(12, Rank.Queen, Color.Spades));
            _deck.Push(new Card(13, Rank.King, Color.Spades));
            #endregion

            #region Hearts
            _deck.Push(new Card(1, Rank.Ace, Color.Hearts));
            _deck.Push(new Card(2, Rank.Two, Color.Hearts));
            _deck.Push(new Card(3, Rank.Three, Color.Hearts));
            _deck.Push(new Card(4, Rank.Four, Color.Hearts));
            _deck.Push(new Card(5, Rank.Five, Color.Hearts));
            _deck.Push(new Card(6, Rank.Six, Color.Hearts));
            _deck.Push(new Card(7, Rank.Seven, Color.Hearts));
            _deck.Push(new Card(8, Rank.Eight, Color.Hearts));
            _deck.Push(new Card(9, Rank.Nine, Color.Hearts));
            _deck.Push(new Card(10, Rank.Ten, Color.Hearts));
            _deck.Push(new Card(11, Rank.Jack, Color.Hearts));
            _deck.Push(new Card(12, Rank.Queen, Color.Hearts));
            _deck.Push(new Card(13, Rank.King, Color.Hearts));
            #endregion 

        }

        public void ShuffleDeck()
        {
            Random rnd = new Random();
            var values = _deck.ToArray();
            _deck.Clear();
            foreach (var value in values.OrderBy(x => rnd.Next()))
                _deck.Push(value);
        }*/

        public StorageActor()
        {
            ReceiveAsync<GetAll>(x => RespondWithAsync());
            ReceiveAsync<Get>(x => RespondWithAsync(x));
            ReceiveAsync<SaveCard>(x => SaveAsync(x));
            ReceiveAsync<Save>(x => SaveAsync(x));
        }

        private async Task SaveAsync(SaveCard save)
        {
            using (var scope = Context.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<CardContext>();
                await context.Card.AddAsync(save.Card);
                var id = await context.SaveChangesAsync();
                Sender.Tell(new SaveSuccess(id));
            }
        }

        private async Task SaveAsync(Save save)
        {
            var json = save.Json;
            var card = new DB.Card
            {
                Name = json["name"].ToObject<string>(),
                Rank = json["rank"].ToObject<Rank>(),
                Color = json["color"].ToObject<Color>(),
                
            };

            await SaveAsync(new SaveCard(card));
        }

        public async Task RespondWithAsync()
        {
            using (var scope = Context.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<CardContext>();
                var cards = await context.Card.ToListAsync();
                Sender.Tell(JArray.FromObject(cards));
            }
        }

        public async Task RespondWithAsync(Get get)
        {
            using (var scope = Context.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<CardContext>();
                var card = await context.Card.FirstOrDefaultAsync(s => s.Id == get.Id);
                Sender.Tell(JObject.FromObject(card));
            }
        }



/*
        protected override void PreStart()
        {
            _deck = new Stack<Card>();
            SetDeck();
            ShuffleDeck();
            
            base.PreStart();
        }*/
    }
}
