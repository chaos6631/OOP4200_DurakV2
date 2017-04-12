using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DurakGameLib;
using CardLib;

namespace DurakGuiTester
{
    public partial class frmDurak : Form
    {
        #region FIELDS & PROPS
        private string playerName;   // this should be set in forms constructor, value is passed by frmStartup
        private string opponentName = "CPU";
        private int talonSize;        // this should be set in forms constructor, value is passed by frmStartup
        private DurakGameLib.Game myGame;
        private Player currentPlayer;
        #endregion

        #region ACCESSORS & MUTATORS
        public string PlayerName
        {
            get
            {
                return playerName;
            }

            set
            {
                playerName = value;
            }
        }

        public string OpponentName
        {
            get
            {
                return opponentName;
            }

            set
            {
                opponentName = value;
            }
        }

        public int TalonSize
        {
            get
            {
                return talonSize;
            }

            set
            {
                talonSize = value;
            }
        }

        public Game MyGame
        {
            get
            {
                return myGame;
            }

            set
            {
                myGame = value;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }

            set
            {
                currentPlayer = value;
            }
        }

        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Default constructor
        /// </summary>
        public frmDurak()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="playerName">players name set in frmStartup</param>
        /// <param name="talonSize">deck size set in frmStartup</param>
        public frmDurak(string playerName, int talonSize)
        {
            InitializeComponent();
            this.PlayerName = playerName;
            this.TalonSize = talonSize;
        }
        #endregion

        #region EVENT HANDLERS
        private void frmDurak_Load(object sender, EventArgs e)
        {
            #region Get Player Name/Deck Size
            //THIS IS HANDLED IN CONSTRUCTOR
            //Create frmStartup to retrieve player info
            //Form playerDetails = new frmStartup();
            //playerDetails.ShowDialog();
            
            #endregion

                       
            //THESE ARE HANDLED IN GAME OBJECT
            //Create Player
            //DurakGameLib.Player playerOne = new DurakGameLib.Player(name);
            //Create Opponent
            //DurakGameLib.Player opponent = new DurakGameLib.Player("cpu");

            // CREATE/START GAME
            MyGame = new DurakGameLib.Game(PlayerName, OpponentName);
            CardsPlayed playedCards = new CardsPlayed();
            MyGame.StartGame(TalonSize); // CARDS ARE DEALT AND TRUMPCARD IS DECIDED
            //Determine attacker/defender
            MyGame.IsHumanAttacker();
            //Human player set to attacker for debugging purposes
            MyGame.HumanPlayer.IsAttacker = true;
            MyGame.ComputerPlayer.IsAttacker = false;
            // Set current player
            if (MyGame.HumanPlayer.IsAttacker)
            {
                CurrentPlayer = MyGame.HumanPlayer;
            }
            else
            {
                CurrentPlayer = MyGame.ComputerPlayer;
            }

            #region Populate Starting Display Controls
            // SET NAMES
            lblPlayerName.Text = PlayerName;
            lblOpponentName.Text = OpponentName;
            // UPDATE PLAYER LABELS IN FORM
            if (MyGame.HumanPlayer.IsAttacker)
            {
                lblPlayerRole.Text = "Attacker";
                lblOpponentRole.Text = "Defender";
            }
            else
            {
                lblPlayerRole.Text = "Defender";
                lblOpponentRole.Text = "Attacker";
            }

            //Talon
            cardTopDeck.Visible = true;
            lblCardsRemainingDisplay.Text = MyGame.GameDeck.RemainingCardCount().ToString();
            //Trump
            CardUserControl.CardUserControl trumpCardDisplay = new CardUserControl.CardUserControl();
            pnlDeckArea.Controls.Add(trumpCardDisplay);
            trumpCardDisplay.Location = new Point(10,12);
            trumpCardDisplay.Card = MyGame.GameTrumpCard;
            trumpCardDisplay.Show();
            //MyGame.ComputerPlayer.TrumpCardSuit = trumpCardDisplay.Card.Suit;   // informs computer what trump is for decision making

            // NEED TO POPULATE PLAYERS CARDS IN THE FORM HERE
            //Display hands
            //Player
            for (int index = 0; index < MyGame.HumanPlayer.PlayerHand.Count(); index++)
            {
                CardUserControl.CardUserControl handCard = new CardUserControl.CardUserControl();
                handCard.Card = MyGame.HumanPlayer.PlayerHand.ElementAt(index);
                handCard.FaceUp = true;
                handCard.Click += new EventHandler(handCard_Click);
                pnlPlayer.Controls.Add(handCard);
                handCard.BringToFront();
                handCard.Location = new Point((580 / MyGame.HumanPlayer.PlayerHand.Count()) + (1160 / MyGame.HumanPlayer.PlayerHand.Count() * index), 12);                
            }
            //CPU
            for (int index = 0; index < MyGame.ComputerPlayer.PlayerHand.Count(); index++)
            {
                CardUserControl.CardUserControl handCard = new CardUserControl.CardUserControl();
                handCard.Card = MyGame.ComputerPlayer.PlayerHand.ElementAt(index);
                handCard.FaceUp = true; //Remove after debugging
                handCard.Click += new EventHandler(handCard_Click);
                handCard.Enabled = true;
                pnlOpponent.Controls.Add(handCard);
                handCard.BringToFront();
                handCard.Location = new Point((580 / MyGame.ComputerPlayer.PlayerHand.Count()) + (1160 / MyGame.HumanPlayer.PlayerHand.Count() * index), 12);
            }
            // TRIGGER ATTACKERS CARD SELECTION METHOD
            // If computer is attacker, their method to play a turn should be called here 
            // else players turn will commence
            if(MyGame.ComputerPlayer.IsAttacker)
            {
                //MyGame.ComputerPlayer.BasicAILogic();
                //// Cards should be disabled once AIPlayer working properly          

                ///// TESTING
                MessageBox.Show("Computer is ATTACKER, they play first!!");
            }            
            
            #endregion
        }
        
        /// <summary>
        /// When the number of cards in the deck reaches 0, hide the talon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblCardsRemainingDisplay_TextChanged(object sender, EventArgs e)
        {
            
            //if ()
            //{
                //cardTopDeck.Visible = false;
            //}
            //For testing
            if (lblCardsRemainingDisplay.Text == "0")
            {
                cardTopDeck.Visible = false;
            }
        }
        
        /// <summary>
        ///  Attempt to play a card when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void handCard_Click(object sender, EventArgs e)
        {
            CardUserControl.CardUserControl handCard = sender as CardUserControl.CardUserControl;
            //// Check to see whos playing            
            if (CurrentPlayer.Equals(MyGame.HumanPlayer))
            {
                //// Humans turn
                if (myGame.IsCardPlayable(handCard.Card))
                {
                    //Add to play area
                    pnlPlayArea.Controls.Add(handCard);
                    MyGame.PlayedCards.Push(handCard.Card);
                    MyGame.PlayedCards.HumanLastCardPlayed = handCard.Card;
                    handCard.Location = new Point(90 * (MyGame.PlayedCards.Count - 1) + 10, 12);
                    handCard.Enabled = false;

                    //Remove from hand
                    pnlPlayer.Controls.Remove(handCard);
                    MyGame.HumanPlayer.PlayCard(handCard.Card);

                    // Switch players
                    CurrentPlayer = MyGame.ComputerPlayer;
                }
                else // Card is NOT playable player must choose different card or pass
                {
                    MessageBox.Show("This card is not playable, pleas pick a card with a higher value than last card played!!");
                }
            }
            //// Computers turn
            else
            {
                if (myGame.IsCardPlayable(handCard.Card))
                {
                    //Add to play area
                    pnlPlayArea.Controls.Add(handCard);
                    MyGame.PlayedCards.Push(handCard.Card);
                    MyGame.PlayedCards.ComputerLastCardPlayed = handCard.Card;
                    handCard.Location = new Point(90 * (MyGame.PlayedCards.Count - 1) + 10, 12);
                    handCard.Enabled = false;

                    //Remove from hand
                    pnlOpponent.Controls.Remove(handCard);
                    MyGame.ComputerPlayer.PlayCard(handCard.Card);

                    // Switch players
                    CurrentPlayer = MyGame.HumanPlayer;
                }
                else // Card is NOT playable player must choose different card or pass
                {
                    MessageBox.Show("This card is not playable, pleas pick a card with a higher value than last card played!!");
                }
            }


            // Inform computer what the last card played was
            //MyGame.ComputerPlayer.HumanLastCard = MyGame.PlayedCards.HumanLastCardPlayed;
            // Launch computers turn
            //MyGame.ComputerPlayer.BasicAILogic();
            // At end of computers turn remove the card played by the computer from the computer hand
            // IF computer has passed INFORM user so that can continue or end round
            if (MyGame.ComputerPlayer.IsPassing)
            {
                string infoMessage = "You may continue playing cards until the ACE Trump has been played";
                infoMessage += ",\nor you may end the round by choosing to END your turn.";
                MessageBox.Show(infoMessage, "Your Opponent Has Chosen To PASS!");
            }
            
        }
               
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEndTurn_Click(object sender, EventArgs e)
        {
            //Human or computer fails to defend
            if (MyGame.PlayedCards.Count == 0)
            {
                //A card must be played
                MessageBox.Show("No cards have been played yet, you must play at least 1 card!");
            }
            // CHECK IF COMPUTER HAS PASSED WHICH MEANS PLAYER HAS WON THIS ROUND
            else if (myGame.ComputerPlayer.IsPassing)
            {
                // If the computer has passed that means that the computer has 
                // lost and will be picking up cards

                //Add played cards to hand, call PickUpPlayedCards()
                MyGame.ComputerPlayer.PickUpCards(this.PickUpPlayedCards());
                
            }
            // CHECK IF COMPUTER HASN'T PASSED WHICH MEANS PLAYER HAS LOST THIS ROUND
            else if (!myGame.ComputerPlayer.IsPassing)
            {
                // COMPUTER MAY CONTINUE PLAYING
                while(!myGame.ComputerPlayer.IsPassing)
                {
                    //MyGame.ComputerPlayer.??
                }
                
                //Add played cards to hand

            }

            // Remove the played cards from the GUI
            // TODO...

            //Deal cards to fill remaining spots
            MyGame.DealCards();
            //Clear played cards
            MyGame.PlayedCards.Clear();
            MyGame.PlayedCards.HumanLastCardPlayed = null;
            MyGame.PlayedCards.ComputerLastCardPlayed = null;
        }
        
        #endregion

        #region METHODS
        /// <summary>
        /// Passes all of the played cards to one of the players
        /// </summary>
        /// <returns>all of the cards played</returns>
        public Cards PickUpPlayedCards()
        {
            Cards returnCards = new Cards();
            foreach (Card card in MyGame.PlayedCards)
            {
                returnCards.Add(card);
            }
            return returnCards;
        }

        #endregion
    }
}
