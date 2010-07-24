using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal abstract class CardBase
    {
        private Game InvolvedGame;

        //For text-changing effects
        //Type: Artificial Evolution
        //Land: Magical Hack,Spectral Shift
        //Color: Alter Reality,Balduvian Shaman,Glamerdye,Sleight of Mind,Spectral Shift,Swirl the Mists
        //Both Land and Color: Crystal Spray, Mind Bend,Whim of Volrath
        private string GetFinalText(string BeginningWord,List<TextChangeOperation> Changers)
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
            foreach (string s in characteristics.Supertypes)
            {
                if(Supertype == GetFinalText(s,typeChangeOperations))
                {
                    return true;
                }
            }

            return false;
        }

        internal bool HasType(string Type)
        {
            foreach (string s in characteristics.Types)
            {
                if(Type == GetFinalText(s,typeChangeOperations))
                {
                    return true;
                }
            }

            return false;
        }

        internal bool HasSubType(string Subtype)
        {
            foreach (string s in characteristics.Subtypes)
            {
                if(Subtype == GetFinalText(s,typeChangeOperations))
                {
                    return true;
                }
            }

            return false;
        }
        
        private List<TextChangeOperation> typeChangeOperations;
        internal List<TextChangeOperation> TypeChangeOperations
        {
            get { return typeChangeOperations; }
        }

        private List<TextChangeOperation> landChangeOperations;
        internal List<TextChangeOperation> LandChangeOperations
        {
            get { return landChangeOperations; }
        }

        private List<TextChangeOperation> colorChangeOperations;
        internal List<TextChangeOperation> ColorChangeOperations 
        {
            get { return colorChangeOperations; }
        }

        private static int nextTimestamp = 0;
        internal static int NextTimestamp
        {
            get { return ++nextTimestamp; }
        }
        private static int nextCardID = 0;
        internal static int NextCardID
        {
            get { return ++nextCardID; }
        }

        private int timestamp;
        internal int Timestamp
        {
            get { return timestamp; }
        }

        private int cardID;
        internal int CardID
        {
            get { return cardID; }
        }

        private string name;
        internal string Name
        {
            get { return name; }
        }

        private ReplacableEvent Destroyed;
        private ReplacableEvent EntersBattlefield;
        private ReplacableEvent LeavesBattlefield;
        private ReplacableEvent EntersGraveyard;
        private ReplacableEvent LeavesGraveyard;
        private ReplacableEvent Tapped;
        private ReplacableEvent Untapped;
        private ReplacableEvent Damaged;
        private ReplacableEvent DiscardedAsCost;
        private ReplacableEvent DiscardedAsEffect;
        private ReplacableEvent Discarded;

        private List<Activatable> activatables;
        internal List<Activatable> Activatables
        {
            get { return activatables; }
        }

        private CharacteristicsCollection characteristics;
        internal CharacteristicsCollection Characteristics
        {
            get { return characteristics; }
        }

        internal CardBase(Game g)
        {
            InvolvedGame = g;
            cardID = CardBase.NextCardID;

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
            characteristics.PreviousLocation = characteristics.Location;
            characteristics.Location = Target;

            if (characteristics.PreviousLocation == CardLocation.Battlefield)
            {
                LeavesBattlefield.Run();
            }

            //Remove card from appropriate list
            switch (characteristics.PreviousLocation)
            {
                case (CardLocation.Library):
                    characteristics.Controller.LibraryCards.Remove(this);
                    break;
                case (CardLocation.Hand):
                    characteristics.Controller.HandCards.Remove(this);
                    break;
                case (CardLocation.Battlefield):
                    characteristics.Controller.BattlefieldCards.Remove(this);
                    LeavesBattlefield.Run();
                    break;
                case (CardLocation.Graveyard):
                    characteristics.Controller.GraveyardCards.Remove(this);
                    LeavesGraveyard.Run();
                    break;
                case (CardLocation.Exile):
                    characteristics.Controller.ExileCards.Remove(this);
                    break;
            }

            //Add card to appropriate list
            switch (characteristics.Location)
            {
                case (CardLocation.Library):
                    characteristics.Owner.LibraryCards.Add(this);
                    break;
                case (CardLocation.Hand):
                    characteristics.Owner.HandCards.Add(this);
                    break;
                case (CardLocation.Battlefield):
                    characteristics.Controller.BattlefieldCards.Add(this);
                    EntersBattlefield.Run();
                    break;
                case (CardLocation.Graveyard):
                    characteristics.Owner.GraveyardCards.Add(this);
                    EntersGraveyard.Run();
                    break;
                case (CardLocation.Exile):
                    characteristics.Owner.ExileCards.Add(this);
                    break;
            }
        }

        
    }
}
