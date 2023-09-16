using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Elympics;
using System;

public class PlayerProvider : ElympicsMonoBehaviour, IInitializable, IServerHandlerGuid
{
    public PlayerData ClientPlayer { get; private set; } = null;
    public PlayerData[] AllPlayersInScene { get; private set; } = null;

    public ElympicsInt _playerCount = new ElympicsInt();

    public bool IsReady { get; private set; } = false;
    public event Action IsReadyChanged = null;
    
    public void OnServerInit(InitialMatchPlayerDatasGuid initialMatchPlayerDatas)
    {
        _playerCount.Value = initialMatchPlayerDatas.Count;
        Debug.Log(_playerCount.Value);
    }

    public void OnPlayerDisconnected(ElympicsPlayer player) { }

    public void OnPlayerConnected(ElympicsPlayer player) { }
    
    public void Initialize()
    {
        FindAllPlayersInScene();
        FindClientPlayerInScene();
        IsReady = true;
        IsReadyChanged?.Invoke();
    }

    private void FindAllPlayersInScene()
    {
        this.AllPlayersInScene = FindObjectsOfType<PlayerData>().OrderBy(x => x.PlayerId).ToArray();
    }

    private void FindClientPlayerInScene()
    {
        foreach (PlayerData player in AllPlayersInScene)
        {
            if ((int)Elympics.Player == player.PlayerId)
            {
                ClientPlayer = player;
                return;
            }
        }

        //Fix for server side.
        ClientPlayer = AllPlayersInScene[0];
    }

    public PlayerData GetPlayerById(int id)
    {
        return AllPlayersInScene.FirstOrDefault(x => x.PlayerId == id);
    }

    public void UpdatePlayerProvider()
    {
        if (_playerCount.Value == 2)
        {
            AllPlayersInScene[3].gameObject.SetActive(false);
            AllPlayersInScene[2].gameObject.SetActive(false);
        }
        this.AllPlayersInScene = FindObjectsOfType<PlayerData>().OrderBy(x => x.PlayerId).ToArray();
    }
    
}