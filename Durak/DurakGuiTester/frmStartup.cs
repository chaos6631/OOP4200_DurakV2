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
    public partial class frmStartup : Form
    {
        public frmStartup()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //Player
            try
            {
                //string name = txtPlayerName.Text;
                //DurakGameLib.Player playerOne = new DurakGameLib.Player(name);
                //Deck
                //int decksize = (int)lbxDeckSize.SelectedValue;
                //DurakGameLib.Talon gameTalon = new DurakGameLib.Talon();
            }
            catch (Exception ex)
            {

            }
           
        }
    }
}
