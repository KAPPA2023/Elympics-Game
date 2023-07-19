using Elympics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInitializer : ElympicsMonoBehaviour, IInitializable
{
    public void Initialize()
    {
        var playerData = GetComponent<PlayerData>();

        InitializeCameras(playerData);
    }

    private void InitializeCameras(PlayerData playerData)
    {
        var camerasInChildren = GetComponentsInChildren<Camera>();

        bool enableCamera = false;

        if (Elympics.IsClient)
            enableCamera = (int)Elympics.Player == playerData.PlayerId;
        else if (Elympics.IsServer)
            enableCamera = playerData.PlayerId == 0;

        foreach (Camera camera in camerasInChildren)
        {
            camera.enabled = enableCamera;

            if (camera.TryGetComponent<AudioListener>(out AudioListener audioListener))
            {
                Destroy(audioListener);
            }
        }
    }
}
