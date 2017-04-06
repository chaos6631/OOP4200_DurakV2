

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ch13CardLib;

namespace DurakGameLib
{
    class Game
    {
        #region CLASS MEMBERS
        // Max players
        // Min players
        static int[] DECK_SIZES = { 20, 36, 52 };        // Deck sizes
        static int INITIAL_PLAYER_CARD_COUNT = 6;       // Initial player card amount
        #endregion

        #region CONSTRUCTORS
        public Game()
        {
            //this.players = new Dictionary<string, Player>();

        }

        #endregion

        #region INSTANCE MEMBERS
        //private Dictionary<string, Player> players;
        private Player humanPlayer;
        private Player computerPlayer;
        private Talon gameDeck;
        private Card gameTrumpCard;

        #endregion

        #region ACCESSORS & MUTATORS
        /// <summary>
        /// Public property for humanPlayer
        /// </summary>
        public Player HumanPlayer
        {
            get
            {
                return humanPlayer;
            }

            set
            {
                humanPlayer = value;
            }
        }

        /// <summary>
        /// Public property for computerPlayer
        /// </summary>   
        public Player ComputerPlayer
        {
            get
            {
                return computerPlayer;
            }

            set
            {
                computerPlayer = value;
            }
        }

        /// <summary>
        /// Public property for gameTrumpCard
        /// </summary>
        public Card GameTrumpCard
        {
            get
            {
                return gameTrumpCard;
            }

            set
            {
                gameTrumpCard = value;
            }
        }

        #endregion

        #region EVENTS, EVENT HANDLERS, DELEGATES
        public delegate void HumanPlayerTurn();
        public event HumanPlayerTurn PlayHumanTurn;


        #endregion


        #region METHODS
        /// <summary>
        /// Commence the next round of the current game
        ///  - Should be called when PlayRound_event
        /// </summary>
        public void PlayNextRound()
        {
            
            //// Decide which player goes first
            if (IsHumanAttacker())
            {
                //// human is attacker
                //// trigger human players select card event function
               
            }
            else
            {
                //// copmuter is attacker
                //// trigger computer players select cards event function
                
            }            
        }

        /// <summary>
        /// 
        ///
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        public bool PlayerTurn(Player attacker, Player defender)
        {
            bool played = false;
            // select card to play
            attacker.PlayCard(attacker.PlayerHand.ElementAt(0));
            // play card, set played to true
            // 
            return played;
        }

        /// <summary>
        /// Start the current game, string should be validated in the GUI
        /// </summary>
        public void StartGame(string playerName)
        {
            //// Initialize Players
            try
            {            
                //// Get choice of deck size from user
                int deckSize = 36;              // should come from event

                //// Initialize Deck and Deal cards
                this.gameDeck = new Talon();

                //// Set deck to required size                
                this.gameDeck.SetDeckSize(deckSize);

                //// Deal cards to players
                DealCards();
                //// Set trump card
                gameTrumpCard = gameDeck.GetCard();
            }
            catch (Exception)
            {
                throw;
            }            
        }

        /// <summary>
        /// End the current game
        /// </summary>
        public void EndGame() { }
        
        /// <summary>
        /// IsHumanAttacker
        /// - First checks to see if it is initial round and then decides who attacker is
        ///   based on lowest trump card
        /// - If it isn't first round the attacker is decided based on who was attacker last
        /// </summary>
        /// <returns>True if human is attacker, false if not</returns>
        public bool IsHumanAttacker()
        {        
            //// Check if it is the first deal of the game    
            if (humanPlayer.IsAttacker == false && computerPlayer.IsAttacker == false)
            {
                //// Default if neither player has any trump
                humanPlayer.IsAttacker = true;          
                //// Player with lowest trump is the attacker                
                foreach (Card humanCard in humanPlayer.PlayerHand)
                {
                    if (humanCard.Suit == gameTrumpCard.Suit)
                    {
                        foreach (Card computerCard in computerPlayer.PlayerHand)
                        {
                            if (computerCard.Suit == gameTrumpCard.Suit
                                && computerCard.Rank < humanCard.Rank)
                            {
                                // Human has higher trump card than computer, computer is attacker
                                computerPlayer.IsAttacker = true;
                                HumanPlayer.IsAttacker = false;
                            }
                            else if(computerCard.Suit == gameTrumpCard.Suit
                                && computerCard.Rank > humanCard.Rank)
                            {
                                // copmuter has higher trump card than human, human is attacker
                                HumanPlayer.IsAttacker = true;
                                computerPlayer.IsAttacker = false;
                            }
                        }
                    }
                }
            }
            else if (computerPlayer.IsAttacker == true)
            {
                HumanPlayer.IsAttacker = true;                  // human plays as attacker
                computerPlayer.IsAttacker = false;
            }    
            else
            {
                HumanPlayer.IsAttacker = false;                 // computer plays as attacker
                computerPlayer.IsAttacker = true;
            }
            

            return HumanPlayer.IsAttacker;
        }
        
        /// <summary>
        /// Deals cards based on wether it is INITIAL DEAL or who is ATTACKER
        /// </summary>
        public void DealCards()
        {
            //// Check if it is initial deal
            if (HumanPlayer.PlayerHand.Count == 0 && ComputerPlayer.PlayerHand.Count == 0)
            {
                for (int i = 1; i <= INITIAL_PLAYER_CARD_COUNT; i++)
                {
                    humanPlayer.TakeFromDeck(gameDeck.GetCard());
                    computerPlayer.TakeFromDeck(gameDeck.GetCard());
                }
            }
            else
            {                         
                //// Deal to attacker first
                if (HumanPlayer.IsAttacker)
                    for (int i = ComputerPlayer.PlayerHand.Count; i <= INITIAL_PLAYER_CARD_COUNT; i++)
                    {
                        ComputerPlayer.TakeFromDeck(gameDeck.GetCard());
                    }
                else
                    for (int i = HumanPlayer.PlayerHand.Count; i <= INITIAL_PLAYER_CARD_COUNT; i++)
                    {
                        HumanPlayer.TakeFromDeck(gameDeck.GetCard());
                    }
            }
        }
        
        
        
        
        #endregion
    }
}
