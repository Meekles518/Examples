using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear_control : MonoBehaviour
{
    
    public Itemdata.Itemtype type;

    //Gloves와 Shoes의 수치, 모든 무기의 속도를 올려줌
    public float Rate;

    
    //Weapon_control의 Init함수처럼, 장비를 관리하는 함수
    //장비의 첫 생성 시, 작동시킬 함수
    public void Init(Itemdata data) {

        //Player의 children으로 넣을 때의 이름 설정
        name = "Gear" + data.Item_id;

        //Player의 transform을 부모로 가지게 함
        transform.parent = GameManager.instance.Player.transform;

        //localPosition을 0벡터로 하여, 부모인 Player의 좌표를 항상 따라가게 함
        transform.localPosition = Vector3.zero;

        //Itemdata에 기입되어있는 type, Dmgs를 저장(Dmgs에는 레벨 별 수치가 적혀있음)
        type = data.Item_type;
        Rate = data.Dmgs[0];


        Gear_update();

    }


    //레벨 업 시 Rate를 변경시키는 함수
    //이미 가지고 있는 장비가 레벨업 시 실행할 함수
    public void Lvl_up(float rate) {

        //
        this.Rate = rate;
        Gear_update();


    }



    //무기의 속도를 올려주는 함수
    void Rate_up() {

        //Parent, 즉 Player에서 GetComponentsInChildren을 통해 자신과 같은 위치의 객체에서 script 가져오기
        //무기는 근접과 원거리 두 종류가 존재하니 배열로 받아줌
        Weapon_control[] Weapons = transform.parent.GetComponentsInChildren<Weapon_control>();

        //근접 무기와 원거리 무기에 따라 Rate 증가 적용 방법이 다름
        foreach(Weapon_control weapon in Weapons) {

            //weapon의 Id를 통해 무기의 종류를 확인
            switch(weapon.Id) {

                //Id == 0, 근접 무기일 경우
                case 0:

                    //기존 회전 속도에 Rate 비율만큼 추가해주어 회전 속도 증가
                    weapon.Weapon_speed += weapon.Weapon_speed * Rate;

                    break;

                //원거리 무기일 경우
                default:

                    //기존 연사 딜레이에 Rate 비율만큼 감소시켜 연사 속도 증가의 효과
                    weapon.Weapon_speed -= weapon.Weapon_speed * Rate;


                    break;
            }
        }
      }


    //Shoes 레벨업 시 Player의 Speed를 올려주는 함수
    public void Speed_up() {

        //Player의 이동속도를 일정 Rate만큼 증가
        GameManager.instance.Player.Player_speed +=  GameManager.instance.Player.Player_speed * Rate;

    }


    //Gear의 종류에 따른 함수를 호출해주는 함수
    public void Gear_update() {

        //이 Script의 Init함수를 거치며 저장된 type이 무엇인지에 따라
        //실행해야 할 함수가 달라짐 
        switch (type) {

            case Itemdata.Itemtype.Glove:

                Rate_up();
                break;
            

            case Itemdata.Itemtype.Shoes:

                Speed_up();
                break;

        }
    }





}
