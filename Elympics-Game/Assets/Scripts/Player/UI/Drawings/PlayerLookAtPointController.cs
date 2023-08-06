using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookAtPointController : MonoBehaviour
{
    public Transform playerCamera;
    public Vector3 scale;
    void LateUpdate()
    {
        if (playerCamera != null)
        {
            transform.position = playerCamera.position + playerCamera.forward;
            transform.rotation = playerCamera.rotation;
            transform.localScale = scale;
        }
    }
}
