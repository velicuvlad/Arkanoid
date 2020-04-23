using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    public AudioSource tick;
    public AudioClip tickOnColl;

    private void Start()
    {
        tick = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(tickOnColl, transform.position, 0.2f);
            this.ApplyEfect();
        }
        if (coll.tag == "Player" || coll.tag == "Deathwall")
        {
            Destroy(this.gameObject);
        }
    }

    protected abstract void ApplyEfect();
    
}
