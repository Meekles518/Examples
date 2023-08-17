using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Adv : MonoBehaviour
{
   //속성을 만들어보기
   //Speed 기초값을 늘려주는 속성
   public static float Speed {

        //Get을 이용해, Speed 이용 시(?) 특정 값을 return해줌?
        //삼향 연산자로, 특정 idx일 때만 1.1f 주기
        get {return GameManager.instance.Player_idx == 0 ? 1.1f : 1f;}

   }

    //Range Weapon 기초 Dmg를 늘려주는 속성
    public static float RangeWeaponDmg {


        get {return GameManager.instance.Player_idx == 1 ? 1.1f : 1f;}

    }



}
