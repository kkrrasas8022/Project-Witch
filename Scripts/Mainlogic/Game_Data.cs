using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Data : MonoBehaviour
{
    public void Save_Data()
    {

    }

    public void Load_Data()
    {

    }
}

[Serializable]
public class Data
{
    public int m_money;

    public int health_upgrade_count;
    public int attack_upgrade_count;
    public int defence_upgrade_count;
    public int critical_upgrade_count;
}
