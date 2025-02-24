using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sound_Manager : MonoBehaviour
{
    public AudioSource Audio_Source;
    public AudioClip Move_Clip;
    public AudioClip Jump_Clip;
    public AudioClip Fire_Clip;
    public AudioClip Ice_Clip;

    public void Sound_Move()
    {
        Audio_Source.clip = Move_Clip;
        Audio_Source.Play();
    }

    public void Sound_Jump()
    {
        Audio_Source.clip = Jump_Clip;
        Audio_Source.Play();
    }

    public void Sound_Fire_Attack()
    {
        Audio_Source.clip = Fire_Clip;
        Audio_Source.Play();
    }

    public void Sound_Ice_Attack()
    {
        Audio_Source.clip = Ice_Clip;
        Audio_Source.Play();
    }
}
