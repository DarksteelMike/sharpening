using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    internal class InputState
    {
        private Game InvolvedGame;

        private List<CardBase> targets;
        internal List<CardBase> Targets
        {
            get { return targets; }
        }

        private int targetPlayer;
        internal int TargetPlayer
        {
            get { return targetPlayer; }
        }

        private Condition isValidTarget;
        internal Condition IsValidTarget
        {
            get { return isValidTarget; }
        }

        private Condition targetingComplete;
        internal Condition TargetingComplete
        {
            get { return targetingComplete; }
        }

        private string message;
        internal string Message
        {
            get { return message; }
        }

        private bool isDone;
        internal bool IsDone
        {
            get { return isDone; }
        }

        private bool wasCanceled;
        internal bool WasCanceled
        {
            get { return wasCanceled; }
        }

        //Event handlers
        private CardActivationDelegate cardActivationHandler;
        internal CardActivationDelegate CardActivationHandler
        {
            get { return cardActivationHandler; }
        }
        private VoidNoParams player1ActivatedHandler;
        internal VoidNoParams Player1ActivatedHandler
        {
            get { return player1ActivatedHandler; }
        }
        private VoidNoParams player2ActivatedHandler;
        internal VoidNoParams Player2ActivatedHandler
        {
            get { return player2ActivatedHandler; }
        }
        private VoidNoParams canceledHandler;
        internal VoidNoParams CanceledHandler
        {
            get { return canceledHandler; }
        }
        private VoidNoParams priorityPassedHandler;
        internal VoidNoParams PriorityPassedHandler
        {
            get { return priorityPassedHandler; }
        }

        internal InputState(Game g,Condition cond,string msg,CardActivationDelegate CardAct,VoidNoParams P1Act,VoidNoParams P2Act,VoidNoParams Canc,VoidNoParams PrioPassed)
        {
            InvolvedGame = g;

            isValidTarget = cond;
            message = msg;

            targets = new List<CardBase>();
            targetPlayer = -1;

            if (CardAct == null)
            {
                cardActivationHandler = new CardActivationDelegate(this.CardActivation_Handler);
            }
            else
            {
                cardActivationHandler = CardAct;
            }

            if (P1Act == null)
            {
                player1ActivatedHandler = new VoidNoParams(this.Player1Activated_Handler);
            }
            else
            {
                player1ActivatedHandler = P1Act;
            }

            if (P2Act == null)
            {
                player2ActivatedHandler = new VoidNoParams(this.Player2Activated_Handler);
            }
            else
            {
                player2ActivatedHandler = P2Act;
            }

            if (Canc == null)
            {
                canceledHandler = new VoidNoParams(this.Canceled_Handler);
            }
            else
            {
                canceledHandler = Canc;
            }

            if (PrioPassed == null)
            {
                priorityPassedHandler = new VoidNoParams(this.PriorityPassed_Handler);
            }
            else
            {
                priorityPassedHandler = PrioPassed;
            }
        }

        internal void CardActivation_Handler(CardActivationEventArgs e)
        {
            CardBase c = InvolvedGame.FindCard(e.CardID);
            if (isValidTarget(c))
            {
                targets.Add(c);
            }
            if (targetingComplete())
            {
                isDone = true;
            }
        }

        internal void Player1Activated_Handler()
        {
            if (isValidTarget(null, 0))
            {
                targetPlayer = 0;
                isDone = true;
            }
        }

        internal void Player2Activated_Handler()
        {
            if (isValidTarget(null, 1))
            {
                targetPlayer = 1;
                isDone = true;
            }
        }

        internal void Canceled_Handler()
        {
            targetPlayer = -1;
            targets.Clear();
            isDone = true;
            wasCanceled = true;
        }

        internal void PriorityPassed_Handler()
        {
            InvolvedGame.PassPriority();
        }
    }
}
