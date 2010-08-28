/*
 * Created by SharpDevelop.
 * User: Elev
 * Date: 2010-08-13
 * Time: 07:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Sharpening.Cards
{
	/// <summary>
	/// Description of Opalescence.
	/// </summary>
	internal class Opalescence : CardBase
	{
		private CELayerEntry TypeChanging;
		private CELayerEntry PTChanging;
		
		internal Opalescence(Game g): base(g)
		{
			this.BaseCharacteristics.Types.Add("Enchantment");
			this.name = "Opalescence";
			
			this.activatables.Add(ActivatableFactory.Create(InvolvedGame,this,"CastPermanent:2 W W"));
			
			//Each other enchantment is a creature. (Layer 4)
			TypeChanging = new CELayerEntry();
			TypeChanging.DependsFields.Add(typeof(CharacteristicsCollection).GetField("Types"));
			TypeChanging.TargetFields.Add(typeof(CharacteristicsCollection).GetField("Types"));
			TypeChanging.CardTgt = null;
			TypeChanging.CardSrc = this;
			TypeChanging.AppliesTo = new Condition(delegate(object[] param) {
			                                                   	return ((CardBase)param[0]).CurrentCharacteristics.Types.Contains("Enchantment") && ((CardBase)param[0]).CardID != this.CardID;
			                                                   });
			TypeChanging.MyEffect = new Effect(delegate(object[] param) {
			                                               	((CardBase)param[0]).CurrentCharacteristics.Types.Add("Creature");
			                                               });
			
			//with power and toughness each equal to it's converted mana cost. (Layer 7B?)
			PTChanging = new CELayerEntry();
			PTChanging.DependsFields.Add(typeof(CharacteristicsCollection).GetField("Types"));
			PTChanging.TargetFields.Add(typeof(CharacteristicsCollection).GetField("Power"));
			PTChanging.TargetFields.Add(typeof(CharacteristicsCollection).GetField("Toughness"));
			PTChanging.CardTgt = null;
			PTChanging.CardSrc = this;
			PTChanging.AppliesTo = new Condition(delegate(object[] param) {
			                                                   	return ((CardBase)param[0]).CurrentCharacteristics.Types.Contains("Enchantment") && ((CardBase)param[0]).CardID != this.CardID;
			                                                   });
			PTChanging.MyEffect = new Effect(delegate(object[] param) {
			                                             	((CardBase)param[0]).CurrentCharacteristics.Power = ((CardBase)param[0]).Activatables[0].CurrentCost.Converted();
			                                             	((CardBase)param[0]).CurrentCharacteristics.Toughness = ((CardBase)param[0]).Activatables[0].CurrentCost.Converted();
			                                               });
			
			this.EntersBattlefield = new ReplacableEvent(new Effect(delegate(object[] param) {
			                                                        	InvolvedGame.LayerSystem.AddEntry(CELayer.Layer4,TypeChanging);
			                                                        	InvolvedGame.LayerSystem.AddEntry(CELayer.Layer7B,PTChanging);
			                                                        }));
			
			this.LeavesBattlefield = new ReplacableEvent(new Effect(delegate(object[] param) {
			                                                        	InvolvedGame.LayerSystem.RemoveAllEntriesFromCard(this);
			                                                        }));
		}
	}
}
