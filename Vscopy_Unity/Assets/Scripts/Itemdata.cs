using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//커스텀 메뉴 생성??
//ScriptableObject 를 상속받는 Class를 만들고, CreateAssetMenu를 통해 
//생성 가능
[CreateAssetMenu(fileName ="Item", menuName = "Scriptable Object/Itemdata")]
public class Itemdata : ScriptableObject
{   

    //무기의 종류를 열거 선언
    public enum Itemtype {Melee, Range, Glove, Shoes, Heal}


    //Item 에 관한 Info를 담을 변수들 선언
    [Header("Main Info")]

    public Itemtype Item_type;

    public int Item_id;

    public string Item_name;

    //TextArea 선언하면 inspector 창에서 크기 커짐
    [TextArea]
    public string Item_desc;

    public Sprite Item_icon;

    


    [Header("Level Data")]

    //레벨 별 기본 dmg, 무기 개수 cnt
    public float Base_dmg;

    public int Base_cnt;

    //Dmg, cnt를 담을 배열 선언
    public float[] Dmgs;
    public int[] Cnts;

    [Header("Weapon")]

    //무기에 맞는 Prefab, GameObject를 담을 변수
    public GameObject Project_tile;

    public Sprite Hand;



}
