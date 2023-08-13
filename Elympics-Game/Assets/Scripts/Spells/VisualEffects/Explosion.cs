using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;
using UnityEngine.VFX;

public class Explosion : ElympicsMonoBehaviour
{
    [Header("Parameters:")]
    [SerializeField] private float explosionDamage = 10.0f;
    [SerializeField] private float explosionRange = 2.0f;

    [Header("References:")]
    [SerializeField] private VisualEffect explosionPS = null;
    [SerializeField] private ElympicsMonoBehaviour bulletOwner = null;

    public void Detonate()
    {
        DetectTargetsInExplosionRange();

        explosionPS.Play();
    }

    private void DetectTargetsInExplosionRange()
    {
        Collider[] objectsInExplosionRange = Physics.OverlapSphere(this.transform.position, explosionRange);

        foreach (Collider objectInExplosionRange in objectsInExplosionRange)
        {
            if (TargetIsNotBehindObstacle(objectInExplosionRange.transform.root.gameObject))
                TryToApplyDamageToTarget(objectInExplosionRange.transform.root.gameObject);
        }
    }

    private void TryToApplyDamageToTarget(GameObject objectInExplosionRange)
    {
        //Damage to apply here!
        Debug.Log("Apply damage to: " + objectInExplosionRange.gameObject.name);
    }

    private bool TargetIsNotBehindObstacle(GameObject objectInExplosionRange)
    {
        var directionToObjectInExplosionRange = (objectInExplosionRange.transform.position - this.transform.position).normalized;

        if (Physics.Raycast(this.transform.position, directionToObjectInExplosionRange, out RaycastHit hit, explosionRange))
        {
            return hit.transform.gameObject == objectInExplosionRange;
        }

        return false;
    }
}
