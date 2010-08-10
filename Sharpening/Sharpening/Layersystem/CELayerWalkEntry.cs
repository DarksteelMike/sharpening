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
    internal class CELayerWalkEntry
    {
        internal bool Visited;
        internal List<CELayerWalkEntry> DependsOn; //Those that this one depends on.
        internal List<CELayerWalkEntry> Dependants; //Those who depend on this one.

        internal List<FieldInfo> DependsFields;
        internal List<FieldInfo> TargetFields;

        private Effect myEffect;
        internal Effect MyEffect
        {
            get { return myEffect; }
        }
        private CardBase cardSrc;
        internal CardBase CardSrc
        {
            get { return cardSrc; }
        }
        private bool isCDA;
        internal bool IsCDA
        {
            get { return isCDA; }
        }

        internal CELayerWalkEntry()
        {
            Visited = false;
            DependsOn = new List<CELayerWalkEntry>();
            Dependants = new List<CELayerWalkEntry>();

            DependsFields = new List<FieldInfo>();
            TargetFields = new List<FieldInfo>();
        }
    }
}
