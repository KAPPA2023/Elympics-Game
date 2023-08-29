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
        playerData.GetComponent<DeathController>().IsDead.ValueChanged += OnDeath;
        if ((int)Elympics.Player == playerData.PlayerId)
        {
            playerData.GetComponentInChildren<SpellSpawner>().SpellHit += OnEnemyDamaged;
        }
    }

    private void OnDeath(bool oldVal, bool newVal)
    {
        audioSource.spatialBlend = 1f;
        audioSource.PlayOneShot(clips[1]);
    }
    
    private void OnEnemyDamaged()
    {
        audioSource.spatialBlend = 0f;
        audioSource.PlayOneShot(clips[0],0.1f);
    }
}
