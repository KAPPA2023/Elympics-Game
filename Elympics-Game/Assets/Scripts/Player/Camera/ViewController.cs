using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] private Transform verticalRotationTarget = null;
    [SerializeField] private Transform horizontalRotationTarget = null;


    public void ProcessView(Quaternion mouseRotation)
    {
        horizontalRotationTarget.localRotation = Quaternion.Euler(0, mouseRotation.eulerAngles.y, 0);
        verticalRotationTarget.localRotation = Quaternion.Euler(mouseRotation.eulerAngles.x, 0, 0);
    }
}
