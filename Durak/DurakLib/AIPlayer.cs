using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ch13CardLib;

namespace DurakGameLib
{
    class AIPlayer : Game
    {
        #region CLASS MEMBERS




        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Constructor for the AIPlayer object
        /// </summary>
        public AIPlayer()
        {

            aiplayhand = new Cards();   // The ia player cards as a hand


        }
        #endregion



        #region INSTANCE MEMBERS
        private Cards aiplayhand;
        private Card humanLastCard;
        private bool isAttacker;    // Is the   attacker
        private Suit superSuit;
        #endregion

        #region ACCESSORS & MUTATORS
        /// <summary>
        /// Getter & Setter for AIplayhand
        /// </summary>
        public Cards AIplayhand
        {
            get
            {
                return aiplayhand; // Returns the ai hand of cards
            }
            set
            {
                aiplayhand = value; // Sets the ai hand of cards
            }
        }

        /// <summary>
        /// Getter & Setter for   Isattacker 
        /// </summary>
        public bool IsAttacker
        {
            get
            {
                return isAttacker;
            }
            set
            {
                isAttacker = value;
            }
        }

        /// <summary>
        /// Getter & Setter for HumanLastCard 
        /// </summary>
        public Card HumanLastCard
        {
            get
            {
                return humanLastCard;
            }
            set
            {
                humanLastCard = value;
            }
        }

        public Suit SuperSuit
        {
            get
            {
                return superSuit;
            }
            set
            {
                superSuit = value;
            }
        }

        #endregion

        #region METHODS
        /// <summary>
        /// remove card from ai player's hand
        /// </summary>
        /// <param name="card"></param>
        public void AiPlayCard(Card card)
        {
            aiplayhand.Remove(card); // Removes the card played from the ai hand
        }

        /// <summary>
        /// Draw cards to the ai player's hand
        /// </summary>
        /// <param name="card"></param>
        public void DrawCard(Card card)
        {
            aiplayhand.Add(card); // Add each to the players hand           
        }

        /// <summary>
        /// Play the minimun card.
        /// </summary>
        /// <param name="card"></param>
        public void PlayMinimumCard(Card card)
        {
            if (card.Rank == Rank.Two)
            {
                AiPlayCard(card);
            }
            else if (card.Rank == Rank.Three)
            {
                AiPlayCard(card);
            }
            else if (card.Rank == Rank.Four)
            {
                AiPlayCard(card);
            }
            else if (card.Rank == Rank.Five)
            {
                AiPlayCard(card);
            }
            else if (card.Rank == Rank.Six)
            {
                AiPlayCard(card);
            }
            else if (card.Rank == Rank.Seven)
            {
                AiPlayCard(card);
            }
            else if (card.Rank == Rank.Eight)
            {
                AiPlayCard(card);
            }
            else if (card.Rank == Rank.Nine)
            {
                AiPlayCard(card);
            }
            else if (card.Rank == Rank.Ten)
            {
                AiPlayCard(card);
            }
            else if (card.Rank == Rank.Jack)
            {
                AiPlayCard(card);
            }
            else if (card.Rank == Rank.Queen)
            {
                AiPlayCard(card);
            }
            else if (card.Rank == Rank.King)
            {
                AiPlayCard(card);
            }
            else if (card.Rank == Rank.Ace)
            {
                AiPlayCard(card);
            }
        }


        public void AdvancedEatCard(Card card)
        {
            if (card.Rank == Rank.Ace)
            {
                DrawCard(card);
            }
            else if (card.Rank == Rank.King)
            {
                DrawCard(card);
            }
            else if (card.Rank == Rank.Queen)
            {
                DrawCard(card);
            }
        }
        /// <summary>
        /// basic logic of ai player
        /// </summary>
        public void BasicAILogic(Card humanLastCard)
        {
            if (IsAttacker == true)
            {
                foreach (Card aicard in AIplayhand)
                {
                    PlayMinimumCard(aicard);
                }
            }
            else
            {
                foreach (Card aicard in AIplayhand)
                {
                    if (humanLastCard >= aicard)
                    {
                        DrawCard(humanLastCard);
                    }

                    if (humanLastCard < aicard || humanLastCard.Suit == aicard.Suit)
                    {

                        AiPlayCard(aicard);
                    }

                    if (humanLastCard < aicard || aicard.Suit == superSuit)
                    {
                        AiPlayCard(aicard);
                    }

                }

            }

        }


        public void AdvancedAILogic(Card humanLastCard)
        {
            if (IsAttacker == true)
            {
                foreach (Card aicard in AIplayhand)
                {
                    PlayMinimumCard(aicard);
                }
            }
            else
            {
                foreach (Card aicard in AIplayhand)
                {
                    if (humanLastCard >= aicard)
                    {
                        DrawCard(humanLastCard);
                    }

                    if (humanLastCard < aicard || humanLastCard.Suit == aicard.Suit)
                    {
                        AdvancedEatCard(humanLastCard);
                        AiPlayCard(aicard);
                    }

                    if (humanLastCard < aicard || aicard.Suit == superSuit)
                    {
                        AdvancedEatCard(humanLastCard);
                        AiPlayCard(aicard);
                    }

                }

            }

        }
        #endregion
    }
}
