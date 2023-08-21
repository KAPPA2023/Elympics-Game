using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerData))]
public class PlayerVisibilityController : ElympicsMonoBehaviour, IInitializable
{
    [Header("References:")]
    [SerializeField] private GameObject[] firstPersonRendererRoots = null;
    [SerializeField] private GameObject[] thirdPersonRendererRoots = null;
    // Start is called before the first frame update

    public void Initialize()
    {
        var playerData = GetComponent<PlayerData>();
        if (playerData.PlayerId != (int)Elympics.Player)
        {
            ProcessRootsOfRenderersToSetGivenLayer(firstPersonRendererRoots, "Invisible");
            ProcessRootsOfRenderersToSetGivenLayer(thirdPersonRendererRoots, "Default");
        }
        else
        {
            ProcessRootsOfRenderersToSetGivenLayer(thirdPersonRendererRoots, "Invisible");
        }

        if (Elympics.IsServer)
        {
            if (playerData.PlayerId == 0)
            {
                ProcessRootsOfRenderersToSetGivenLayer(thirdPersonRendererRoots, "Invisible");
            }
        }
    }
    
    private void ProcessRootsOfRenderersToSetGivenLayer(GameObject[] rootsOfRenderersToDisable, string layerName)
    {
        foreach (GameObject rootOfRenderersToDisable in rootsOfRenderersToDisable)
        {
            var rendererObjectsInChildren = Array.ConvertAll(rootOfRenderersToDisable.GetComponentsInChildren<Renderer>(true), x => x.gameObject);
            SetGivenObjectsLayer(rendererObjectsInChildren, layerName);

            rendererObjectsInChildren = Array.ConvertAll(rootOfRenderersToDisable.GetComponentsInChildren<Graphic>(true), x => x.gameObject);
            SetGivenObjectsLayer(rendererObjectsInChildren, layerName);
        }
    }

    private void SetGivenObjectsLayer(GameObject[] objectsToChangeLayer, string layerName)
    {
        foreach (GameObject objectToChangeLayer in objectsToChangeLayer)
        {
            objectToChangeLayer.layer = LayerMask.NameToLayer(layerName);
        }
    }
}
