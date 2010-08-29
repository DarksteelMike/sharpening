using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    class InputStateMachine
    {
        private Game InvolvedGame;

        private InputState standardState;
        internal InputState StandardState
        {
            get { return standardState; }
        }

        private InputState currentState;
        internal InputState CurrentState
        {
            get { return currentState; }
            set
            {
                if (currentState != null)
                {
                    foreach (IUserInterfaceBridge UI in InvolvedGame.UserInterfaces)
                    {
                        UI.CardActivation -= currentState.CardActivationHandler;
                        UI.Player1Activated -= currentState.Player1ActivatedHandler;
                        UI.Player2Activated -= currentState.Player2ActivatedHandler;
                        UI.Canceled -= currentState.CanceledHandler;
                        UI.PriorityPassed -= currentState.PriorityPassedHandler;
                    }
                }
                currentState = value;
                foreach (IUserInterfaceBridge UI in InvolvedGame.UserInterfaces)
                {
                    UI.CardActivation += currentState.CardActivationHandler;
                    UI.Player1Activated += currentState.Player1ActivatedHandler;
                    UI.Player2Activated += currentState.Player2ActivatedHandler;
                    UI.Canceled += currentState.CanceledHandler;
                    UI.PriorityPassed += currentState.PriorityPassedHandler;
                    UI.SetMessage(currentState.Message);
                }
            }
        }

        internal InputStateMachine(Game g,InputState s)
        {
            InvolvedGame = g;
            standardState = CurrentState = s;
        }
        
        internal void Run(){
        	while(!CurrentState.IsDone)
        	{}
        }
    }
}
