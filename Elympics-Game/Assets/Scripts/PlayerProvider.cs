using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Elympics;
using System;

public class PlayerProvider : ElympicsMonoBehaviour, IInitializable
{
    public PlayerData ClientPlayer { get; private set; } = null;
    public PlayerData[] AllPlayersInScene { get; private set; } = null;

    public bool IsReady { get; private set; } = false;
    public event Action IsReadyChanged = null;

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
        Debug.Log(AllPlayersInScene.Length);
        return AllPlayersInScene.FirstOrDefault(x => x.PlayerId == id);
    }
}