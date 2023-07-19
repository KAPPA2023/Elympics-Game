using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public abstract class Spell : ElympicsMonoBehaviour, IUpdatable
{
    [SerializeField] protected float spellDamage;
    [SerializeField] protected float spellSpeed;
    private Vector3 spellVelocity;
    private ElympicsBool shouldBeDestoyed = new ElympicsBool();
    //if spell exploded on collision or smth
    //[SerializeField] private float spellRange;

    public void spawnSpell(Vector3 position, Vector3 direction, long tick)
    {
        //we can use tick to setup timers - for example fireball could explode after 2 seconds in air
        transform.position = position;
        spellVelocity = direction.normalized * spellSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerInfo = other.GetComponentInChildren<PlayerInfo>();
        if (playerInfo != null)
        {
            playerInfo.DealDamage(spellDamage);
        }
        // if (other.TryGetComponent<PlayerInfo>(out var hitPlayer))
        // {
        //     Debug.Log(other.name);
        //     hitPlayer.DealDamage(spellDamage);
        // }
        shouldBeDestoyed.Value = true;
    }

    public void ElympicsUpdate()
    {
        if(shouldBeDestoyed.Value) ElympicsDestroy(gameObject);
        transform.position += spellVelocity;
    }
}
