/*
 * Created by SharpDevelop.
 * User: Elev
 * Date: 2010-08-12
 * Time: 16:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Sharpening.Cards
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	internal class GloriousAnthem : CardBase
	{
		private CELayerEntry PTChanging;
		
        /*
         * Glorious Anthem
         * 1 W W
         * Enchantment
         * Creatures you control get +1/+1.
         * 10E Rare, 9E Rare, 8E Rare, 7E Rare, USG Rare
         * */
		internal GloriousAnthem(Game g): base(g)
		{
			this.name = "Glorious Anthem";
			this.baseCharacteristics.Types.Add("Enchantment");
			
			this.activatables.Add(ActivatableFactory.Create(g,this,"CastPermanent:1 W W"));
            
			
			//Creatures you control get +1/+1 (Layer7C)
			PTChanging = new CELayerEntry();
			PTChanging.DependsFields.Add(typeof(CharacteristicsCollection).GetField("Types"));
			PTChanging.CardTgt = null;
			PTChanging.CardSrc = this;
			PTChanging.AppliesTo = new Condition(delegate(object[] param) {
			                                     	return ((CardBase)param[0]).CurrentCharacteristics.Types.Contains("Creature") && ((CardBase)param[0]).CurrentCharacteristics.Controller == this.currentCharacteristics.Controller;
			                                     });
			PTChanging.TargetFields.Add(typeof(CharacteristicsCollection).GetField("Power"));
			PTChanging.TargetFields.Add(typeof(CharacteristicsCollection).GetField("Toughness"));
			
			PTChanging.MyEffect = new Effect(delegate(object[] param) {
			                                 	((CardBase)param[0]).CurrentCharacteristics.Power++;
			                                 	((CardBase)param[0]).CurrentCharacteristics.Toughness++;
			                                 });
			
			this.EntersBattlefield = new ReplacableEvent(new Effect(delegate(object[] param) {
			                                                        	InvolvedGame.LayerSystem.AddEntry(CELayer.Layer7C,PTChanging);
			                                                        }));
			
			this.LeavesBattlefield = new ReplacableEvent(new Effect(delegate(object[] param) {
			                                                        	InvolvedGame.LayerSystem.RemoveAllEntriesFromCard(this);
			                                                        }));
		}
	}
}
