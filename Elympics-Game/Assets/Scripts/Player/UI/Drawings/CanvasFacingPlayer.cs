using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFacingPlayer : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] private PlayerProvider playerProvider;

    public void Awake()
    {
        if (playerProvider.IsReady)
        {
            HideCanvasOfOtherPlayers();
        }
        else
        {
            playerProvider.IsReadyChanged += HideCanvasOfOtherPlayers;
        }
    }

    private void HideCanvasOfOtherPlayers()
    {
        if (GetComponentInParent<PlayerData>().PlayerId != playerProvider.ClientPlayer.PlayerId)
        {
            this.gameObject.SetActive(false);
        }
    }
    

    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position;
            transform.rotation = playerTransform.rotation;
            transform.localScale = playerTransform.localScale;
        }
    }
}
