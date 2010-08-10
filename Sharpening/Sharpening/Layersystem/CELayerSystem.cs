using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace Sharpening
{
    internal enum CELayer { Layer1 = 0, Layer2, Layer3, Layer4, Layer5, Layer6, Layer7A, Layer7B, Layer7C, Layer7D, Layer7E };

    //NOTE: This is based on 2007 rules, as elaborated on in an article by Nick Sephton
    //http://www.wizards.com/Magic/Magazine/Article.aspx?x=judge/article/20071126a
    internal class CELayerSystem
    {
        private Game InvolvedGame;
        private List<CELayerWalkEntry> Layer1; //Copy CELayerEntrys
        private List<CELayerWalkEntry> Layer2; //Control-changing CELayerEntrys
        private List<CELayerWalkEntry> Layer3; //Text-changing CELayerEntrys
        private List<CELayerWalkEntry> Layer4; //Type-changing CELayerEntrys
        private List<CELayerWalkEntry> Layer5; //Color-changing CELayerEntrys
        private List<CELayerWalkEntry> Layer6; //Ability-changing CELayerEntrys
        private List<CELayerWalkEntry> Layer7A; //P/T CDAs
        private List<CELayerWalkEntry> Layer7B; //Specifically setting P/T CELayerEntrys
        private List<CELayerWalkEntry> Layer7C; //Static abilities that modify but do not specifically set P/T
        private List<CELayerWalkEntry> Layer7D; //Counters
        private List<CELayerWalkEntry> Layer7E; //CELayerEntrys that switch P/T

        private Dictionary<CELayer, List<CELayerWalkEntry>> LayerMap;

        internal CELayerSystem(Game g)
        {
            InvolvedGame = g;
            Layer1 = new List<CELayerWalkEntry>();
            Layer2 = new List<CELayerWalkEntry>();
            Layer3 = new List<CELayerWalkEntry>();
            Layer4 = new List<CELayerWalkEntry>();
            Layer5 = new List<CELayerWalkEntry>();
            Layer6 = new List<CELayerWalkEntry>();
            Layer7A = new List<CELayerWalkEntry>();
            Layer7B = new List<CELayerWalkEntry>();
            Layer7C = new List<CELayerWalkEntry>();
            Layer7D = new List<CELayerWalkEntry>();
            Layer7E = new List<CELayerWalkEntry>();

            LayerMap = new Dictionary<CELayer, List<CELayerWalkEntry>>();
            LayerMap.Add(CELayer.Layer1, Layer1);
            LayerMap.Add(CELayer.Layer2, Layer2);
            LayerMap.Add(CELayer.Layer3, Layer3);
            LayerMap.Add(CELayer.Layer4, Layer4);
            LayerMap.Add(CELayer.Layer5, Layer5);
            LayerMap.Add(CELayer.Layer6, Layer6);
            LayerMap.Add(CELayer.Layer7A, Layer7A);
            LayerMap.Add(CELayer.Layer7B, Layer7B);
            LayerMap.Add(CELayer.Layer7C, Layer7C);
            LayerMap.Add(CELayer.Layer7D, Layer7D);
            LayerMap.Add(CELayer.Layer7E, Layer7E);
        }

        internal void AddEntry(CELayer Layer, CELayerWalkEntry Entry)
        {
            LayerMap[Layer].Add(Entry);
            if (Layer != CELayer.Layer7A && Layer != CELayer.Layer7B && Layer != CELayer.Layer7C && Layer != CELayer.Layer7D && Layer != CELayer.Layer7E)
            {
                SortLayerCDA(LayerMap[Layer]);
            }
            else
            {
                SortLayer(LayerMap[Layer]);
            }
        }

        internal void RemoveAllEntriesFromCard(CardBase Card)
        {
            foreach (CELayer LayerKey in LayerMap.Keys)
            {
                foreach (CELayerWalkEntry Entry in LayerMap[LayerKey])
                {
                    if (Entry.CardSrc.CardID == Card.CardID)
                    {
                        LayerMap[LayerKey].Remove(Entry);
                    }
                }
            }
        }

        //Bubblesorts by timestamp
        private void SortLayer(List<CELayerWalkEntry> Layer)
        {
            bool HasChanged = false;
            do
            {
                HasChanged = false;
                for (int i = 0; i < Layer.Count - 1; i++)
                {
                    if (Layer[i].CardSrc.Timestamp > Layer[i + 1].CardSrc.Timestamp)
                    {
                        HasChanged = true;
                        CELayerWalkEntry tmp = Layer[i + 1];
                        Layer[i + 1] = Layer[i];
                        Layer[i] = tmp;
                    }
                }
            } while (HasChanged);
        }

        //Bubblesorts by timestamp,prioritizing CDA effects
        private void SortLayerCDA(List<CELayerWalkEntry> Layer)
        {
            List<CELayerWalkEntry> CDAEffects = new List<CELayerWalkEntry>();
            List<CELayerWalkEntry> OtherEffects = new List<CELayerWalkEntry>();

            foreach (CELayerWalkEntry Entry in Layer)
            {
                if (Entry.IsCDA)
                {
                    CDAEffects.Add(Entry);
                }
                else
                {
                    OtherEffects.Add(Entry);
                }
            }

            bool HasChanged = false;

            do
            {
                HasChanged = false;
                for (int i = 0; i < CDAEffects.Count - 1; i++)
                {
                    if (CDAEffects[i].CardSrc.Timestamp > CDAEffects[i + 1].CardSrc.Timestamp)
                    {
                        HasChanged = true;
                        CELayerWalkEntry tmp = CDAEffects[i];
                        CDAEffects[i] = CDAEffects[i + 1];
                        CDAEffects[i + 1] = tmp;
                    }
                }
            } while (HasChanged);

            do
            {
                HasChanged = false;
                for (int i = 0; i < OtherEffects.Count - 1; i++)
                {
                    if (OtherEffects[i].CardSrc.Timestamp > OtherEffects[i + 1].CardSrc.Timestamp)
                    {
                        HasChanged = true;
                        CELayerWalkEntry tmp = OtherEffects[i];
                        OtherEffects[i] = OtherEffects[i + 1];
                        OtherEffects[i + 1] = tmp;
                    }
                }
            } while (HasChanged);

            Layer.Clear();

            Layer.AddRange(CDAEffects);
            Layer.AddRange(OtherEffects);
        }

        internal void SortLayerDependency(List<CELayerEntry> Layer)
        {
            List<CELayerEntry> SortByTimestamp = new List<CELayerEntry>();
            List<List<int>> DependsOn = new List<List<int>>();
            for (int i = 0; i < Layer.Count; i++)
            {
                DependsOn.Add(new List<int>());
            }

            for (int PrimEntryNum = 0; PrimEntryNum < Layer.Count; PrimEntryNum++)
            {
                foreach (FieldInfo PrimFI in Layer[PrimEntryNum].DependsOnFields)
                {
                    for (int SecEntryNum = 0; SecEntryNum < Layer.Count; SecEntryNum++)
                    {
                        foreach (FieldInfo SecFI in Layer[SecEntryNum].ChangingFields)
                        {
                            if (PrimFI.Name == SecFI.Name)
                            {
                                DependsOn[PrimEntryNum].Add(SecEntryNum);
                            }
                        }
                    }
                }
            }
        }

        internal void Run()
        {
            foreach (Player p in InvolvedGame.Players)
            {
                foreach (CardBase c in p.OwnedCards)
                {
                    c.LandChangeOperations.Clear();
                    c.ColorChangeOperations.Clear();
                    c.TypeChangeOperations.Clear();
                    c.Characteristics.ActualPower = c.Characteristics.BasePower;
                    c.Characteristics.ActualToughness = c.Characteristics.BaseToughness;
                }
            }
        }

        internal void SortDependency(ref List<CELayerWalkEntry> Layer)
        {
            int EntryNum = Layer.Count;
            //First, determine all "bonds" between entries. I.e. who depends on who?
            for (int i = 0; i < Layer.Count; i++)
            {
                for (int j = 0; j < Layer.Count; j++)
                {
                    if (i != j)
                    {
                        foreach (FieldInfo FI1 in Layer[i].DependsFields)
                        {
                            foreach (FieldInfo FI2 in Layer[j].TargetFields)
                            {
                                if (FI1 == FI2)
                                {
                                    Layer[i].DependsOn.Add(Layer[j]);
                                    Layer[j].Dependants.Add(Layer[i]);
                                }
                            }
                        }
                    }
                }
            }

            List<CELayerWalkEntry> Final = new List<CELayerWalkEntry>();

            while (Final.Count < EntryNum)
            {
                //While there are entries without DependsOn entries, add those to output in timestamp order
                List<CELayerWalkEntry> NoDependencies = new List<CELayerWalkEntry>();
                bool EntriesCanBeCulled = true;
                while (EntriesCanBeCulled)
                {
                    EntriesCanBeCulled = false;
                    foreach (CELayerWalkEntry CELWE in Layer)
                    {
                        if (CELWE.DependsOn.Count == 0)
                        {
                            foreach (CELayerWalkEntry CELWE2 in Layer)
                            {
                                CELWE2.Dependants.Remove(CELWE);
                                CELWE2.DependsOn.Remove(CELWE);
                            }
                            Layer.Remove(CELWE);
                            NoDependencies.Add(CELWE);
                            EntriesCanBeCulled = true;
                        }
                    }
                }

                foreach (CELayerWalkEntry CELWE in Layer)
                {
                    CELWE.Visited = false;
                }

                SortLayer(NoDependencies); //TODO: Change SortLayer.
                Final.AddRange(NoDependencies.ToArray());

                //Walk the tree to detect and take care of circular dependencies
                List<CELayerWalkEntry> CircularDependency = new List<CELayerWalkEntry>();
                if (WalkDependencyTree(Layer[0], CircularDependency))
                {
                    SortLayer(CircularDependency); //TODO: Change SortLayer.
                    Final.AddRange(CircularDependency.ToArray());
                    foreach (CELayerWalkEntry CELWE1 in CircularDependency)
                    {
                        foreach (CELayerWalkEntry CELWE2 in Layer)
                        {
                            CELWE2.Dependants.Remove(CELWE1);
                            CELWE2.DependsOn.Remove(CELWE1);
                        }
                        Layer.Remove(CELWE1);
                    }
                }
            }

            Layer = Final;
        }

        internal bool WalkDependencyTree(CELayerWalkEntry StartPoint, List<CELayerWalkEntry> CircDepList)
        {
            foreach (CELayerWalkEntry CELWE in StartPoint.Dependants)
            {
                if (CELWE.Visited) //We've been here already! Climb back up the tree and add all passed nodes to the Circular Dependency List
                {
                    CircDepList.Add(CELWE);
                    return true;
                }
                else //Havn't been here yet. Mark it as visited and check if we're at a leaf node.
                {
                    CELWE.Visited = true;
                    if (CELWE.Dependants.Count == 0) //We're at a leaf node! This path doesn't lead into a circular dependency.
                    {
                        return false;
                    }
                    else //We're not at a leaf node. Climb deeper down this branch.
                    {
                        if (WalkDependencyTree(CELWE, CircDepList)) //If one of the nodes further down this branch leads into a circular dependency, this one must too! Mark it as such and return.
                        {
                            CircDepList.Add(CELWE);
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
