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
            //Create frmStartup to retrieve player info
            Form playerDetails = new frmStartup();
            playerDetails.ShowDialog();
            
            #endregion

                       
            //THESE SHOULD BE HANDLED IN GAME 
            //Create Player
            //DurakGameLib.Player playerOne = new DurakGameLib.Player(name);
            //Create Opponent
            //DurakGameLib.Player opponent = new DurakGameLib.Player("cpu");

            //Create/Start Game
            DurakGameLib.Game myGame = new DurakGameLib.Game(PlayerName, OpponentName);
            myGame.StartGame(TalonSize);

            #region Populate Starting Display Controls
            //Names
            lblPlayerName.Text = PlayerName;
            lblOpponentName.Text = OpponentName;
            //Roles
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
            //Display hand

            //Talon
            cardTopDeck.Visible = true;
            //lblCardsRemainingDisplay.Text = myGame.gameDeck;
            //Trump
            CardUserControl.CardUserControl trumpCardDisplay = new CardUserControl.CardUserControl();
            pnlDeckArea.Controls.Add(trumpCardDisplay);
            trumpCardDisplay.Card = myGame.GameTrumpCard;
            trumpCardDisplay.FaceUp = true;
            trumpCardDisplay.Show();

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

        #endregion
    }
}
