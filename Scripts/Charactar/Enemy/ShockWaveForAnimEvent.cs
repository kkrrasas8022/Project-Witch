using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveForAnimEvent : MonoBehaviour
{
    BossType1 monster;
    public MonsterSoundMgr soundMgr;
    void Start()
    {
        monster = GetComponentInParent<BossType1>();
    }
    
    public void CreateShockWave()
    {
        soundMgr.PlaySkillSound2();
        monster.Stamp();
    }
}
