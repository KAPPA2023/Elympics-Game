using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Elympics;

public class PlayerSpawner : ElympicsMonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    
    public static PlayerSpawner Instance = null;
    
    private void Awake()
    {
        if (PlayerSpawner.Instance == null)
            PlayerSpawner.Instance = this;
        else
            Destroy(this);
    }
    
    public void SpawnPlayer(PlayerData player)
    {
        //TODO: add spawning logic so player can't spawn next to other player
        Vector3 spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)].position;

        player.transform.position = spawnPoint;
    }
}
