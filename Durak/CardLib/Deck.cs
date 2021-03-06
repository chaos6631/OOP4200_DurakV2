﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardLib
{
    public class Deck : ICloneable
    {
        #region MEMBERS AND PROPERTIES
        protected Cards cards = new Cards();      
        public Cards Cards
        {
            get
            {
                return cards;
            }
            set
            {
                this.cards = value;
            }
        }
        #endregion

        #region EVENT HANDLERS
        public event EventHandler LastCardDrawn;
        #endregion
        
        #region CONSTRUCTORS


        /// <summary>
        /// Default Constructor : Sets deck to the standard 52 cards
        /// </summary>
        public Deck()
        {

            for (int suitVal = 0; suitVal < 4; suitVal++)
            {
                for (int rankVal = 1; rankVal < 14; rankVal++)
                {
                    Cards.Add(new Card((Suit)suitVal, (Rank)rankVal));
                }
            }
        }

        /// <summary>
        /// Constructor : Creates a deck with a specified number of cards 
        /// </summary>
        /// <param name="deckSize">Size of deck</param>
        /// <param name="minRank">Minimum rank in deck</param>
        /// <param name="maxRank">Maximum rank in deck</param>
        public Deck(int deckSize, int minRank, int maxRank)
        {

        }

        /// <summary>
        /// Constructor used for cloning
        /// </summary>
        /// <param name="newCards"></param>
        private Deck(Cards newCards)
        {
            cards = newCards;
        }
       

        /// <summary>
        /// Nondefault constructor. Allows aces to be set high.
        /// </summary>
        public Deck(bool isAceHigh) : this()
        {
            Card.isAceHigh = isAceHigh;
        }

        /// <summary>
        /// Nondefault constructor. Allows a trump suit to be used.
        /// </summary>
        public Deck(bool useTrumps, Suit trump) : this()
        {
            Card.useTrumps = useTrumps;
            Card.trump = trump;
        }
        
        /// <summary>
        /// Nondefault constructor. Allows aces to be set high and a trump suit
        /// to be used.
        /// </summary>
        public Deck(bool isAceHigh, bool useTrumps, Suit trump) : this()
        {
            Card.isAceHigh = isAceHigh;
            Card.useTrumps = useTrumps;
            Card.trump = trump;
        }
        #endregion

        #region METHODS
        /// <summary>
        /// Clones an existing set of cards 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Deck newDeck = new Deck(Cards.Clone() as Cards);
            return newDeck;
        }

        /// <summary>
        /// GetCard()
        /// </summary>
        /// <returns>The card off the top of the stack</returns>
        public Card GetCard()
        {
            int cardCount = Cards.Count;
            Card returnCard = this.GetCard(cardCount - 1);            
            this.Cards.RemoveAt(cardCount - 1);
            return returnCard;
        }

        /// <summary>
        /// GetCard(cardNum)
        /// </summary>
        /// <param name="cardNum">Index of the card wanted</param>
        /// <returns>The card at the passed index</returns>
        protected Card GetCard(int cardNum)
        {
            if (cardNum >= 0 && cardNum <= 51)
            {
                if ((cardNum == 0) && (LastCardDrawn != null))
                    LastCardDrawn(this, EventArgs.Empty);
                return Cards[cardNum];
            }
            else
            {
                throw new CardOutOfRangeException(Cards.Clone() as Cards);
            }      
        }

        /// <summary>
        /// Shuffles the card objects in the deck
        /// </summary>
        public void Shuffle(int deckSize)
        {
            Cards newDeck = new Cards();
            bool[] assigned = new bool[deckSize];
            Random sourceGen = new Random();
            for (int i = 0; i < deckSize; i++)
            {
                int sourceCard = 0;
                bool foundCard = false;
                while (foundCard == false)
                {
                    sourceCard = sourceGen.Next(deckSize);
                    if (assigned[sourceCard] == false)
                        foundCard = true;
                }
                assigned[sourceCard] = true;
                newDeck.Add(Cards[sourceCard]);
            }
            newDeck.CopyTo(Cards);
        }
        #endregion
    }
}
