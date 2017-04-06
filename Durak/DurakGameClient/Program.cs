using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DurakGameLib;

namespace DurakGameClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Game newGame = new Game();
            newGame.StartGame("Chris");

            bool continuePlaying = true;
            while (continuePlaying)
            {
                if (newGame.GameDeck.RemainingCardCount() > 0)
                {
                    newGame.PlayNextRound();
                }
                else if (newGame.HumanPlayer.PlayerHand.Count > 0 && newGame.ComputerPlayer.PlayerHand.Count > 0)
                {
                    newGame.PlayNextRound();
                }
                else
                {
                    continuePlaying = false;
                    newGame.EndGame();
                }
            }
        }
    }
}
