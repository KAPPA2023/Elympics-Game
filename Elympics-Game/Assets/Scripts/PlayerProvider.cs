using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Elympics;

public class PlayerProvider : ElympicsMonoBehaviour, IInitializable
{
    public PlayerData[] AllPlayersInScene { get; private set; } = null;
    
    public void Initialize()
    {
        FindAllPlayersInScene();
    }
    
    private void FindAllPlayersInScene()
    {
        this.AllPlayersInScene = FindObjectsOfType<PlayerData>().OrderBy(x => x.PlayerId).ToArray();
    }
    
    public PlayerData GetPlayerById(int id)
    {
        return AllPlayersInScene.FirstOrDefault(x => x.PlayerId == id);
    }
}
