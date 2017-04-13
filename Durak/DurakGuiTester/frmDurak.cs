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
        private bool trumpTaken = false; //flag tripped if the trump card is drawn as the last card
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
            // CREATE/START GAME
            MyGame = new DurakGameLib.Game(PlayerName, OpponentName);
            CardsPlayed playedCards = new CardsPlayed();
            // CARDS ARE DEALT AND TRUMPCARD IS DECIDED
            MyGame.StartGame(TalonSize);
            //Determine attacker/defender
            MyGame.IsHumanAttacker();
            //Human player set to attacker for debugging purposes
            MyGame.HumanPlayer.IsAttacker = true;
            MyGame.ComputerPlayer.IsAttacker = false;
            // Set current player
            //if (MyGame.HumanPlayer.IsAttacker)
            //{
            //    CurrentPlayer = MyGame.HumanPlayer;
            //}
            //else
            //{
            //    CurrentPlayer = MyGame.ComputerPlayer;
            //}

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
            trumpCardDisplay.Location = new Point(10, 12);
            trumpCardDisplay.Card = MyGame.GameTrumpCard;
            trumpCardDisplay.Show();

            // INFORM AI OF THE TRUMP SUIT
            //MyGame.ComputerPlayer.TrumpCardSuit = MyGame.GameTrumpCard.Suit;

            // NEED TO POPULATE PLAYERS CARDS IN THE FORM HERE
            this.CreatePlayerCardsGUI();

            // TRIGGER ATTACKERS CARD SELECTION METHOD
            // If computer is attacker, their method to play a turn should be called here 
            // else players turn will commence
            if (MyGame.ComputerPlayer.IsAttacker)
            {
                ///// TESTING
                MessageBox.Show("Computer is ATTACKER, they play first!!");


                //MyGame.ComputerPlayer.BasicAILogic();
                //// Cards should be disabled once AIPlayer working properly 
                AITurn();
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
            bool turnFinished = false;

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
                //CurrentPlayer = MyGame.ComputerPlayer;
                turnFinished = true;
            }
            else // Card is NOT playable player must choose different card or pass
            {
                MessageBox.Show("This card is not playable, please pick a card with a higher value than last card played!!");
            }

            if (turnFinished)
            {
                //// Computers turn
                AITurn();
            }
           


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEndTurn_Click(object sender, EventArgs e)
        {
            ////// TERSTING PURPOSES WITHOUT AIPLAYER
            //if (MessageBox.Show("Who is ending turn?", "Click YES for human, NO for cpu", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //{
            //    // Player is passing
            //    MyGame.HumanPlayer.IsPassing = true;
            //}
            //else
            //{
            //    // Computer is passing
            //    MyGame.ComputerPlayer.IsPassing = true;
            //}
            //Human or computer fails to defend
            if (MyGame.PlayedCards.Count == 0)
            {
                //A card must be played
                MessageBox.Show("No cards have been played yet, you must play at least 1 card!");
            }
            else
            {
                // CHECK IF COMPUTER HAS PASSED WHICH MEANS PLAYER HAS WON THIS ROUND
                if (myGame.ComputerPlayer.IsPassing)
                {
                    MessageBox.Show("You have won this round!!");
                    if (MyGame.ComputerPlayer.IsAttacker == false)
                    {
                        //Add played cards to hand, call PickUpPlayedCards()
                        MyGame.ComputerPlayer.PickUpCards(this.PickUpPlayedCards());
                    }
                    this.EndRound(myGame.HumanPlayer, myGame.ComputerPlayer);
                }
                // CHECK IF HUMAN HAS PASSED WHICH MEANS COMPUTER HAS WON THIS ROUND
                else
                {
                    MessageBox.Show("You lost this round!!");
                    if (MyGame.HumanPlayer.IsAttacker == false)
                    {
                        //Add played cards to hand, call PickUpPlayedCards()
                        MyGame.HumanPlayer.PickUpCards(this.PickUpPlayedCards());
                    }
                    this.EndRound(myGame.ComputerPlayer, myGame.HumanPlayer);
                }
                #region TO BE USED WHEN AI WORKING
                // CHECK IF COMPUTER HASN'T PASSED WHICH MEANS PLAYER HAS LOST THIS ROUND
                //else if (!MyGame.ComputerPlayer.IsPassing)
                //{                   

                //// COMPUTER MAY CONTINUE PLAYING
                //while(!MyGame.ComputerPlayer.IsPassing)
                //{
                //    //MyGame.ComputerPlayer.??
                //    CardUserControl.CardUserControl guiCard = new CardUserControl.CardUserControl();
                //    Card cardToPlay = new Card();  //MyGame.ComputerPlayer.BasicLogic();
                //    if (MyGame.IsCardPlayable(cardToPlay))
                //    {
                //        //// REPEATED CODE FROM CLICK COULD BE TURNED INTO A REUSABLE METHOD
                //        //Add to play area
                //        pnlPlayArea.Controls.Add(guiCard);
                //        MyGame.PlayedCards.Push(cardToPlay);
                //        MyGame.PlayedCards.ComputerLastCardPlayed = cardToPlay;
                //        guiCard.Location = new Point(90 * (MyGame.PlayedCards.Count - 1) + 10, 12);
                //        guiCard.Enabled = false;

                //        //Remove from hand
                //        pnlOpponent.Controls.Remove(guiCard);
                //        MyGame.ComputerPlayer.PlayCard(cardToPlay);
                //    }

                //    // isPassing SHOULD BE SET INSIDE THE AIPLAYER TURN LOGIC
                //}

                //Add played cards to hand

                //}
                #endregion
            }

            //MessageBox.Show("We will now clear the cards");
            //// Remove the played cards from the GUI
            //// TODO...
            //pnlPlayArea.Controls.Clear();

            //// Remove the played cards from the GAME
            //MyGame.PlayedCards.Clear();
            //MyGame.PlayedCards.HumanLastCardPlayed = null;
            //MyGame.PlayedCards.ComputerLastCardPlayed = null;

            ////Deal cards to fill remaining spots
            //MyGame.DealCards();

            //// CLEAR PLAYER PANELS TO DO A REFRESH WITH THE NEW CARDS, JUST EASIER THAN TRYING TO UPDATE
            //pnlOpponent.Controls.Clear();
            //pnlPlayer.Controls.Clear();
            //// Add the newly dealt cards to the player panels
            //this.CreatePlayerCardsGUI();
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

        /// <summary>
        /// AI Players turn
        /// </summary>
        public void AITurn()
        {
            
            MyGame.ComputerPlayer.IsPassing = true;
            // FIND OUT IF AI IS PASSING BEFORE PROCEDDING WITH TURN
            foreach (Card checkCard in MyGame.ComputerPlayer.PlayerHand)
            {
                if (myGame.IsCardPlayable(checkCard))
                {
                    // Set passing to false because a playable card has been found
                    MyGame.ComputerPlayer.IsPassing = false;
                }
            }
            // IF THEY ARE NOT PASSING PLAY A CARD
            if (!MyGame.ComputerPlayer.IsPassing)
            {
                // Find out what card will be played by AI
                Card cardplayed = MyGame.ComputerPlayer.BasicAILogic(MyGame.PlayedCards.HumanLastCardPlayed);
                
                // CREATE THE CARD CONTROL TO BE INSERTED INTO THE PLAYED CARDS PANEL
                //CardUserControl.CardUserControl cardPlayedCtrl = new CardUserControl.CardUserControl(cardplayed);
                //cardPlayedCtrl.Card.FaceUp = true;
                //int index = pnlOpponent.Controls.IndexOf(cardPlayedCtrl);
                int index = -1;
                for (int i = 0; i < pnlOpponent.Controls.Count && index == -1; i++)
                {
                    CardUserControl.CardUserControl c = pnlOpponent.Controls[i] as CardUserControl.CardUserControl;
                    if (cardplayed == c.Card)
                    {
                        index = i;
                    }
                }

                CardUserControl.CardUserControl cardPlayedCtrl = pnlOpponent.Controls[index] as CardUserControl.CardUserControl;
                if (myGame.IsCardPlayable(cardplayed))
                {
                    //Add to play area
                    //cardPlayedCtrl.Card = cardplayed;
                    pnlPlayArea.Controls.Add(cardPlayedCtrl);
                    MyGame.PlayedCards.Push(cardplayed);
                    MyGame.PlayedCards.ComputerLastCardPlayed = cardplayed;
                    cardPlayedCtrl.Location = new Point(90 * (MyGame.PlayedCards.Count - 1) + 10, 12);
                    cardPlayedCtrl.Enabled = false;

                    //Remove from hand
                    pnlOpponent.Controls.Remove(cardPlayedCtrl);
                    //pnlOpponent.Controls.RemoveAt(index);
                    MyGame.ComputerPlayer.PlayCard(cardplayed);

                    // Switch players
                    //CurrentPlayer = MyGame.HumanPlayer;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreatePlayerCardsGUI()
        {
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
                //Refresh role
                handCard.Location = new Point((580 / MyGame.HumanPlayer.PlayerHand.Count()) + (1160 / MyGame.HumanPlayer.PlayerHand.Count() * index), 12);
                if (MyGame.HumanPlayer.IsAttacker)
                {
                    lblPlayerRole.Text = "Attacker";
                    //CurrentPlayer = MyGame.HumanPlayer;
                }
                else
                {
                    lblPlayerRole.Text = "Defender";
                }
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
                handCard.Location = new Point((580 / MyGame.ComputerPlayer.PlayerHand.Count()) + (1160 / MyGame.ComputerPlayer.PlayerHand.Count() * index), 12);
                if (MyGame.ComputerPlayer.IsAttacker)
                {
                    lblOpponentRole.Text = "Attacker";
                    //CurrentPlayer = MyGame.ComputerPlayer;
                }
                else
                {
                    lblOpponentRole.Text = "Defender";
                }
            }
        }


        public void EndRound(Player winner, Player loser)
        {
            MessageBox.Show("We will now clear the cards");
            // Remove the played cards from the GUI
            // TODO...
            pnlPlayArea.Controls.Clear();
            //Remove passing status
            MyGame.HumanPlayer.IsPassing = false;
            MyGame.ComputerPlayer.IsPassing = false;

            // Remove the played cards from the GAME
            MyGame.PlayedCards.Clear();
            MyGame.PlayedCards.HumanLastCardPlayed = null;
            MyGame.PlayedCards.ComputerLastCardPlayed = null;

            //Deal cards to fill remaining spots
            MyGame.DealCards();
            lblCardsRemainingDisplay.Text = MyGame.GameDeck.RemainingCardCount().ToString();

            //Draw trump if talon is empty
            if (MyGame.GameDeck.RemainingCardCount() == 0 && MyGame.HumanPlayer.PlayerHand.Count() < 6 && trumpTaken == false)//replace with variable
            {
                MyGame.HumanPlayer.TakeFromDeck(MyGame.GameTrumpCard);
                trumpTaken = true;
            }
            else if (MyGame.GameDeck.RemainingCardCount() == 0 && MyGame.ComputerPlayer.PlayerHand.Count() < 6 && trumpTaken == false)//replace with variable
            {
                MyGame.ComputerPlayer.TakeFromDeck(MyGame.GameTrumpCard);
                trumpTaken = true;
            }

            //SWAP PLAYER ROLES IF DEFENSE WINS
            winner.IsAttacker = true;
            loser.IsAttacker = false;

            // CLEAR PLAYER PANELS TO DO A REFRESH WITH THE NEW CARDS, JUST EASIER THAN TRYING TO UPDATE
            pnlOpponent.Controls.Clear();
            pnlPlayer.Controls.Clear();
            // Add the newly dealt cards to the player panels
            this.CreatePlayerCardsGUI();

            //Check if game is over
            if (trumpTaken == true && MyGame.HumanPlayer.PlayerHand.Count == 0)
            {
                MyGame.ComputerPlayer.IsDurak = true;
                MessageBox.Show(MyGame.ComputerPlayer.PlayerName.ToString() + " is the Durak");
                MyGame.EndGame();
            }
            else if (trumpTaken == true && MyGame.ComputerPlayer.PlayerHand.Count == 0)
            {
                MyGame.HumanPlayer.IsDurak = true;
                MessageBox.Show(MyGame.HumanPlayer.PlayerName.ToString() + " is the Durak");
                MyGame.EndGame();
            }
        }


        #endregion
    }
}
