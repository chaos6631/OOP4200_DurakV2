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
        
        public frmDurak()
        {
            InitializeComponent();
        }

        private void frmDurak_Load(object sender, EventArgs e)
        {
            #region Get Player Name/Deck Size
            //Create frmStartup to retrieve player info
            Form playerDetails = new frmStartup();
            playerDetails.ShowDialog();
            #endregion

            //Create Deck   

            //Create Player
            DurakGameLib.Player playerOne = new DurakGameLib.Player();
            //Create Opponent
            
            //Distribute starting hand

            //Assign trump

            //Determine player order

            #region Populate Starting Display Controls
            //Names
            lblPlayerName.Text = "";
            lblOpponentName.Text = "CPU";
            //Roles
            if (playerOne.IsAttacker)
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
            //Trump Card
                        
            
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

        
    }
}
