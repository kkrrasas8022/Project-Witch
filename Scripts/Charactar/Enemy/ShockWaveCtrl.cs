using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveCtrl : MonoBehaviour
{
    [SerializeField] float speed;
    float spreadSpeed;

    void Start()
    {
        spreadSpeed = speed * 2;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        transform.localScale = new Vector3(transform.localScale.x + (spreadSpeed * Time.deltaTime), transform.localScale.y, transform.localScale.z);
    }
}
