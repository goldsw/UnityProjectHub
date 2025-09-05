using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamgeManager : MonoBehaviour
{
    public UnitStats unitStats;
    public Material material;
    // Start is called before the first frame update
    private void Start()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            float damge = 10f;
            unitStats.health -= damge;
            if(unitStats.health <= 0)
            {
                material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }
        }
    }
}
