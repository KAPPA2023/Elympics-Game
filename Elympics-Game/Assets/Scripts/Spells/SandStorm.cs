using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class SandStorm : ElympicsMonoBehaviour, IUpdatable
{
    private int owner;
    [SerializeField] protected float lifeTime;

    private ElympicsFloat deathTimer = new ElympicsFloat(0.0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void SpawnSpell(Vector3 position,int client)
    {
        //we can use tick to setup timers - for example fireball could explode after 2 seconds in air
        transform.position = position;
        owner = client;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        var playerInfo = other.GetComponent<PlayerData>();
        if (playerInfo != null)
        {
            //SpellHit?.Invoke();
            playerInfo.DealDamage(2,owner);
        }
    }
    public void ElympicsUpdate()
    {
        deathTimer.Value += Elympics.TickDuration;
        if (deathTimer >= lifeTime)
        {
            ElympicsDestroy(gameObject);
        }
    }
}
