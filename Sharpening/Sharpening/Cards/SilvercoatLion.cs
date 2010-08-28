/*
 * Created by SharpDevelop.
 * User: Elev
 * Date: 2010-08-13
 * Time: 10:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Sharpening.Cards
{
	/// <summary>
	/// Description of SilvercoatLion.
	/// </summary>
	internal class SilvercoatLion : CardBase
	{
		internal SilvercoatLion(Game g) : base(g)
		{
			this.name = "Silvercoat Lion";
			this.baseCharacteristics.Color.Add("White");
			this.baseCharacteristics.Types.Add("Creature");
			this.baseCharacteristics.Subtypes.Add("Cat");
			this.baseCharacteristics.Power = 2;
			this.baseCharacteristics.Toughness = 2;
			
			this.activatables.Add(ActivatableFactory.Create(InvolvedGame,this,"CastPermanent:1 W"));
		}
	}
}
