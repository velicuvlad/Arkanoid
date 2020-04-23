using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    #region Singleton
    private static PowerUpsManager _instance;

    public static PowerUpsManager Instance => _instance;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public List<Powerup> Buffs;
    public List<Powerup> Debuffs;

    [Range(0,100)]
    public float BuffChance;
    [Range(0,100)]
    public float DebuffChance;
}
