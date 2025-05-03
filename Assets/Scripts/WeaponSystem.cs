using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    private int pistolMask;
    public float gainGunRadius;
    public float findGunDelay;
    private GameObject[] GunItem = new GameObject[2]; //나중에 사용할 변수
    GameObject closeGun;
    // Start is called before the first frame update
    void Start()
    {
        pistolMask = 1 << LayerMask.NameToLayer("Pistol");
        StartCoroutine(FindGun(findGunDelay));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FindGun(float delay)
    {
        while (true)
        {
            Debug.Log(closeGun);
            Debug.Log(FindGunInRedius(gainGunRadius, pistolMask).name);
            yield return new WaitForSeconds(delay);
        }

    }

    void GunColisionEnter()
    {

    }

    GameObject FindGunInRedius(float red, int hitMask)
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, red, hitMask);
        closeGun = gameObject;
        foreach(var i in targetsInViewRadius)
        {
            if((Vector3.Distance(transform.position, i.transform.position) < Vector3.Distance(transform.position, closeGun.transform.position)) || closeGun == gameObject)
            closeGun = i.gameObject;
        }
        
        return closeGun;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, gainGunRadius);
        Gizmos.color = Color.red;
        if(closeGun != null)
        Gizmos.DrawLine(transform.position, closeGun.transform.position);
    }
}
