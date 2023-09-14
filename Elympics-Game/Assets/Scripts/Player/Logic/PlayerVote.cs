using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerVote : ElympicsMonoBehaviour
{
    public ElympicsBool vote = new ElympicsBool(false);
    public void HandleInput(bool spacePressed)
    {
        if (spacePressed)
        {
            vote.Value = !vote.Value;
        }
    }
}
