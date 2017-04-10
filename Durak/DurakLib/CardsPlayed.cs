using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardLib;

namespace DurakGameLib
{
    public class CardsPlayed : Stack<Card>
    {
        /// <summary>
        /// to be used by aiplayer to make decisions.
        /// this should be set when a human player plays a card, and it is added to the CardsPlayed object
        /// </summary>
        Card humanLastCardPlayed;
        Card computerLastCardPlayed;     
        

        public Card HumanLastCardPlayed
        {
            get
            {
                return humanLastCardPlayed;
            }

            set
            {
                humanLastCardPlayed = value;
            }
        }

        public Card ComputerLastCardPlayed
        {
            get
            {
                return computerLastCardPlayed;
            }

            set
            {
                computerLastCardPlayed = value;
            }
        }
    }

}
