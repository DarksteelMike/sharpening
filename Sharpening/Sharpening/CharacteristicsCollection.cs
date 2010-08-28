using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpening
{
    internal class CharacteristicsCollection
    {
        private Player owner;
        internal Player Owner
        {
            get { return owner; }
        }

        private Player controller;
        internal Player Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        private CardLocation previousLocation;
        internal CardLocation PreviousLocation
        {
            get { return previousLocation; }
            set { previousLocation = value; }
        }

        private CardLocation location;
        internal CardLocation Location
        {
            get { return location; }
            set { location = value; }
        }

        private int power;
        internal int Power
        {
            get { return power; }
            set { power = value; }
        }

        private int toughness;
        internal int Toughness
        {
            get { return toughness; }
            set { toughness = value; }
        }

        private int assignedDamage;
        internal int AssignedDamage
        {
            get { return assignedDamage; }
        }

        private List<string> supertypes;
        internal List<string> Supertypes
        {
            get { return supertypes; }
        }

        private List<string> types;
        internal List<string> Types
        {
            get { return types; }
        }

        private List<string> subtypes;
        internal List<string> Subtypes
        {
            get { return subtypes; }
        }

        private bool isTapped;
        internal bool IsTapped
        {
            get { return isTapped; }
            set { isTapped = value; }
        }
        
        private List<string> counters;
        internal List<string> Counters
        {
        	get { return counters; }
        }
        
        private List<string> keywords;
        internal List<string> Keywords
        {
        	get { return keywords; }
        }
        
        private List<string> color;
        internal List<string> Color
        {
        	get { return color; }
        }

        public CharacteristicsCollection()
        {
            supertypes = new List<string>();
            types = new List<string>();
            subtypes = new List<string>();
            counters = new List<string>();
            keywords = new List<string>();
            color = new List<string>();

            previousLocation = CardLocation.Library;
            location = CardLocation.Library;

        }
        
        internal CharacteristicsCollection(Player COwner,Player CController, CardLocation CPreviousLocation,CardLocation CLocation,int CPower,int CToughness,int CAssignedDamage,List<string> CSupertypes,List<string> CTypes,List<string> CSubtypes,bool CIsTapped)
        {
        	owner = COwner;
        	controller = CController;
        	previousLocation = CPreviousLocation;
        	location = CLocation;
        	power = CPower;
        	toughness = CToughness;
        	assignedDamage = CAssignedDamage;
        	supertypes = CSupertypes;
        	types = CTypes;
        	subtypes = CSubtypes;
        	isTapped = CIsTapped;
        }
        
        internal CharacteristicsCollection Copy()
        {
        	return new CharacteristicsCollection(owner,controller,previousLocation,location,power,toughness,assignedDamage,supertypes,types,subtypes,isTapped);
        }
    }
}
