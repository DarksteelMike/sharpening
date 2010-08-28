/*
 * Created by SharpDevelop.
 * User: Elev
 * Date: 2010-08-02
 * Time: 09:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sharpening
{
    /// <summary>
    /// Description of CELayerWalkEntry.
    /// </summary>
    internal class CELayerEntry
    {
        internal bool Visited;
        internal List<CELayerEntry> DependsOn; //Those that this one depends on.
        internal List<CELayerEntry> Dependants; //Those who depend on this one.

        internal List<FieldInfo> DependsFields;
        internal List<FieldInfo> TargetFields;

        private Effect myEffect;
        internal Effect MyEffect
        {
            get { return myEffect; }
            set { myEffect = value; }
        }
        private CardBase cardSrc;
        internal CardBase CardSrc
        {
            get { return cardSrc; }
            set { cardSrc = value; }
        }
        private CardBase cardTgt;
        internal CardBase CardTgt
        {
        	get { return cardTgt; }
        	set { cardTgt = value; }
        }
        private Condition appliesTo;
        internal Condition AppliesTo
        {
        	get { return appliesTo; }
        	set { appliesTo = value; }
        }
        private bool isCDA;
        internal bool IsCDA
        {
            get { return isCDA; }
            set { isCDA = value; }
        }

        internal CELayerEntry()
        {
            Visited = false;
            DependsOn = new List<CELayerEntry>();
            Dependants = new List<CELayerEntry>();

            DependsFields = new List<FieldInfo>();
            TargetFields = new List<FieldInfo>();
        }
    }
}
