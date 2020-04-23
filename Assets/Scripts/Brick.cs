using UnityEngine;
using System;
using static UnityEngine.ParticleSystem;
using System.Collections.Generic;

public class Brick : MonoBehaviour
{
    public int hitpoints = 1;
    public ParticleSystem destroyEffect;
    public ParticleSystemRenderer destroyEffect_rn;
    public static event Action<Brick> brickDestroy;
    private MeshRenderer _mr;
    private BoxCollider brickColl;
    public AudioSource tick;
    public AudioClip tickOnColl;
    
    private void Awake()
    {
        _mr = GetComponent<MeshRenderer>();
        
        Ball.lightningEnable += lightningEnable;
        Ball.lightningDisable += lightningDisable;
        brickColl = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        tick = GetComponent<AudioSource>();
    }

    private void lightningDisable(Ball obj)
    {
        if (this != null)
        {
            this.brickColl.isTrigger = false;
        }
    }

    private void lightningEnable(Ball obj)
    {
        if(this != null)
        {
            this.brickColl.isTrigger = true;
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        AudioSource.PlayClipAtPoint(tickOnColl, transform.position, 0.2f);
        Ball ball = coll.gameObject.GetComponent<Ball>();
        ApplyCollision(ball);
    }

    private void OnTriggerEnter(Collider coll)
    {
        AudioSource.PlayClipAtPoint(tickOnColl, transform.position, 0.2f);
        Ball ball = coll.gameObject.GetComponent<Ball>();
        ApplyCollision(ball);
    }

    private void ApplyCollision(Ball ball)
    {
        
        this.hitpoints--;
        if (this.hitpoints <= 0 || (ball != null && ball.isLightning))
        {
            BrickManager.Instance.RemainingBricks.Remove(this);
            brickDestroy?.Invoke(this);
            SpawnDestroyEffect();
            GeneratePowerup();
            Destroy(this.gameObject);
        }
        else
        {
            this._mr.material = BrickManager.Instance.mats[this.hitpoints - 1];
        }
    }

    private void GeneratePowerup()
    {
        float buffChance = UnityEngine.Random.Range(0, 100f);
        float debuffChance = UnityEngine.Random.Range(0, 100f);
        bool spawned = false;
        if(buffChance <= PowerUpsManager.Instance.BuffChance)
        {
            spawned = true;
            Powerup newBuff = this.SpawnPower(true);
        }
        if(debuffChance <= PowerUpsManager.Instance.DebuffChance && !spawned)
        {
            spawned = true;
            Powerup newDebuff = this.SpawnPower(false);
        }
    }

    private Powerup SpawnPower(bool isBuff)
    {
        List<Powerup> list;
        if (isBuff)
        {
            list = PowerUpsManager.Instance.Buffs;
        }
        else
        {
            list = PowerUpsManager.Instance.Debuffs;
        }

        int powerIndex = UnityEngine.Random.Range(0, list.Count);
        Powerup prefab = list[powerIndex];
        Powerup newPower = Instantiate(prefab, this.transform.position, Quaternion.identity) as Powerup;

        return newPower;

    }

    private void SpawnDestroyEffect()
    {
        
        Vector3 brick = transform.position;
        Vector3 spawnPos = new Vector3(brick.x, brick.y, brick.z - 0.2f);
        GameObject effect = Instantiate(destroyEffect.gameObject, spawnPos, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        destroyEffect_rn = effect.GetComponent<ParticleSystemRenderer>();
        destroyEffect_rn.material.color = this._mr.material.color;
        Destroy(effect, destroyEffect.main.startLifetime.constant);

    }
    
    public void Init(Transform container, Material mat, Color color, int hitPoints)
    {
        this.transform.SetParent(container);
        this._mr.material = mat;
        this._mr.material.color = color;
        this.hitpoints = hitPoints;
    }

    private void OnDisable()
    {
        Ball.lightningEnable -= lightningEnable;
        Ball.lightningDisable -= lightningDisable;
    }

}
