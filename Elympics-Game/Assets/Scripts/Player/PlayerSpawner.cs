using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Elympics;

public class PlayerSpawner : ElympicsMonoBehaviour, IInitializable
{
    [SerializeField] private float safeRadius = 3f;
    [SerializeField] private Transform[] spawnPoints;
    
    private System.Random random = null;
    
    public static PlayerSpawner Instance = null;
    
    private void Awake()
    {
        if (PlayerSpawner.Instance == null)
            PlayerSpawner.Instance = this;
        else
            Destroy(this);
    }
    
    public void Initialize()
    {
        if (!Elympics.IsServer)
            return;
        random = new System.Random();
    }
    
    public void SpawnPlayer(PlayerData player)
    {
        Vector3 spawnPoint = GetSpawnPointWithoutPlayersInRange().position;
        player.transform.position = spawnPoint;
    }
    
    private Transform GetSpawnPointWithoutPlayersInRange()
    {
        var randomizedSpawnPoints = GetRandomizedSpawnPoints();
        Transform chosenSpawnPoint = null;

        foreach (Transform spawnPoint in randomizedSpawnPoints)
        {
            chosenSpawnPoint = spawnPoint;

            Collider[] objectsInRange = Physics.OverlapSphere(chosenSpawnPoint.position, safeRadius);

            if (!objectsInRange.Any(x => x.transform.root.gameObject.TryGetComponent<PlayerData>(out _)))
                break;
        }

        return chosenSpawnPoint;
    }
    
    private IOrderedEnumerable<Transform> GetRandomizedSpawnPoints()
    {
        return spawnPoints.OrderBy(x => random.Next());
    }
}
