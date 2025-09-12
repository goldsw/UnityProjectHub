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

    public float timer;
    public float gainGunRadius;
    public float findGunDelay;
    public GameObject gunHandle;
    public GameObject[] GunItems = new GameObject[2];
    GameObject selectGun;
    GunStats[] Weapons;
    GunStats currentGun;
    int currentGunindex;
    readonly GunStats noneItem = new GunStats
    {
        bullet = null,
        fullBullet = -1,
        fireRate = 99999999999999,
        damge = 0
    };
    int gunindex = 3;
    int groundLayerMask;

    private void Start()
    {
        // 배열 생성 & 내부 원소 초기화
        Weapons = new GunStats[2];
        for (int i = 0; i < Weapons.Length; i++)
            Weapons[i] = new GunStats();

        groundLayerMask |= (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Obstacles"));

        installGun(); // 처음 무기 세팅
        currentGun = FindCurrentGun(gunindex);
    }

    public void installGun()
    {
        for (int i = 0; i <= 1; i++)
        {
            Item item = null;
            if (GunItems[i].transform.childCount >= 1)
                item = GunItems[i].GetComponentInChildren<InventoryItem>().item;
            if (item == null)
            {
                Debug.Log("무기가 없습니다");
                initializeGunState(i, noneItem);
                continue;
            }

            initializeGunState(i, item);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentGun = Weapons[0];
            Debug.Log("메인 무기 선택: " + (currentGun.bullet ? currentGun.bullet.name : "null"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentGun = Weapons[1];
            Debug.Log("보조 무기 선택: " + (currentGun.bullet ? currentGun.bullet.name : "null"));
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            TryfireGun(currentGun);
        }
        installGun();
    }

    private GunStats FindCurrentGun(int index)
    {
        if (index != 3)
            return Weapons[index];

        for (int i = 0; i <= 1; i++)
        {
            if (GunItems[i] != null)
            {
                return Weapons[i];
            }
        }
        return new GunStats();
    }

    public void TryfireGun(GunStats currentgun)
    {
        if (currentgun == null || currentgun.bullet == null) return;

        if (timer > currentgun.fireRate)
        {
            fireGun(currentgun);
            timer = 0f;
        }
    }

    public void fireGun(GunStats currentgun)
    {
        Ray ray;
        RaycastHit rayinfo;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out rayinfo, 10000f, groundLayerMask))
        {
            GameObject bullet = Instantiate(
                currentgun.bullet,
                transform.position,
                Quaternion.LookRotation(rayinfo.point - transform.position)
            );
            bulletBehaviour bulletScript = bullet.GetComponent<bulletBehaviour>();
            bulletScript.stats.damge = currentGun.damge;
            bulletScript.fireStart(rayinfo.point);
        }
    }

    public void initializeGunState(int index, Item item)
    {
        Weapons[index].fullBullet = item.maxbullet;
        Weapons[index].fireRate = item.fireSpeed;
        Weapons[index].damge = item.damge;
        Weapons[index].bullet = item.bullet;
    }

    public void initializeGunState(int index, GunStats item)
    {
        Weapons[index].fullBullet = item.fullBullet;
        Weapons[index].fireRate = item.fireRate;
        Weapons[index].damge = item.damge;
        Weapons[index].bullet = item.bullet;
    }

    [System.Serializable]
    public class GunStats
    {
        public int fullBullet;
        public float fireRate;
        public float damge;
        public GameObject bullet;
    }
}
