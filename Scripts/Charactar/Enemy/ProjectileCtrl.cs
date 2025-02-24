using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileCtrl : MonoBehaviour
{
    public float power { get; set; }
    public float dmgRate = 1;
    [SerializeField] bool isThrough;
    float lifeTime = 10f;

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if(lifeTime < 0 )
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInParent<Charactar>().Damaged(power * dmgRate);

            if(!isThrough)
            {
                Destroy(gameObject);
            }
        }
    }
}
