using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class WeaponSystem : MonoBehaviour
{
    public PlayerIO _playerIO;
    private int secondaryWeaponMasks;
    private int mainWeaponMasks;

    //¹«±â ÀÎµ¦½º
    [SerializeField]
    private int mainIndex = 0;
    [SerializeField]
    private int secondIndex = 1;

    public float timer;
    public float gainGunRadius;
    public float findGunDelay;
    public GameObject gunHandle;
    public GameObject[] GunItems = new GameObject[2];
    GameObject selectGun;
    GunStats[] Weapons;
    GunStats currentGun;
    GunStats noneItem = new GunStats
    {
        bullet = null,
        fullBullet = -1,
        fireRate = 99999999999999,
        damge = 0
    };
    int gunindex = 3;
    int groundLayerMask;

    // Start is called before the first frame update

    private void Start()
    {
        Weapons = new GunStats[2];
        groundLayerMask |= (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Obstacles"));

        for (int i = 0; i <= 1; i++)
        {
            Item item;
            if (!(item = GunItems[i].GetComponentInChildren<InventoryItem>().item))
            {
                initializeGunState(i, noneItem);


                continue;
            }

            Debug.Log(item.name);
            initializeGunState(i, item);

        }

        currentGun = FindCurrentGun(gunindex);



    }

    private void Update()
    {

        timer += 1 * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentGun = FindCurrentGun(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            currentGun = FindCurrentGun(1);

        if (Input.GetKey(KeyCode.Mouse0))
        {

            TryfireGun(currentGun);
        }
    }
    private GunStats FindCurrentGun(int index)
    {
        if(index != 3)
            return Weapons[index];


        for (int i = 0; i <= 1; i++)
        {
            if(GunItems[i] != null)
            {
                Debug.Log(Weapons[i].bullet);
                return Weapons[i];
            }

        }
        return new GunStats();
    }
    public void TryfireGun(GunStats currentgun)
    {
        if (timer > currentgun.fireRate)
        {
            currentGun = FindCurrentGun(gunindex);
            Debug.Log(currentgun.fireRate);
            fireGun(currentgun);
            timer = 0f;
        
        
        }
    }
    public void fireGun(GunStats currentgun)
    {
        Ray ray;
        RaycastHit rayinfo;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out rayinfo, 10000f, groundLayerMask);

        Debug.Log(currentgun.bullet);
        GameObject bullet = Instantiate(currentgun.bullet, transform.position, Quaternion.LookRotation(rayinfo.point - transform.position));
        bulletBehaviour bulletScript = bullet.GetComponent<bulletBehaviour>();
        bulletScript.stats.damge = currentGun.damge;
        bulletScript.fireStart(rayinfo.point);
    }





    public void initializeGunState(int index, Item item)
    {

        Weapons[index].fullBullet = item.maxbullet;
        Weapons[index].fireRate = item.fireSpeed;
        Weapons[index].damge = item.damge;
        Weapons[index].bullet = item.bullet;
        Debug.Log(Weapons[index].fullBullet);
        Debug.Log(Weapons[index].fireRate);
        Debug.Log(Weapons[index].damge);
        Debug.Log(Weapons[index].bullet);
    }
    public void initializeGunState(int index, GunStats item)
    {

        Weapons[index].fullBullet = item.fullBullet;
        Weapons[index].fireRate = item.fireRate;
        Weapons[index].damge = item.damge;
        Weapons[index].bullet = item.bullet;
        Debug.Log(Weapons[index].fullBullet);
        Debug.Log(Weapons[index].fireRate);
        Debug.Log(Weapons[index].damge);
        Debug.Log(Weapons[index].bullet);
    }


    public struct GunStats
    {
        public int fullBullet;
        public float fireRate;
        public float damge;
        public GameObject bullet;



    }



}
