using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class WeaponSystem : MonoBehaviour
{
    public PlayerIO _playerIO;
    private int secondaryWeaponMasks;
    private int mainWeaponMasks;

    //무기 인덱스
    [SerializeField]
    private int mainIndex = 0;
    [SerializeField]
    private int secondIndex = 1;

    public float gainGunRadius;
    public float findGunDelay;
    public GameObject gunHandle;
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
            
        }

        
    }



    void GunChange()
    {
        if ((1 << selectGun.layer & mainWeaponMasks) != 0)
        {
            if (GunItems[mainIndex] != null)
            {
                WeaponIsEnable(mainIndex);
            }
            if(GunItems[secondIndex] == true)//만약 손에 보조무기가 들려있다면. 보조무기를 집어넣음
            {
                GunItems[secondIndex].SetActive(false);
            }
            GunItems[mainIndex] = selectGun;
            BoxCollider _colider = GunItems[0].GetComponent<BoxCollider>();
            _colider.enabled = false;
            GunItems[mainIndex].transform.parent = gunHandle.transform;
            GunItems[mainIndex].transform.position = gunHandle.transform.position;
        }
        if ((1 << selectGun.layer & secondaryWeaponMasks) != 0)
        {
            if(GunItems[1] != null)
            {
                WeaponIsEnable(secondIndex);
            }
            if (GunItems[mainIndex] == true)
            {
                GunItems[mainIndex].SetActive(false);
            }
            GunItems[secondIndex] = selectGun;
            BoxCollider _colider = GunItems[secondIndex].GetComponent<BoxCollider>();
            _colider.enabled = false;
            GunItems[secondIndex].transform.parent = gunHandle.transform;
            GunItems[secondIndex].transform.position = gunHandle.transform.position;
            //Destroy(selectGun);
        }
    }
    void WeaponIsEnable(int weaponindex)
    {
        BoxCollider _colider = GunItems[weaponindex].GetComponent<BoxCollider>();
        GunItems[weaponindex].transform.parent = null;
        GunItems[weaponindex].SetActive(true);
        _colider.enabled = true;
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
