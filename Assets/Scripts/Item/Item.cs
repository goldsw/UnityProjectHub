using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{

    [Header("모든 아이템")]
    public GameObject obj;
    public ItemType type;
    public ActionType actionType;
    public Animator anim;

    [Header("총의 경우")]
    public GameObject bullet;
    public GameObject magazine; //사용할까?
    public int maxbullet;
    public float fireSpeed;

    [Header("겹칠 수 있는가?")]
    public bool stackable;
    public int currentStack;
    public int maxStack;

    [Header("UI")]
    public Sprite image;
    public Sprite equipImage;


    public enum ItemType
    {
        Gun,
        Usingitem
    }
    public enum ActionType
    {
        Eat,//에너지 드링크?
        injection,//주사하다
        Throw,//수류탄?
        Cure,//붕대? 구급상자?
        Secondary_Weapon,//총일 경우
        Main_Weapon
    }


}
