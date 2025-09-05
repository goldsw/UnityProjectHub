using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehaviour : MonoBehaviour
{
    // Update is called once per frame
    private void Start()
    {
        
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
    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
            Debug.Log("Ä¡Áî»÷µå");
        Destroy(gameObject);
    }


}
