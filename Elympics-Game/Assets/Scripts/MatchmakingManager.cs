using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class MatchmakingManager : MonoBehaviour
{
    public void PlayOnline()
    {
        ElympicsLobbyClient.Instance.PlayOnlineInRegion(null, null,null, "Default");
    }
}
