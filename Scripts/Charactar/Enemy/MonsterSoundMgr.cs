using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSoundMgr : MonoBehaviour
{
    [SerializeField] AudioClip attack;
    [SerializeField] AudioClip damage;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip skill1;
    [SerializeField] AudioClip skill2;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAttackSound()
    {
        audioSource.clip = attack;
        audioSource.Play();
    }

    public void PlayDamageSound()
    {
        audioSource.clip = damage;
        audioSource.Play();
    }

    public void PlayDeathSound()
    {
        audioSource.clip = death;
        audioSource.Play();
    }

    public void PlaySkillSound()
    {
        if (skill1 != null)
        {
            audioSource.clip = skill1;
            audioSource.Play();
        }
    }

    public void PlaySkillSound2()
    {
        if (skill2 != null)
        {
            audioSource.clip = skill2;
            audioSource.Play();
        }
    }
}
