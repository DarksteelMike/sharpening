using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    public enum CardLocation { Library,Hand,Battlefield,Graveyard,Exile };

    internal class Player
    {
        private int playerID;
        internal int PlayerID
        {
            get { return playerID; }
        }

        private Game InvolvedGame;

        private int life;
        internal int Life
        {
            get { return life; }
        }

        private int poisonCounters;
        internal int PoisonCounters
        {
            get { return poisonCounters; }
        }

        private int maximumHandSize;
        internal int MaximumHandSize
        {
            get { return maximumHandSize; }
            set { maximumHandSize = value; }
        }

        private int maxLandPerTurn;
        internal int MaxLandPerTurn
        {
            get { return maxLandPerTurn; }
            set { maxLandPerTurn = value; }
        }

        private int landsPlayedThisTurn;
        internal int LandsPlayedThisTurn
        {
            get { return landsPlayedThisTurn; }
        }

        private ReplacableEvent LoseLife;
        private ReplacableEvent GainLife;
        private ReplacableEvent DrawCard;
        private ReplacableEvent DiscardCard;
        private ReplacableEvent DiscardCardAsCost;
        private ReplacableEvent DiscardCardAsEffect;

        private List<CardBase> libraryCards;
        internal List<CardBase> LibraryCards
        {
            get { return libraryCards; }
        }
        private List<CardBase> handCards;
        internal List<CardBase> HandCards
        {
            get { return handCards; }
        }
        private List<CardBase> battlefieldCards;
        internal List<CardBase> BattlefieldCards
        {
            get { return battlefieldCards; }
        }
        private List<CardBase> graveyardCards;
        internal List<CardBase> GraveyardCards
        {
            get { return graveyardCards; }
        }
        private List<CardBase> exileCards;
        internal List<CardBase> ExileCards
        {
            get { return exileCards; }
        }

        private List<CardBase> ownedCards;
        internal List<CardBase> OwnedCards
        {
            get { return ownedCards; }
        }

        public Player(int id,Game g)
        {
            playerID = id;
            InvolvedGame = g;
            life = 20;
            poisonCounters = 0;
            landsPlayedThisTurn = 0;
            maxLandPerTurn = 1;

            libraryCards = new List<CardBase>();
            handCards = new List<CardBase>();
            battlefieldCards = new List<CardBase>();
            graveyardCards = new List<CardBase>();
            exileCards = new List<CardBase>();
            ownedCards = new List<CardBase>();

            LoseLife = new ReplacableEvent(new Effect(delegate(object[] param)
            {
                life -= (int)param[0];
            }));

            GainLife = new ReplacableEvent(new Effect(delegate(object[] param)
            {
                life += (int)param[0];
            }));

            DrawCard = new ReplacableEvent(new Effect(delegate(object[] param)
            {
                life -= (int)param[0];
            }));

            DiscardCard = new ReplacableEvent(new Effect(delegate(object[] param)
            {
                
            }));

            DiscardCardAsCost = new ReplacableEvent(new Effect(delegate(object[] param)
            {
                DiscardCard.Run();
            }));

            DiscardCardAsEffect = new ReplacableEvent(new Effect(delegate(object[] param)
            {
                DiscardCard.Run();
            }));
        }
    }
}
