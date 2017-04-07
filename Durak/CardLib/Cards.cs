﻿/*  Cards.cs 
 *  Description - A collection of Card objects, inherits from Collection class List
 *                and inplements the ICloneable interface.
 *  Author  : Chris Calder     
 *  Version : 1.0    
 *  Since   : 1.0 , Feb 2017
 *  
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardLib
{
    public class Cards : List<Card>, ICloneable
    {
        #region METHODS
                
        /// <summary>
        /// Utility method for copying card instances into another Cards
        /// instance—used in Deck.Shuffle(). This implementation assumes that
        /// source and target collections are the same size.
        /// </summary>
        public void CopyTo(Cards targetCards)
        {
            for (int index = 0; index < this.Count; index++)
            {
                targetCards[index] = this[index];
            }
        }
              
        /// <summary>
        /// Creates a deep copy of a Cards object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Cards newCards = new Cards();
            foreach (Card sourceCard in this)
            {
                newCards.Add((Card)sourceCard.Clone());
            }
            return newCards;
        }

        #endregion
    }
}
