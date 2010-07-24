using System;
using System.Collections.Generic;

using System.Text;

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
                foreach (CELayerEntry Entry in LayerMap[LayerKey])
                {
                    if (Entry.CardSrc.CardID == Card.CardID)
                    {
                        LayerMap[LayerKey].Remove(Entry);
                    }
                }
            }
        }

        //Bubblesorts by timestamp
        private void SortLayer(List<CELayerEntry> Layer)
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

        //Bubblesorts by timestamp,prioritizing CDA effects
        private void SortLayerCDA(List<CELayerEntry> Layer)
        {
            List<CELayerEntry> CDAEffects = new List<CELayerEntry>();
            List<CELayerEntry> OtherEffects = new List<CELayerEntry>();

            foreach (CELayerEntry Entry in Layer)
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
                        CELayerEntry tmp = CDAEffects[i];
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
                        CELayerEntry tmp = OtherEffects[i];
                        OtherEffects[i] = OtherEffects[i + 1];
                        OtherEffects[i + 1] = tmp;
                    }
                }
            } while (HasChanged);

            Layer.Clear();

            Layer.AddRange(CDAEffects);
            Layer.AddRange(OtherEffects);
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
    }
}
