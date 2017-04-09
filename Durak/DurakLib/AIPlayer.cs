using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardLib;

namespace DurakGameLib
{
    class AIPlayer : Player
    {
        #region CLASS MEMBERS

        string AI_PLAYER_NAME = "AI Player";

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Constructor for the AIPlayer object
        /// </summary>
        public AIPlayer()
        {
            new Player(AI_PLAYER_NAME);
        }
        #endregion

        #region INSTANCE MEMBERS
        private Suit trumpCardSuit;
        private Card humanLastCard;

        #endregion

        #region ACCESSORS & MUTATORS

        #endregion

        #region METHODS

        /// <summary>
        /// Getter & Setter from Trump Card Suit
        /// </summary>
        public Suit TrumpCardSuit
        {
            get { return trumpCardSuit; } 
            set { trumpCardSuit = value; } 
        }

        /// <summary>
        /// Getter & Setter for the human's last card
        /// </summary>
        public Card HumanLastCard
        {
            get { return humanLastCard; }

            set { humanLastCard = value; }
        }

        /// <summary>
        /// Returns the minimun card.
        /// </summary>
        /// <param name="card"></param>
        public Card FindMinimumCard(Card card, Card minimumCard)
        {

            if (card < minimumCard)
            {
                minimumCard = card;
            }

            return minimumCard;
        }

        /// <summary>
        /// find eatncard for ai from human last card
        /// </summary>
        /// <param name="card"></param>
        /// <param name="eatnCard"></param>
        /// <returns></returns>
        public Card FindeatnCard(Card card, Card eatnCard)
        {
            if (card.Rank == Rank.Ace)
            {
                eatnCard = card;
            }
            else if (card.Rank == Rank.King)
            {
                eatnCard = card;
            }
            else if (card.Rank == Rank.Queen)
            {
                eatnCard = card;
            }

            return eatnCard;

        }
        /// <summary>
        /// basic logic of ai player
        /// </summary>
        public void BasicAILogic()
        {
            if (IsAttacker == true) // If the AI player is attacking
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

        }

        public void AdvancedAILogic()
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

                Card cardEatn = new Card();
                cardEatn = FindeatnCard(humanLastCard, cardEatn);//find eatn card from human last card.

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
                    if(cardEatn != null)
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
