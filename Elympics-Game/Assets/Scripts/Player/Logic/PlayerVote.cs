using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerVote : ElympicsMonoBehaviour, IUpdatable
{
    public ElympicsArray<ElympicsBool> votes = new ElympicsArray<ElympicsBool>(3, () => new ElympicsBool());
    public ElympicsInt selectedCard = new ElympicsInt(0);
    
    protected ElympicsFloat currentTime = new ElympicsFloat();
    protected bool canSwitch => currentTime.Value <= 0.0f;
    
    
    public void ElympicsUpdate()
    {
        if (!canSwitch)
        {
            currentTime.Value -= Elympics.TickDuration;
        }    
    }
    
    
    public void HandleInput(bool spacePressed, float x)
    {
        if (canSwitch)
        {
            switch (x)
            {
                case >= 1f:
                    ChangeSelect(1);
                    currentTime.Value = 0.2f;
                    break;
                case <= -1:
                    ChangeSelect(-1);
                    currentTime.Value = 0.2f;
                    break;
            }
        }

        if (spacePressed)
        {
            votes.Values[selectedCard.Value].Value = !votes.Values[selectedCard.Value].Value;
        }
    }


    private void ChangeSelect(int x)
    {
        if ((selectedCard.Value + x) > 2)
        {
            selectedCard.Value = 0;
        }
        else if ((selectedCard.Value + x) < 0)
        {
            selectedCard.Value = 2;
        }
        else
        {
            selectedCard.Value += x;
        }
    }
    
}
