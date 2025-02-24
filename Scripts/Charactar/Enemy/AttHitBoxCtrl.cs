using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttHitBoxCtrl : MonoBehaviour
{
    public GameObject attHitBox;
    public MonsterSoundMgr soundMgr;
    
    public void ActiveAttHitBox()
    {
        soundMgr.PlayAttackSound();
        attHitBox.SetActive(true);
    }

    public void DeactiveAttHitBox()
    {
        attHitBox.SetActive(false);
    }
}
