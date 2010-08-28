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
        private List<CELayerEntry> Layer1; //Copy CELayerEntrys
        private List<CELayerEntry> Layer2; //Control-changing CELayerEntrys
        private List<CELayerEntry> Layer3; //Text-changing CELayerEntrys
        private List<CELayerEntry> Layer4; //Type-changing CELayerEntrys
        private List<CELayerEntry> Layer5; //Color-changing CELayerEntrys
        private List<CELayerEntry> Layer6; //Ability-changing CELayerEntrys
        private List<CELayerEntry> Layer7A; //P/T CDAs
        private List<CELayerEntry> Layer7B; //Specifically setting P/T CELayerEntrys
        private List<CELayerEntry> Layer7C; //Static abilities that modify but do not specifically set P/T
        private List<CELayerEntry> Layer7D; //Counters
        private List<CELayerEntry> Layer7E; //CELayerEntrys that switch P/T

        private Dictionary<CELayer, List<CELayerEntry>> LayerMap;

        internal CELayerSystem(Game g)
        {
            InvolvedGame = g;
            Layer1 = new List<CELayerEntry>();
            Layer2 = new List<CELayerEntry>();
            Layer3 = new List<CELayerEntry>();
            Layer4 = new List<CELayerEntry>();
            Layer5 = new List<CELayerEntry>();
            Layer6 = new List<CELayerEntry>();
            Layer7A = new List<CELayerEntry>();
            Layer7B = new List<CELayerEntry>();
            Layer7C = new List<CELayerEntry>();
            Layer7D = new List<CELayerEntry>();
            Layer7E = new List<CELayerEntry>();

            LayerMap = new Dictionary<CELayer, List<CELayerEntry>>();
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

        internal void AddEntry(CELayer Layer, CELayerEntry Entry)
        {
            LayerMap[Layer].Add(Entry);
            if (Layer != CELayer.Layer7A && Layer != CELayer.Layer7B && Layer != CELayer.Layer7C && Layer != CELayer.Layer7D && Layer != CELayer.Layer7E)
            {
                FullSortCDA(LayerMap[Layer]);
            }
            else
            {
                FullSort(LayerMap[Layer]);
            }
        }

        internal void RemoveAllEntriesFromCard(CardBase Card)
        {
        	Dictionary<CELayer,bool> ShouldSort = new Dictionary<CELayer,bool>();
        	ShouldSort.Add(CELayer.Layer1,false);
        	ShouldSort.Add(CELayer.Layer2,false);
        	ShouldSort.Add(CELayer.Layer3,false);
        	ShouldSort.Add(CELayer.Layer4,false);
        	ShouldSort.Add(CELayer.Layer5,false);
        	ShouldSort.Add(CELayer.Layer6,false);
        	ShouldSort.Add(CELayer.Layer7A,false);
        	ShouldSort.Add(CELayer.Layer7B,false);
        	ShouldSort.Add(CELayer.Layer7C,false);
        	ShouldSort.Add(CELayer.Layer7D,false);
        	ShouldSort.Add(CELayer.Layer7E,false);
        	
            foreach (CELayer LayerKey in LayerMap.Keys)
            {
                foreach (CELayerEntry Entry in LayerMap[LayerKey])
                {
                    if (Entry.CardSrc.CardID == Card.CardID)
                    {
                        LayerMap[LayerKey].Remove(Entry);
                        ShouldSort[LayerKey] = true;
                    }
                }
            }

            foreach (CELayer LayerKey in ShouldSort.Keys)
            {
                if (ShouldSort[LayerKey])
                {
                    if (LayerKey != CELayer.Layer7A && LayerKey != CELayer.Layer7B && LayerKey != CELayer.Layer7C && LayerKey != CELayer.Layer7D && LayerKey != CELayer.Layer7E)
                    {
                        FullSortCDA(LayerMap[LayerKey]);
                    }
                    else
                    {
                        FullSort(LayerMap[LayerKey]);
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
                    c.CurrentCharacteristics = c.BaseCharacteristics.Copy();
                }
            }
            
            RunLayer(Layer1);
            RunLayer(Layer2);
            RunLayer(Layer3);
            RunLayer(Layer4);
            RunLayer(Layer5);
            RunLayer(Layer6);
            RunLayer(Layer7A);
            RunLayer(Layer7B);
            RunLayer(Layer7C);
            RunLayer(Layer7D);
            RunLayer(Layer7E);
        }
        
        private void RunLayer(List<CELayerEntry> Layer)
        {
            foreach(CELayerEntry CELWE in Layer)
            {
            	if(CELWE.CardTgt == null)
            	{
            		foreach(Player p in InvolvedGame.Players)
            		{
            			foreach(CardBase Card in p.OwnedCards)
            			{
            				if(CELWE.AppliesTo(Card))
            				{
            					CELWE.MyEffect(Card);
            				}
            			}
            		}
            	}
            	else
            	{
            		if(CELWE.AppliesTo(CELWE.CardTgt))
            		{
            			CELWE.MyEffect(CELWE.CardTgt);
            		}
            	}
            }
        }
        
        //Combines all sorting methods, doesn't care about CDA effects
        private void FullSort(List<CELayerEntry> Layer)
        {
        	SortListByDependency(Layer);
        }

        //Combines all sorting methods but prioritizes CDA effects
        private void FullSortCDA(List<CELayerEntry> Layer)
        {
        	List<CELayerEntry> CDAs = new List<CELayerEntry>();
        	List<CELayerEntry> Others = new List<CELayerEntry>();
        	
        	foreach(CELayerEntry CELWE in Layer)
        	{
        		if(CELWE.IsCDA)
        		{
        			CDAs.Add(CELWE);
        		}
        		else
        		{
        			Others.Add(CELWE);
        		}
        	}
        	
        	SortListByTimestamp(CDAs);
        	
        	SortListByDependency(Others);
        	
        	Layer.Clear();
        	Layer.AddRange(CDAs.ToArray());
        	Layer.AddRange(Others.ToArray());        	
        }
        
        //Bubblesorts by timestamp
        private void SortListByTimestamp(List<CELayerEntry> Layer)
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
                        CELayerEntry tmp = Layer[i + 1];
                        Layer[i + 1] = Layer[i];
                        Layer[i] = tmp;
                    }
                }
            } while (HasChanged);
        }
        
        //Sorts by dependency
        private void SortListByDependency(List<CELayerEntry> Layer)
        {
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
                                if (FI1.Name == FI2.Name)
                                {
                                    Layer[i].DependsOn.Add(Layer[j]);
                                    Layer[j].Dependants.Add(Layer[i]);
                                }
                            }
                        }
                    }
                }
            }

            List<CELayerEntry> Final = new List<CELayerEntry>();

            while (Layer.Count != 0)
            {
                //While there are entries without DependsOn entries, add those to output in timestamp order
                List<CELayerEntry> NoDependencies = new List<CELayerEntry>();
                bool EntriesCanBeCulled = true;
                while (EntriesCanBeCulled)
                {
                    EntriesCanBeCulled = false;
                    foreach (CELayerEntry CELWE in Layer)
                    {
                        if (CELWE.DependsOn.Count == 0)
                        {
                            foreach (CELayerEntry CELWE2 in Layer)
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

                foreach (CELayerEntry CELWE in Layer)
                {
                    CELWE.Visited = false;
                }

                SortListByTimestamp(NoDependencies);
                Final.AddRange(NoDependencies.ToArray());
                
                if(Layer.Count == 0) //We've already sorted all necessary effects, no need to walk.
                {
                	break;
                }

                //Walk the tree to detect and take care of circular dependencies
                List<CELayerEntry> CircularDependency = new List<CELayerEntry>();
                if (WalkDependencyTree(Layer[0], CircularDependency))
                {
                    SortListByTimestamp(CircularDependency);
                    Final.AddRange(CircularDependency.ToArray());
                    foreach (CELayerEntry CELWE1 in CircularDependency)
                    {
                        foreach (CELayerEntry CELWE2 in Layer)
                        {
                            CELWE2.Dependants.Remove(CELWE1);
                            CELWE2.DependsOn.Remove(CELWE1);
                        }
                        Layer.Remove(CELWE1);
                    }
                }
            }

            Layer.Clear();
            Layer.AddRange(Final.ToArray());
        }

        //Helper to SortListByDependency, solves dependency loops.
        private bool WalkDependencyTree(CELayerEntry StartPoint, List<CELayerEntry> CircDepList)
        {
            foreach (CELayerEntry CELWE in StartPoint.Dependants)
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
