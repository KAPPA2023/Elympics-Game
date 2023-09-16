using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerVote : ElympicsMonoBehaviour
{
    public ElympicsArray<ElympicsBool> votes = new ElympicsArray<ElympicsBool>(3, () => new ElympicsBool());
    public ElympicsInt selectedCard = new ElympicsInt(0);
    private float previousDir = 0;
    public void HandleInput(bool spacePressed, float x)
    {
        if (previousDir != x)
        {
            switch (x)
            {
                case >= 1f:
                    ChangeSelect(1);
                    break;
                case <= -1:
                    ChangeSelect(-1);
                    break;
            }
        }
        previousDir = x;

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
