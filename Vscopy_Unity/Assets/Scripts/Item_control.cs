using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//각 Item의 Lv을 띄워줄 UI로 화면에 띄워줄 Script
public class Item_control : MonoBehaviour
{
 
    //Itemdata의 정보를 저장할 변수
    public Itemdata Data;

    public int Level;

    //Weapon과 Gear의 생성 및 레벨업을 위해 Script를 가져와 저장할 변수
    public Weapon_control Weapon;

    public Gear_control Gear;

    //Image와 Text 변환을 위해 사용할 변수
    Image Icon;

    Text Lvl_text;

    Text Name_text;

    Text Desc_text;

    private void Awake() {
        
        //GetComponentsInChildren은 배열로 Component를 가져오고, 0 idx는 자기자신이다
        Icon = GetComponentsInChildren<Image>()[1];

        //Itemdata 에 저장되어있는 Item_icon 가져오기
        //Itemdata 에는 각 Item의 Sprite가 들어있음
        Icon.sprite = Data.Item_icon;

        //GetComponentsInChildren을 통해 Text 정보 가져와서 저장하기
        //Children 으로 Level_text, Name_text, Desc_text 순서로 존재함
        Text[] Txt = GetComponentsInChildren<Text>();
        Lvl_text = Txt[0];
        Name_text = Txt[1];
        Desc_text = Txt[2];

        //Name_text 초기화 하기
        Name_text.text = Data.Item_name;

    }

    //OnEnable 되었을 때 작동하는 함수
    private void OnEnable() {
        
        //레벨을 표시하는 txt 계속 초기화
        Lvl_text.text = "Lv." + Level;


        //Item_type에 따라 switch문으로 분류
        switch (Data.Item_type) {

            //Item_desc 에는 Item에 대한 설명이 적힌 문자열이 있음
            //문자열 중간에 {0}, {1}을 적어놓아서, Format문을 사용 가능

            //이런 방법 대신, TextMeshProUGUI 의 SetText를 활용해도 됨
            //애초에 Text 대신 TextMeshProUGUI를 사용하면 더 편할듯?

            case Itemdata.Itemtype.Range:
            case Itemdata.Itemtype.Melee:

                
                Desc_text.text = string.Format(Data.Item_desc, Data.Dmgs[Level] * 100, Data.Cnts[Level]);

                break;


            case Itemdata.Itemtype.Glove:
            case Itemdata.Itemtype.Shoes:

                Desc_text.text = string.Format(Data.Item_desc, Data.Dmgs[Level] * 100);

                break;


            case Itemdata.Itemtype.Heal:

                 Desc_text.text = string.Format(Data.Item_desc);

                break;



        }

    }


    //버튼 클릭 시 실행시킬 함수
    public void On_click() {

        //각 버튼의 Data 변수에 저장되어있는 Itemdata의 Itemtype에 따라 Switch문 
        switch (Data.Item_type) {

            case Itemdata.Itemtype.Glove:
            case Itemdata.Itemtype.Shoes:

                //첫 레벨 업 시 기어 생성
                if (Level == 0) {

                    //새 GameObject 생성 및 AddComponent로 Script 가져오기
                    //후에도 이 변수를 사용하기 위해 선언
                    GameObject New_gear = new GameObject();

                    Gear = New_gear.AddComponent<Gear_control>();
                    
                    //Init 함수에, Itemdata의 정보를 저장한 변수 Data 넣어주기
                    Gear.Init(Data);

                }

                else {
                    
                    //Itemdata에 저장되어 있는 레벨 별 Dmgs 가져오기(Glove, Shoes에서는 Rate의 용도로 사용)
                    float Rate = Data.Dmgs[Level];

                    //Gear_control의 Lvl_up 함수에 Rate 넣어서 비율 증가시키기
                    Gear.Lvl_up(Rate);

                }

                //Lv up
                Level++;
                break;

            //여러개의 case를 붙여서 사용 가능
            case Itemdata.Itemtype.Range:
            case Itemdata.Itemtype.Melee:

                //첫 레벨업 시 무기 생성
                if (Level == 0) {

                    //새로운 GameObject 선언
                    GameObject New_weapon = new GameObject();

                    //AddComponent를 통해 특정 Component 가져오고 반환하기
                    Weapon = New_weapon.AddComponent<Weapon_control>();

                    //Init 함수에, Itemdata의 정보를 저장한 변수 Data 넣어주기
                    Weapon.Init(Data);

                }

                //레벨 업 시
                else {
                                      
                    //Itemdata에 저장되어 있는 기본 무기 dmg 가져오기
                    float Weapon_dmg = Data.Base_dmg;
                    int Count = 0;


                    //Itemdata에 있는 Dmgs, Cnts 배열에 접근해, 레벨 업 시
                    //각 배열의 idx의 수치만큼 무기 강화시켜주기
                    //무기의 dmg는 곱연산으로 증가시켜주기
                    //무기의 Cnt는 더해주기(근접 무기는 갯수, 원거리 무기는 관통 수 의미)
                    Weapon_dmg += Weapon_dmg * Data.Dmgs[Level - 1];
                    Count += Data.Cnts[Level - 1];


                    //레벨 업에 따른 Weapon 강화를 해주는 함수 
                    Weapon.Lvl_up(Weapon_dmg, Count);

                }

                //Lv up
                Level++;
                break;

            case Itemdata.Itemtype.Heal :

                //GameManager에 저장되어 있는 현재 체력을, 최대 체력으로 변경해서 회복의 효과
                GameManager.instance.P_Current_hp = GameManager.instance.P_Max_hp;

                break;


        }

        

        //현재 Dmgs 의 배열의 길이는 5다. 이는 최대 레벨을 5로 구상해놓았기 때문
        //최대 레벨을 넘지 않게 조절하는 코드
        if (Level >= Data.Dmgs.Length) {

            //Button의 interactable을 false로 해, 클릭 못하게 함
            GetComponent<Button>().interactable = false;

        }
    }



}
