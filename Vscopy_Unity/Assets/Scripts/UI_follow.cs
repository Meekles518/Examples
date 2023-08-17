using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_follow : MonoBehaviour
{
    //RectTransform을 담을 변수 선언
    RectTransform Rect;


    private void Awake() {
        //Component 가져오기
        Rect = GetComponent<RectTransform>();
    }
    
    //Player의 이동이 FixedUpdate로 구현되기에, UI의 Player 추적도 FixedUpdate로 구현
    private void FixedUpdate() {
        
        //WorldToScreenPoint를 통해, world(게임) 좌표를 Screen(UI) 좌표로 변동
        //변동된 좌표를 대입해주어서, Player의 이동을 따라가도록 함
        Rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.Player.transform.position);


    }





}
