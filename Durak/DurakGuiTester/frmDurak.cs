using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DurakGuiTester
{
    public partial class frmDurak : Form
    {
        #region FIELDS & PROPS
        private string playerName;   // this should be set in forms constructor, value is passed by frmStartup
        private string opponentName = "CPU";
        private int talonSize;        // this should be set in forms constructor, value is passed by frmStartup
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
            DurakGameLib.Game myGame = new DurakGameLib.Game(PlayerName, OpponentName);
            myGame.StartGame(TalonSize); // CARDS ARE DEALT AND TRUMPCARD IS DECIDED
            //Determine attacker/defender
            myGame.IsHumanAttacker();

            #region Populate Starting Display Controls
            // SET NAMES
            lblPlayerName.Text = PlayerName;
            lblOpponentName.Text = OpponentName;
            // UPDATE PLAYER LABELS IN FORM
            if (myGame.HumanPlayer.IsAttacker)
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
            lblCardsRemainingDisplay.Text = myGame.GameDeck.RemainingCardCount().ToString();
            //Trump
            CardUserControl.CardUserControl trumpCardDisplay = new CardUserControl.CardUserControl();
            pnlDeckArea.Controls.Add(trumpCardDisplay);
            trumpCardDisplay.Location = new Point(10,12);
            trumpCardDisplay.Card = myGame.GameTrumpCard;
            trumpCardDisplay.Show();

            // NEED TO POPULATE PLAYERS CARDS IN THE FORM HERE
            //Display hands
            //Player
            for (int index = 0; index < myGame.HumanPlayer.PlayerHand.Count(); index++)
            {
                CardUserControl.CardUserControl handCard = new CardUserControl.CardUserControl();
                handCard.Card = myGame.HumanPlayer.PlayerHand.ElementAt(index);
                handCard.FaceUp = true;
                handCard.Click += new EventHandler(card_Click);
                pnlPlayer.Controls.Add(handCard);
                handCard.BringToFront();
                handCard.Location = new Point((580 / myGame.HumanPlayer.PlayerHand.Count()) + (1160 / myGame.HumanPlayer.PlayerHand.Count() * index), 12);
            }
            //CPU
            for (int index = 0; index < myGame.ComputerPlayer.PlayerHand.Count(); index++)
            {
                CardUserControl.CardUserControl handCard = new CardUserControl.CardUserControl();
                handCard.Card = myGame.ComputerPlayer.PlayerHand.ElementAt(index);
                handCard.FaceUp = true; //Remove after debugging
                pnlOpponent.Controls.Add(handCard);
                handCard.BringToFront();
                handCard.Location = new Point((580 / myGame.ComputerPlayer.PlayerHand.Count()) + (1160 / myGame.HumanPlayer.PlayerHand.Count() * index), 12);
            }
            // TRIGGER ATTACKERS CARD SELECTION METHOD
            //Human player set to attacker for debugging purposes
            myGame.HumanPlayer.IsAttacker = true;
            myGame.ComputerPlayer.IsAttacker = false;

            if (myGame.HumanPlayer.IsAttacker)
            {
                //myGame.PlayerTurn(myGame.HumanPlayer);
            }
            else
            {
                
            }
            #endregion
        }


        #endregion

        #region METHODS
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
        protected void card_Click(object sender, EventArgs e)
        {
            //CURRENTLY EVENT IS NOT TRIGGERING AT ALL
            CardUserControl.CardUserControl card = sender as CardUserControl.CardUserControl;
            card.FaceUp = false;
        }

        #endregion
    }
}
