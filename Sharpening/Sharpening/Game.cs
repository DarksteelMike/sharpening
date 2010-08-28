using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal class Game
    {
        private StateBasedEffectSystem stateBasedEffects;
        internal StateBasedEffectSystem StateBasedEffects
        {
        	get { return stateBasedEffects; }
        }
        
        private EffectScheduler schedulingSystem;
        internal EffectScheduler SchedulingSystem
        {
        	get { return schedulingSystem; }
        }
        
        private CELayerSystem layerSystem;
        internal CELayerSystem LayerSystem
        {
        	get { return layerSystem; }
        }
        

        private List<IUserInterfaceBridge> userInterfaces;
        internal List<IUserInterfaceBridge> UserInterfaces
        {
            get { return userInterfaces; }
        }

        private InputStateMachine inputSM;
        internal InputStateMachine InputSM
        {
            get { return inputSM; }
        }

        private List<Player> players;
        internal List<Player> Players
        {
            get { return players; }
        }

        private int priority;
        internal int Priority
        {
            get { return priority; }
        }

        private int whoseTurn;
        internal int WhoseTurn
        {
            get { return whoseTurn; }
        }

        private Phase currentPhase;
        internal Phase CurrentPhase
        {
            get { return currentPhase; }
        }

        private int phaseNum;
        internal int PhaseNum
        {
            get { return phaseNum; }
        }

        private Stack<Spell> spellStack;

        internal void PushSpell(Spell s)
        {
            spellStack.Push(s);
            foreach (IUserInterfaceBridge UI in userInterfaces)
            {
                UI.PushOnStack(s.Description, s.CardSrc.CardID);
            }
        }
        internal void PopSpell()
        {
            spellStack.Pop();
        }

        //Used to continue along the phase ring if both players pass
        private Stack<string> ActionStack;

        internal Game(List<IUserInterfaceBridge> UI)
        {
            spellStack = new Stack<Spell>();
            ActionStack = new Stack<string>();
            userInterfaces = new List<IUserInterfaceBridge>();
            userInterfaces.AddRange(UI.ToArray());

            stateBasedEffects = new StateBasedEffectSystem(this);
            schedulingSystem = new EffectScheduler();
            layerSystem = new CELayerSystem(this);

            players = new List<Player>();
            players.Add(new Player(0,this));
            players.Add(new Player(1,this));

            priority = whoseTurn = 0;

            currentPhase = Phase.Main1;
            
            Effect UntapEffect = new Effect(delegate (object[] param) 
                                            {
                                            	foreach(CardBase Card in players[whoseTurn].BattlefieldCards)
                                                {
                                                	if(Card.AutoUntaps)
                                                    {
                                                    	Card.CurrentCharacteristics.IsTapped = false;
                                                    }
                                                }
                                            });
            
            Effect DrawEffect = new Effect(delegate(object[] param)
                                           {
                                           		//Draw code here.
                                           });
            Effect CleanupEffect = new Effect(delegate(object[] param)
                                              {
                                              	while(players[whoseTurn].HandCards.Count > players[whoseTurn].MaximumHandSize)
                                              	{
                                              		//Set the correct input state for discard.
                                              	}
                                              });
            EffectSchedulerEntry UntapESC1 = new EffectSchedulerEntry(UntapEffect, null, players[0], Phase.Untap, PhasePart.End,false);
            EffectSchedulerEntry UntapESC2 = new EffectSchedulerEntry(UntapEffect, null, players[1], Phase.Untap, PhasePart.End,false);
            EffectSchedulerEntry DrawESC1 = new EffectSchedulerEntry(DrawEffect, null, players[0], Phase.Draw, PhasePart.Beginning,false);
            EffectSchedulerEntry DrawESC2 = new EffectSchedulerEntry(DrawEffect, null, players[1], Phase.Draw, PhasePart.Beginning,false);
            EffectSchedulerEntry CleanupESC1 = new EffectSchedulerEntry(CleanupEffect, null, players[0], Phase.Cleanup, PhasePart.Beginning,false);
            EffectSchedulerEntry CleanupESC2 = new EffectSchedulerEntry(CleanupEffect, null, players[1], Phase.Cleanup, PhasePart.Beginning,false);
            
            schedulingSystem.AddEntry(UntapESC1);
            schedulingSystem.AddEntry(UntapESC2);
            schedulingSystem.AddEntry(DrawESC1);
            schedulingSystem.AddEntry(DrawESC2);
            schedulingSystem.AddEntry(CleanupESC1);
            schedulingSystem.AddEntry(CleanupESC2);
        }

        internal void PassPriority()
        {
            stateBasedEffects.Run();

            priority = 1 - priority;
            if (ActionStack.Peek() == "Pass" || ActionStack.Count == 0)
            {
                if (spellStack.Count == 0)
                {
                    ActionStack.Clear();
                    Array a = Enum.GetValues(typeof(Phase));
                    phaseNum++;
                    if (phaseNum == a.Length)
                    {
                        phaseNum = 0;
                        whoseTurn = 1 - whoseTurn;
                    }

                    schedulingSystem.LeavingPhase(currentPhase,players[whoseTurn]);
                    currentPhase = (Phase)a.GetValue(phaseNum);
                    schedulingSystem.EnteredPhase(currentPhase,players[whoseTurn]);
                    //Both players passed in succession,go to next phase
                }
                else
                {                	
                	Spell popped = spellStack.Pop();
                	if(popped.CanResolve())
                	{
                		popped.Resolve();
                	}
                }
            }
            else
            {
                ActionStack.Push("Pass");
            }
        }

        private void CardActivation_Handler(CardActivationEventArgs e)
        {
            CardBase card = FindCard(e.CardID);
            
            //TODO: Handle abilities that any player can activate here
            //foreach(
        }

        private void PriorityPassing_Handler()
        {
            PassPriority();
        }

        internal CardBase FindCard(int CardID)
        {
            foreach (Player p in players)
            {
                foreach (CardBase c in p.OwnedCards)
                {
                    if (c.CardID == CardID)
                    {
                        return c;
                    }
                }
            }

            return null;
        }
    }
}
