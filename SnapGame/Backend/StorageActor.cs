using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Shared;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
   
    public class StorageActor : ReceiveActor
    {
        
        private Stack<Card> _deck;

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
            _deck.Push(new Card(14, Rank.Ace, Color.Diamonds));
            _deck.Push(new Card(15, Rank.Two, Color.Diamonds));
            _deck.Push(new Card(16, Rank.Three, Color.Diamonds));
            _deck.Push(new Card(17, Rank.Four, Color.Diamonds));
            _deck.Push(new Card(18, Rank.Five, Color.Diamonds));
            _deck.Push(new Card(19, Rank.Six, Color.Diamonds));
            _deck.Push(new Card(20, Rank.Seven, Color.Diamonds));
            _deck.Push(new Card(21, Rank.Eight, Color.Diamonds));
            _deck.Push(new Card(22, Rank.Nine, Color.Diamonds));
            _deck.Push(new Card(23, Rank.Ten, Color.Diamonds));
            _deck.Push(new Card(24, Rank.Jack, Color.Diamonds));
            _deck.Push(new Card(25, Rank.Queen, Color.Diamonds));
            _deck.Push(new Card(26, Rank.King, Color.Diamonds));
            #endregion

            #region Spades
            _deck.Push(new Card(27, Rank.Ace, Color.Spades));
            _deck.Push(new Card(28, Rank.Two, Color.Spades));
            _deck.Push(new Card(29, Rank.Three, Color.Spades));
            _deck.Push(new Card(30, Rank.Four, Color.Spades));
            _deck.Push(new Card(31, Rank.Five, Color.Spades));
            _deck.Push(new Card(32, Rank.Six, Color.Spades));
            _deck.Push(new Card(33, Rank.Seven, Color.Spades));
            _deck.Push(new Card(34, Rank.Eight, Color.Spades));
            _deck.Push(new Card(35, Rank.Nine, Color.Spades));
            _deck.Push(new Card(36, Rank.Ten, Color.Spades));
            _deck.Push(new Card(37, Rank.Jack, Color.Spades));
            _deck.Push(new Card(38, Rank.Queen, Color.Spades));
            _deck.Push(new Card(39, Rank.King, Color.Spades));
            #endregion

            #region Hearts
            _deck.Push(new Card(40, Rank.Ace, Color.Hearts));
            _deck.Push(new Card(41, Rank.Two, Color.Hearts));
            _deck.Push(new Card(42, Rank.Three, Color.Hearts));
            _deck.Push(new Card(43, Rank.Four, Color.Hearts));
            _deck.Push(new Card(44, Rank.Five, Color.Hearts));
            _deck.Push(new Card(45, Rank.Six, Color.Hearts));
            _deck.Push(new Card(46, Rank.Seven, Color.Hearts));
            _deck.Push(new Card(47, Rank.Eight, Color.Hearts));
            _deck.Push(new Card(48, Rank.Nine, Color.Hearts));
            _deck.Push(new Card(49, Rank.Ten, Color.Hearts));
            _deck.Push(new Card(50, Rank.Jack, Color.Hearts));
            _deck.Push(new Card(51, Rank.Queen, Color.Hearts));
            _deck.Push(new Card(52, Rank.King, Color.Hearts));
            #endregion 

        }

        public void ShuffleDeck()
        {
            Random rnd = new Random();
            var values = _deck.ToArray();
            _deck.Clear();
            foreach (var value in values.OrderBy(x => rnd.Next()))
                _deck.Push(value);
        }

        public StorageActor()
        {
            Receive<GetAll>(x => HandleGetAll());
            Receive<Get>(x => HandleGet(x));
        }

        public void HandleGet(Get get)
        {
            var card = _deck.FirstOrDefault(c => c.Id == get.Id);
            var json = card == null ? new JObject() : JObject.FromObject(card);

            Console.WriteLine(json);
            Sender.Tell(new GetResult(json));
        }

        public void HandleGetAll()
        {
            Sender.Tell(new GetAllResult(JArray.FromObject(_deck)));
        }



        protected override void PreStart()
        {
            _deck = new Stack<Card>();
            SetDeck();
            ShuffleDeck();
            
            base.PreStart();
        }
    }
}
