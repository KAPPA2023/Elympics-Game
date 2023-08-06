using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFacingPlayer : MonoBehaviour
{
    public Transform playerTransform;
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
