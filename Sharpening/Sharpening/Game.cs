using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal class Game
    {
        StateBasedEffectSystem SBESystem;
        EffectScheduler SchedulingSystem;
        CELayerSystem LayerSystem;

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

            SBESystem = new StateBasedEffectSystem(this);
            SchedulingSystem = new EffectScheduler();
            LayerSystem = new CELayerSystem(this);

            players = new List<Player>();
            players.Add(new Player(0,this));
            players.Add(new Player(1,this));

            priority = whoseTurn = 0;

            currentPhase = Phase.Main1;
        }

        internal void PassPriority()
        {
            SBESystem.Run();

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

                    currentPhase = (Phase)a.GetValue(phaseNum);
                    //Both players passed in succession,go to next phase
                }
                else
                {
                    spellStack.Pop().Resolve();
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
