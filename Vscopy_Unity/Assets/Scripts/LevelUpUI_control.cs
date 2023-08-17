using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUI_control : MonoBehaviour
{
    
    //UI의 RectTransform을 받을 변수
    RectTransform Rect;

    //이 Script를 가질 Level_up_image의 Children으로 들어가 있는 Item들을 받기 위한 배열
    Item_control[] Item;

    private void Awake() {
        
        //Component 가져오기
        Rect = GetComponent<RectTransform>();

        Item = GetComponentsInChildren<Item_control>(true);


    }

    //UI를 보여주는 함수, enable 대신 scale 크기 조정으로 해보기
    public void Show_UI() {

        //레벨 업 시 화면에 띄울 무작위 장비 3개를 정하고 나타내는 함수
        Random_item();

        //Scale을 1,1,1 로 늘려 보이게 하기
        Rect.localScale = Vector3.one;

        //GameManager의 Stop_game 함수를 호출해, 게임의 시간 정지
        GameManager.instance.Stop_game();
    }

    //UI를 감추는 함수
    public void Hide_UI(){

        //Scale을 0, 0, 0으로 줄여 안보이게 하기
        Rect.localScale = Vector3.zero;

        //GameManager의 Resume_game 함수를 호출해, 게임의 시간 다시 진행
        GameManager.instance.Resume_game();

    }

    //게임 시작 시 기본 무기 지급을 위해, Item_control에 있는 On_click 함수를 작동시키는 함수
    public void Select(int idx) {

        Item[idx].On_click();

    }

    //레벨업 시 랜덤하게 장비 강화 UI를 띄우게 해주는 함수
    public void Random_item() {


        //아이템 비활성화
        foreach (Item_control item in Item) {

            //foreach문으로, Children으로 있는 모든 Item들을 비활성화 
            item.gameObject.SetActive(false);

        }

        //무작위 3개 장비 뽑기

        //길이 3의 int 배열 선언
        int[] random = new int[3];

        //장비들 중 무작위로 3개를 뽑기 위한 while 구문
        while (true) {

            //5 C 3 의 경우의 수 중 하나 무작위로 뽑아주는 내장함수 없나?
            //일단은 일일이 Random으로 돌리고, 3개가 전부 같지 않을 때만 break하도록 하자
            random[0] = Random.Range(0, Item.Length);
            random[1] = Random.Range(0, Item.Length);
            random[2] = Random.Range(0, Item.Length);

            if (random[0] != random[1] && random[1] != random[2] && random[0] != random[2]) {
                break;
            }

        }

        //무작위로 뽑은 3개의 장비 활성화 시키는 구문
        for (int idx = 0; idx < random.Length; idx++) {

            //무작위로 뽑은 3개의 장비를 for문을 통해 하나씩 활성화 시켜주기
            Item_control Ran_item = Item[random[idx]];

            //무작위로 뽑은 아이템의 레벨이, Itemdata의 Dmgs 배열의 길이와 같거나 더 큰경우
            //(Itemdata의 Dmgs와 Cnts 배열의 길이 = 최대 레벨)
            if (Ran_item.Level >= Ran_item.Data.Dmgs.Length) {

                //대신 소비 아이템을 화면에 띄우도록 함. Level_up_image > Panel > Level_Up_group에 보면
                //Children이 5개 있는데, 마지막 자식이 소비 아이템임 
                Item[Item.Length - 1].gameObject.SetActive(true);

            }

            else {
                //장비 활성화 시키기
                Ran_item.gameObject.SetActive(true);
            }

        }



    }




}
