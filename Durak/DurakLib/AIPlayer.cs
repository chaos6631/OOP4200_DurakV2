﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardLib;

namespace DurakGameLib
{
    public class AIPlayer : Player
    {
        #region CLASS MEMBERS

        static string AI_PLAYER_NAME = "AI Player";

        #endregion

        #region INSTANCE MEMBERS
        private Suit trumpCardSuit;

        #endregion
        
        #region CONSTRUCTORS

        /// <summary>
        /// Constructor for the AIPlayer object
        /// </summary>
        public AIPlayer() : base(AI_PLAYER_NAME)
        {
            //new Player(AI_PLAYER_NAME);   // not correct way to call base constructor
        }
        #endregion

        #region ACCESSORS & MUTATORS

        /// <summary>
        /// Getter & Setter from Trump Card Suit
        /// </summary>
        public Suit TrumpCardSuit
        {
            get { return trumpCardSuit; }
            set { trumpCardSuit = value; }
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Returns the minimun card.
        /// </summary>
        /// <param name="card"></param>
        public Card FindMinimumCard(Card card, Card minimumCard)
        {

            if (card < minimumCard && card.Suit == minimumCard.Suit)
            {
                minimumCard = card;
            }

            return minimumCard;
        }

        /// <summary>
        /// find highest for ai from human last card
        /// this is what this method should find, change needs to be made
        /// </summary>
        /// <param name="card"></param>
        /// <param name="eatnCard"></param>
        /// <returns></returns>
        public bool FindHighestCard(Card card, Card eatnCard)
        {
            bool pickUp = false;

            if (card.Rank == Rank.Ace)
            {
                eatnCard = card;
                pickUp = true;
            }
            else if (card.Rank == Rank.King)
            {
                eatnCard = card;
                pickUp = true;
            }
            else if (card.Rank == Rank.Queen)
            {
                eatnCard = card;
                pickUp = true;
            }

            return pickUp;

        }
        /// <summary>
        /// basic logic of ai player
        /// </summary>
        public Card BasicAILogic(Card humanLastCard)
        {
            
            Card minimumCard = new Card();
            Card cardToPlay = new Card();
            if (IsAttacker == true) // If the AI player is attacking
            {                
                bool isFirstLoop = true;
                
                foreach (Card aicard in PlayerHand) // Loop through the AI Player's Hand
                {
                    if (isFirstLoop)
                    {
                        minimumCard = aicard;
                        isFirstLoop = false;
                    }
                   
                    minimumCard = FindMinimumCard(aicard, minimumCard); // find the minimum card
                }

                PlayCard(minimumCard); // Play the minimum card
                cardToPlay = minimumCard;
            }
            else
            {
                Boolean aiLoses = true;
                foreach (Card aicard in PlayerHand)
                {
                    cardToPlay = aicard;

                    if (aiLoses)
                    {
                        if (humanLastCard <= aicard)
                        {
                            aiLoses = false;
                        }

                        if (humanLastCard < aicard && humanLastCard.Suit == aicard.Suit)
                        {
                            aiLoses = false;
                        }

                        else if (humanLastCard < aicard && aicard.Suit == trumpCardSuit)
                        {
                            aiLoses = false;
                        }
                    }

                }

                if(aiLoses)
                {
                    TakeFromDeck(humanLastCard);
                } else
                {
                    PlayCard(cardToPlay);
                }

            }
            return cardToPlay;
        }

        public void AdvancedAILogic(Card humanLastCard)
        {
            if (IsAttacker == true)
            {
                Card minimumCard = new Card();
                bool isFirstLoop = true;
                foreach (Card aicard in PlayerHand) // Loop through the AI Player's Hand
                {
                    if (isFirstLoop)
                    {
                        minimumCard = aicard;
                        isFirstLoop = false;
                    }

                    minimumCard = FindMinimumCard(aicard, minimumCard); // find the minimum card
                }

                PlayCard(minimumCard); // Play the minimum card
            }
            else
            {
                Boolean aiLoses = true;
                Card cardToPlay = new Card();

                bool cardEatn ;
                cardEatn = FindHighestCard(humanLastCard, cardToPlay);//find highest card from human last card.

                foreach (Card aicard in PlayerHand)
                {
                    cardToPlay = aicard;

                    if (aiLoses)
                    {
                        if (humanLastCard <= aicard)
                        {
                            aiLoses = false;
                        }

                        if (humanLastCard < aicard && humanLastCard.Suit == aicard.Suit)
                        {
                            aiLoses = false;
                        }

                        else if (humanLastCard < aicard && aicard.Suit == trumpCardSuit)
                        {
                            aiLoses = false;
                        }
                    }

                }

                if (aiLoses)
                {
                    TakeFromDeck(humanLastCard);
                }
                else
                {   
                    if(cardEatn == true)
                    {
                        TakeFromDeck(humanLastCard);
                    }
                    else { 
                    PlayCard(cardToPlay);
                    }
                }

            }
        }
                

        }
        #endregion
    }
