using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCtrlForAnimEvent : MonoBehaviour
{
    BossType1 monster;
    GameObject rock;
    public MonsterSoundMgr soundMgr;
    void Start()
    {
        monster = GetComponentInParent<BossType1>();
    }

    void Update()
    {
        
    }

    public void CreatRock()
    {
        rock = monster.CreateRock();
        rock.GetComponent<ProjectileCtrl>().power = monster.Power;
        soundMgr.PlaySkillSound();
    }

    public void ThrowRock()
    {
        rock.transform.SetParent(null);
        Vector3 dir = monster.GetPlayerPos() - rock.transform.position;
        dir.Normalize();
        rock.GetComponent<Rigidbody>().AddForce(dir * 500);
    }
}
