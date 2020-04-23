using UnityEngine;
using System;
using System.Collections;

public class Ball : MonoBehaviour
{
    public static event Action<Ball> onDeath;
    public static event Action<Ball> lightningEnable;
    public static event Action<Ball> lightningDisable;

    public bool isLightning;
    public ParticleSystem lightningEffect;

    private MeshRenderer mr;
    private float lightningEffectDuration = 10;

    private void Awake()
    {
        this.mr = GetComponent<MeshRenderer>();
    }

    public void Die()
    {
        onDeath?.Invoke(this);
        Destroy(gameObject, 1);
    }

    public void StartLightningBall()
    {
        if (!this.isLightning)
        {
            this.isLightning = true;
            this.mr.enabled = false;
            lightningEffect.gameObject.SetActive(true);
            StartCoroutine(StopLightingEffect(this.lightningEffectDuration));
            lightningEnable?.Invoke(this);
        }
    }

    private IEnumerator StopLightingEffect(float time)
    {
        yield return new WaitForSeconds(time);
        if (this.isLightning)
        {
            this.isLightning = false;
            this.mr.enabled = true;
            lightningEffect.gameObject.SetActive(false);
            lightningDisable?.Invoke(this);
        }
    }
}
