﻿using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal delegate void CardMovedEventHandler(CardMovedEventArgs e);
    internal abstract class CardBase
    {
        protected Game InvolvedGame;

        //For text-changing effects
        //Type: Artificial Evolution
        //Land: Magical Hack,Spectral Shift
        //Color: Alter Reality,Balduvian Shaman,Glamerdye,Sleight of Mind,Spectral Shift,Swirl the Mists
        //Both Land and Color: Crystal Spray, Mind Bend,Whim of Volrath
        protected string GetFinalText(string BeginningWord, List<TextChangeOperation> Changers)
        {
            string Result = BeginningWord;
            foreach (TextChangeOperation Op in Changers)
            {
                if (Result == Op.ReplaceFrom) Result = Op.ReplaceTo;
            }

            return Result;
        }

        internal bool HasSupertype(string Supertype)
        {
            foreach (string s in currentCharacteristics.Supertypes)
            {
                if (Supertype == GetFinalText(s, typeChangeOperations))
                {
                    return true;
                }
            }

            return false;
        }

        internal bool HasType(string Type)
        {
            foreach (string s in currentCharacteristics.Types)
            {
                if (Type == GetFinalText(s, typeChangeOperations))
                {
                    return true;
                }
            }

            return false;
        }

        internal bool HasSubType(string Subtype)
        {
            foreach (string s in currentCharacteristics.Subtypes)
            {
                if (Subtype == GetFinalText(s, typeChangeOperations))
                {
                    return true;
                }
            }

            return false;
        }

        internal bool HasAnyType(string AnyType)
        {
            return HasSupertype(AnyType) || HasType(AnyType) || HasSubType(AnyType);
        }

        protected List<TextChangeOperation> typeChangeOperations;
        internal List<TextChangeOperation> TypeChangeOperations
        {
            get { return typeChangeOperations; }
        }

        protected List<TextChangeOperation> landChangeOperations;
        internal List<TextChangeOperation> LandChangeOperations
        {
            get { return landChangeOperations; }
        }

        protected List<TextChangeOperation> colorChangeOperations;
        internal List<TextChangeOperation> ColorChangeOperations
        {
            get { return colorChangeOperations; }
        }

        protected static int nextTimestamp = 0;
        internal static int NextTimestamp
        {
            get { return ++nextTimestamp; }
        }
        protected static int nextCardID = 0;
        internal static int NextCardID
        {
            get { return ++nextCardID; }
        }

        protected int timestamp;
        internal int Timestamp
        {
            get { return timestamp; }
        }

        protected int cardID;
        internal int CardID
        {
            get { return cardID; }
        }

        protected string name;
        internal string Name
        {
            get { return name; }
        }
        
        protected bool autoUntaps;
        internal bool AutoUntaps
        {
        	get { return autoUntaps; }
        }

        protected ReplacableEvent Destroyed;
        protected ReplacableEvent EntersBattlefield;
        protected ReplacableEvent LeavesBattlefield;
        protected ReplacableEvent EntersGraveyard;
        protected ReplacableEvent LeavesGraveyard;
        protected ReplacableEvent Tapped;
        protected ReplacableEvent Untapped;
        protected ReplacableEvent Damaged;
        protected ReplacableEvent DiscardedAsCost;
        protected ReplacableEvent DiscardedAsEffect;
        protected ReplacableEvent Discarded;

        protected List<Activatable> activatables;
        internal List<Activatable> Activatables
        {
            get { return activatables; }
        }

        protected CharacteristicsCollection baseCharacteristics;
        internal CharacteristicsCollection BaseCharacteristics
        {
            get { return baseCharacteristics; }
        }
        protected CharacteristicsCollection currentCharacteristics;
        internal CharacteristicsCollection CurrentCharacteristics
        {
        	get { return currentCharacteristics; }
        	set { currentCharacteristics = value; }
        }

        internal CardBase(Game g)
        {
            InvolvedGame = g;
            cardID = CardBase.NextCardID;
            
            baseCharacteristics = new CharacteristicsCollection();
            currentCharacteristics = new CharacteristicsCollection();

            typeChangeOperations = new List<TextChangeOperation>();
            colorChangeOperations = new List<TextChangeOperation>();
            landChangeOperations = new List<TextChangeOperation>();

            activatables = new List<Activatable>();

            Destroyed = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    Move(CardLocation.Graveyard);
                }));

            EntersBattlefield = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    timestamp = CardBase.NextTimestamp;
                }));

            Discarded = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    Move(CardLocation.Graveyard);
                }));

            DiscardedAsCost = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    Discarded.Run();
                }));

            DiscardedAsEffect = new ReplacableEvent(new Effect(delegate(object[] param)
                {
                    Discarded.Run();
                }));
        }

        internal void Move(CardLocation Target)
        {
            currentCharacteristics.PreviousLocation = currentCharacteristics.Location;
            currentCharacteristics.Location = Target;

            if (currentCharacteristics.PreviousLocation == CardLocation.Battlefield)
            {
                LeavesBattlefield.Run();
            }

            //Remove card from appropriate list
            switch (currentCharacteristics.PreviousLocation)
            {
                case (CardLocation.Library):
                    currentCharacteristics.Controller.LibraryCards.Remove(this);
                    break;
                case (CardLocation.Hand):
                    currentCharacteristics.Controller.HandCards.Remove(this);
                    break;
                case (CardLocation.Battlefield):
                    currentCharacteristics.Controller.BattlefieldCards.Remove(this);
                    LeavesBattlefield.Run();
                    break;
                case (CardLocation.Graveyard):
                    currentCharacteristics.Controller.GraveyardCards.Remove(this);
                    LeavesGraveyard.Run();
                    break;
                case (CardLocation.Exile):
                    currentCharacteristics.Controller.ExileCards.Remove(this);
                    break;
            }

            //Add card to appropriate list
            switch (currentCharacteristics.Location)
            {
                case (CardLocation.Library):
                    currentCharacteristics.Owner.LibraryCards.Add(this);
                    break;
                case (CardLocation.Hand):
                    currentCharacteristics.Owner.HandCards.Add(this);
                    break;
                case (CardLocation.Battlefield):
                    currentCharacteristics.Controller.BattlefieldCards.Add(this);
                    EntersBattlefield.Run();
                    break;
                case (CardLocation.Graveyard):
                    currentCharacteristics.Owner.GraveyardCards.Add(this);
                    EntersGraveyard.Run();
                    break;
                case (CardLocation.Exile):
                    currentCharacteristics.Owner.ExileCards.Add(this);
                    break;
            }
        }


    }
}
