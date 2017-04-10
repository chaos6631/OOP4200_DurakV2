

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardLib;

namespace DurakGameLib
{
    public class Game
    {
        #region CLASS MEMBERS
        // Max players
        // Min players
        static int[] DECK_SIZES = { 20, 36, 52 };        // Deck sizes
        static int INITIAL_PLAYER_CARD_COUNT = 6;       // Initial player card amount
        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Default
        /// </summary>
        public Game()
        {
            //this.players = new Dictionary<string, Player>();

        }
        /// <summary>
        /// Paramaterized
        /// </summary>
        /// <param name="humanName"></param>
        /// <param name="opponentName"></param>
        public Game(string humanName, string computerName)
        {
            this.humanPlayer = new Player(humanName);
            //this.HumanPlayer.PlayerName = humanName;
            this.computerPlayer = new Player(computerName); // NEEDS TO BE CHNAGED TO AIPLAYER WHEN CONSTRUCTOR IS CREATED
            //this.ComputerPlayer.PlayerName = computerName;
            this.playedCards = new CardsPlayed();
        }
        #endregion

        #region INSTANCE MEMBERS
        //private Dictionary<string, Player> players;
        private Player humanPlayer;
        private Player computerPlayer;
        private Talon gameDeck;
        private Card gameTrumpCard;
        private CardsPlayed playedCards;
        private bool continueGame;
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

        public Talon GameDeck
        {
            get
            {
                return gameDeck;
            }
            set
            {
                gameDeck = value;
            }
        }

        public bool ContinueGame
        {
            get
            {
                return continueGame;
            }

            set
            {
                continueGame = value;
            }
        }

        public CardsPlayed PlayedCards
        {
            get
            {
                return playedCards;
            }

            set
            {
                playedCards = value;
            }
        }

        #endregion

        #region EVENTS, EVENT HANDLERS, DELEGATES
        public delegate void HumanPlayerTurn();
        public event HumanPlayerTurn PlayHumanTurn;
        public delegate void PlayerHasGoneOut();
        public event PlayerHasGoneOut PlayerWin;


        #endregion
        
        #region METHODS
        /// <summary>
        /// Commence the next round of the current game
        ///  - Should be called when PlayRound_event
        /// </summary>
        public void PlayNextRound()
        {
            bool roundContinue = true;

            while(roundContinue)
            {
                //// Decide which player goes first
                if (IsHumanAttacker())
                {
                    //// human is attacker     
                    if (HumanPlayer.IsPlayable)
                    {
                        if (!PlayerTurn(HumanPlayer))
                        {
                            roundContinue = false;
                        }
                    }

                }
                else
                {
                    //// copmuter is attacker   
                    if (ComputerPlayer.IsPlayable)
                    {
                        if (!PlayerTurn(ComputerPlayer))
                        {
                            roundContinue = false;
                        }
                    }

                }

                //// Check if deck is empty and if a player is out of cards
                if (GameDeck.RemainingCardCount() == 0)
                {
                    if (HumanPlayer.PlayerHand.Count == 0 || ComputerPlayer.PlayerHand.Count == 0)
                    {
                        roundContinue = false;
                    }
                }
            }                        
        }

        /// <summary>
        /// Calls actions related to a players turn
        ///
        /// </summary>
        /// <param name="attacker">The player who is an attacker</param>
        public bool PlayerTurn(Player attacker)
        {
            bool played = false;
            int cardIndex = 0;      // equals selected card
                                    //// trigger human players select card event function
            if (attacker.Equals(ComputerPlayer))
            {
                //Do AI TURN PROCESSES
                played = true;
            }
            else
            {

            }
            //if (answer == "n")
            //{
            //    Console.WriteLine("Which card do you wish to play?");
            //    cardIndex = Console.Read();
            //    // selected card to play is set to card index
            //    if (IsCardPlayable(attacker.PlayerHand.ElementAt(cardIndex)))
            //    {
            //        attacker.PlayCard(attacker.PlayerHand.ElementAt(cardIndex));
            //        played = true;
            //    }
            //}
            
            
            
            // PLAYCARD NEEDS TO BE CHANGED TO RETURN THE CARD in the player class
            // play card, set played to true
            // 
            return played;
        }

        /// <summary>
        /// Checks to see if the players card can actually be played
        /// </summary>
        /// <param name="playerCard">The card being checked against the last card played by the other player</param>
        /// <returns>True if card can be played, false if not</returns>
        public bool IsCardPlayable(Card playerCard)
        {
            bool isPlayable = false;
            
            //// Is card Trump, if so are both cards trump cards
            if (playerCard.Suit == GameTrumpCard.Suit)
            {
                if((playerCard.Suit == GameTrumpCard.Suit &&
                PlayedCards.Last().Suit == GameTrumpCard.Suit))
                {
                    if (playerCard.Rank > PlayedCards.Last().Rank)
                    {
                        isPlayable = true;
                    }
                }
                else
                {
                    isPlayable = true;
                }
            }
            else if (playedCards.Count == 0)
            {
                isPlayable = true;
            }
            //// Is card of higher value than other players card
            else if (playerCard.Rank > PlayedCards.Last().Rank)
            {
                isPlayable = true;
            }
            
            return isPlayable;
        }

        /// <summary>
        /// Start the current game, string should be validated in the GUI
        /// </summary>
        public void StartGame(int deckSize)
        {
            //// Initialize Players in constructor
            try
            {
                //// Get choice of deck size from user
                //int deckSize = 36;              // should come from event
                continueGame = true;
                //// Initialize Deck and Deal cards
                this.GameDeck = new Talon();

                //// Set deck to required size                
                this.GameDeck.SetDeckSize(deckSize);

                //// Deal cards to players
                DealCards();
                //// Set trump card
                GameTrumpCard = GameDeck.GetCard();
                GameTrumpCard.FaceUp = true;
            }
            catch (Exception)
            {
                throw;
            }            
        }

        /// <summary>
        /// End the current game
        /// </summary>
        public void EndGame()
        {

        }
        
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
                    humanPlayer.TakeFromDeck(GameDeck.GetCard());

                    computerPlayer.TakeFromDeck(GameDeck.GetCard());
                }
            }
            else
            {                         
                //// Deal to attacker first
                if (HumanPlayer.IsAttacker)
                    for (int i = ComputerPlayer.PlayerHand.Count; i <= INITIAL_PLAYER_CARD_COUNT; i++)
                    {
                        ComputerPlayer.TakeFromDeck(GameDeck.GetCard());
                    }
                else
                    for (int i = HumanPlayer.PlayerHand.Count; i <= INITIAL_PLAYER_CARD_COUNT; i++)
                    {
                        HumanPlayer.TakeFromDeck(GameDeck.GetCard());
                    }
            }
        }
        
        
        
        
        #endregion
    }
}
