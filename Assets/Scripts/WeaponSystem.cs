using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class WeaponSystem : MonoBehaviour
{
    public PlayerIO _playerIO;
    private int secondaryWeaponMasks;
    private int mainWeaponMasks;
    public float gainGunRadius;
    public float findGunDelay;
    private GameObject[] GunItems = new GameObject[2];
    GameObject selectGun;
    // Start is called before the first frame update
    void Start()
    {
        _playerIO = new PlayerIO();
        secondaryWeaponMasks = 1 << LayerMask.NameToLayer("Pistol");
        mainWeaponMasks = 1 << LayerMask.NameToLayer("MachineGun");
        StartCoroutine(FindGun());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GunItems[0]);
        Debug.Log(GunItems[1]);
    }

    IEnumerator FindGun()
    {
        while (true)
        {
            selectGun = FindGunInRedius(gainGunRadius, secondaryWeaponMasks + mainWeaponMasks); // layerMask는 2진수 연산 (0101 <- 1번째 레이어와 3번째 레이어가 켜져있다)으로 하기 때문에 겹치지 않으니 + 연산을 해도 됨
            yield return new WaitForSeconds(findGunDelay);
        }

    }
    public void interactionKey(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            GunChange();
            Instantiate(GunItems[0], gameObject.transform.position, transform.rotation, transform);
        }

        
    }



    void GunChange()
    {
        if ((1 << selectGun.layer & mainWeaponMasks) != 0)
        {
            GunItems[0] = selectGun;
            Destroy(selectGun);
        }
        if ((1 << selectGun.layer & secondaryWeaponMasks) != 0)
        {
            GunItems[1] = selectGun;
            Destroy(selectGun);
        }
    }


    void GunColisionEnter()
    {

    }

    GameObject FindGunInRedius(float _rad, int _hitMask)
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, _rad, _hitMask);
        GameObject closeGun = gameObject;
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
        if(selectGun != null)
            Gizmos.DrawLine(transform.position, selectGun.transform.position);
    }
}
