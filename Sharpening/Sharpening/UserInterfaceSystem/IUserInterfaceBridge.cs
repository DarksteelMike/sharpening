using System;
using System.Collections.Generic;

using System.Text;

namespace Sharpening
{
    public delegate void CardActivationDelegate(CardActivationEventArgs e);
    public delegate void VoidNoParams();
    public interface IUserInterfaceBridge
    {
        void AssociateCardIDWithName(int CardID, string Name);
        void SetLifePlayer1(int value);
        void SetLifePlayer2(int value);

        void RemoveCardFrom(int CardID, CardLocation Location);
        void AddCardTo(int CardID, CardLocation Location);

        void SetMessage(string Message);

        void PushOnStack(string Message, int srcCardID);
        void PopOffStack();

        void SetCanCancel(bool CanCancel);

        event CardActivationDelegate CardActivation;
        event VoidNoParams Player1Activated;
        event VoidNoParams Player2Activated;
        event VoidNoParams PriorityPassed;
        event VoidNoParams Canceled;
    }
}
