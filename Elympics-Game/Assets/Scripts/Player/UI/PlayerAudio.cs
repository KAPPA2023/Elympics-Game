using System;
using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class PlayerAudio : ElympicsMonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private List<AudioClip> clips;

    private void Awake()
    {
        playerData.GetComponent<DeathController>().GetComponent<StatsController>()._health.ValueChanged += OnDamaged;
        if ((int)Elympics.Player == playerData.PlayerId)
        {
            playerData.GetComponentInChildren<SpellSpawner>().SpellHit += OnEnemyDamaged;
        }
    }

    private void OnDamaged(float oldVal, float newVal)
    {
        if (newVal < oldVal)
        {
            audioSource.spatialBlend = 1f;
            audioSource.PlayOneShot(clips[1]);
        }
    }
    
    private void OnEnemyDamaged()
    {
        audioSource.spatialBlend = 0f;
        audioSource.PlayOneShot(clips[0],0.1f);
    }
}
