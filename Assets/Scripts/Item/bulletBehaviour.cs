using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehaviour : MonoBehaviour
{
    // Update is called once per frame

    public bulletStats stats;
    public GameObject bulletHole;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }
    public void fireStart(Vector3 position)
    {
        StartCoroutine("fire", position);
    }


    public IEnumerator fire(Vector3 position)
    {
        while (true)
        {

            Vector3 pos = (position - transform.position).normalized;


            transform.position += pos * 10f * Time.fixedDeltaTime;


            yield return new WaitForFixedUpdate();
        }
    }

    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point;
        Quaternion normal = 
            Quaternion.LookRotation(
            contact.normal - new Vector3(0, 0, 0), Vector3.up
            ) ;

        Instantiate(bulletHole, pos, normal);
        

        Destroy(gameObject);
    }

    public struct bulletStats
    {
        public float damge;

    }





}
