using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Elympics;
using MatchTcpClients.Synchronizer;
using TMPro;
using UnityEngine;

public class ClientInfo : ElympicsMonoBehaviour,IClientHandlerGuid
{
    [SerializeField] private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStandaloneClientInit(InitialMatchPlayerDataGuid data)
    {
    }

    public void OnClientsOnServerInit(InitialMatchPlayerDatasGuid data)
    {
    }

    public void OnConnected(TimeSynchronizationData data)
    {
    }

    public void OnConnectingFailed()
    {
    }

    public void OnDisconnectedByServer()
    {
    }

    public void OnDisconnectedByClient()
    {
    }

    public void OnSynchronized(TimeSynchronizationData data)
    {
        text.text = $"ping: {data.RoundTripDelay.Milliseconds}ms";
    }

    public void OnAuthenticated(Guid userId)
    {
    }

    public void OnAuthenticatedFailed(string errorMessage)
    {
    }

    public void OnMatchJoined(Guid matchId)
    {
    }

    public void OnMatchJoinedFailed(string errorMessage)
    {
    }

    public void OnMatchEnded(Guid matchId)
    {
    }
}
