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

        private int basePower;
        internal int BasePower
        {
            get { return basePower; }
        }

        private int baseToughness;
        internal int BaseToughness
        {
            get { return baseToughness; }
        }

        private int actualPower;
        internal int ActualPower
        {
            get { return actualPower; }
            set { actualPower = value; }
        }

        private int actualToughness;
        internal int ActualToughness
        {
            get { return actualToughness; }
            set { ActualToughness = value; }
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

        public CharacteristicsCollection()
        {
            supertypes = new List<string>();
            types = new List<string>();
            subtypes = new List<string>();

            previousLocation = CardLocation.Library;
            location = CardLocation.Library;

        }
    }
}
